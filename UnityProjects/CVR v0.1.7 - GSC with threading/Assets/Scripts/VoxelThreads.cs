using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

public class VoxelThreads : MonoBehaviour {

	//Loom Code
	#region LoomCode
	void OnApplicationQuit()
	{
		_quitting = true;
	}
	static bool _quitting;
	private static VoxelThreads _current;
	private int _count;

	private static List<QeueuItem> chunksGenerating = new List<QeueuItem>();

	private static Queue<QeueuItem> p1Chunks = new Queue<QeueuItem>();
	private static Queue<QeueuItem> p2Chunks = new Queue<QeueuItem>();
	private static Queue<QeueuItem> p3Chunks = new Queue<QeueuItem>();
	public static int MaxVoxelMeshUpdateThreads = 16;


	public int ActiveThreads =0;
	public static VoxelThreads Current
	{
		get
		{
			Initialize();
			return _current;
		}
	}
	static bool _initialized;
	static int _threadId;	
	public static void Initialize()
	{
		if (!Application.isPlaying || _quitting)
			return;
		var go = !_initialized;
		if (!go && _threadId == Thread.CurrentThread.ManagedThreadId && _current == null)
			go = true;
		
		if (go)
		{
			foreach (var voxelThreads in Resources.FindObjectsOfTypeAll(typeof(VoxelThreads)).Cast<VoxelThreads>())
				DestroyImmediate(voxelThreads.gameObject);
			var g = new GameObject("VoxelThreads");
			_current = g.AddComponent<VoxelThreads>();
			Instantiate(g, new Vector3(0,0,0), Quaternion.identity);
			_initialized = true;
			_threadId = Thread.CurrentThread.ManagedThreadId;
			
		}
		chunksGenerating.Capacity=MaxVoxelMeshUpdateThreads;
		
	}	
	void OnDestroy()
	{
		_actions.Clear();
		_delayed.Clear();
		if (_current == this)
		{
			_initialized = false;
		}
	}
	
	private readonly List<Action> _actions = new List<Action>();
	public class DelayedQueueItem
	{
		public float time;
		public Action action;
		public string name;
	}
	private readonly List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
	
	public static void QueueOnMainThread(Action action, string name)
	{
		QueueOnMainThread(action, 0, name);
	}	
	public static void QueueOnMainThread(Action action, float time, string name)
	{
		if (!Application.isPlaying)
			return;
		if (Math.Abs(time - 0) > float.Epsilon || !string.IsNullOrEmpty(name))
		{
			lock (Current._delayed)
			{
				DelayedQueueItem existing = null;
				if (!string.IsNullOrEmpty(name))
					existing = Current._delayed.FirstOrDefault(d => d.name == name);
				if (existing != null)
				{
					existing.time = Time.time + time;
					return;
				}
				var queueItem = new DelayedQueueItem();
				queueItem.name = name;
				queueItem.time = Time.time + time;
				queueItem.action = action;
				Current._delayed.Add(queueItem);
			}
		}
		else
		{
			lock (Current._actions)
			{
				Current._actions.Add(action);
			}
		}	
	}
	/// <summary>
	/// Queues an action on the main thread
	/// </summary>
	/// <param name='action'>
	/// The action to execute
	/// </param>
	public static void QueueOnMainThread(Action action)
	{
		QueueOnMainThread(action, 0f);
	}
	/// <summary>
	/// Queues an action on the main thread after a delay
	/// </summary>
	/// <param name='action'>
	/// The action to run
	/// </param>
	/// <param name='time'>
	/// The amount of time to delay
	/// </param>
	public static void QueueOnMainThread(Action action, float time)
	{
		QueueOnMainThread(action, time, null);
	}	
	/// <summary>
	/// Runs an action on another thread
	/// </summary>
	/// <param name='action'>
	/// The action to execute on another thread
	/// </param>
	public static void RunAsync(Action action)
	{
		var t = new Thread(RunAction)
		{
			Priority = System.Threading.ThreadPriority.Normal
		};
		t.Start(action);
	}	
	private static void RunAction(object action)
	{
		((Action)action)();
	}
	
	readonly Action[] _toRun = new Action[4000];	



	struct QueuedChunk{
		public bool Active;

		public VoxelSystemChunkGreedy chunk;
	}
	QueuedChunk[] qChunks = new QueuedChunk[MaxVoxelMeshUpdateThreads];
	void QuChunk(ref VoxelSystemChunkGreedy _chunk)
	{
		for(int i =0; i < MaxVoxelMeshUpdateThreads; i++)
		{
			if(!qChunks[i].chunk.Generating)
			{

				qChunks[i].chunk = _chunk;
			}
		}
	}
	void Update()
	{
//		for(int i =0; i<MaxVoxelMeshUpdateThreads; i++)
//		{
//			if(qChunks[i].Active)
//			if(qChunks[i].Seated)
//			if(!qChunks[i].chunk.Generating)
//			if(!qChunks[i].chunk.MeshBaked)
//			{
//				qChunks[i].chunk.GenerateThisMesh();
//				qChunks[i].Active = false;
//			}
//		}

		Profiler.BeginSample("GenerateThisMesh");
		for (int i = 0; i < chunksGenerating.Count; i++) {
			if(!chunksGenerating[i].Chunk.Generating)
			{
				QueueOnMainThread(chunksGenerating[i].GenMeshN);
				chunksGenerating.Remove(chunksGenerating[i]);
				ActiveThreads--;
			}
		}
		Profiler.EndSample();
		Profiler.BeginSample("p1Chunks queue");
		while (p1Chunks.Count!=0 && chunksGenerating.Count < MaxVoxelMeshUpdateThreads)
		{

			if(!p1Chunks.Peek().Chunk.needsUpdating)
			{
				p1Chunks.Dequeue();
				continue;
			}
			if(!chunksGenerating.Contains(p1Chunks.Peek()))
			{
				chunksGenerating.Add(p1Chunks.Dequeue());
				RunAsync(chunksGenerating[chunksGenerating.Count-1].UpdateN);
				ActiveThreads++;

			}else p1Chunks.Dequeue();
		}
		Profiler.EndSample();
		Profiler.BeginSample("p2Chunks queue");
		while (p2Chunks.Count!=0 && chunksGenerating.Count < MaxVoxelMeshUpdateThreads)
		{
			if(!p2Chunks.Peek().Chunk.needsUpdating)
			{
				p2Chunks.Dequeue();
				continue;
			}
			if(!chunksGenerating.Contains(p2Chunks.Peek()))
			{
				chunksGenerating.Add(p2Chunks.Dequeue());
				RunAsync(chunksGenerating[chunksGenerating.Count-1].UpdateN);
				ActiveThreads++;

			}else p2Chunks.Dequeue();
		}
		Profiler.EndSample();
		Profiler.BeginSample("p3Chunks queue");
		while (p3Chunks.Count!=0 && chunksGenerating.Count < MaxVoxelMeshUpdateThreads)
		{
			if(!p3Chunks.Peek().Chunk.needsUpdating)
			{
				p3Chunks.Dequeue();
				continue;
			}
			if(!chunksGenerating.Contains(p3Chunks.Peek()))
			{
				chunksGenerating.Add(p3Chunks.Dequeue());
				RunAsync(chunksGenerating[chunksGenerating.Count-1].UpdateN);
				ActiveThreads++;


			}else p3Chunks.Dequeue();
		}
		Profiler.EndSample();
		//		if (Current != this)
		//		{
		//			if (Application.isPlaying)
		//				DestroyImmediate(gameObject);
		//			return;
		//		}
		Profiler.BeginSample("Rest of VoxelThread Update");
		if (!Application.isPlaying)
		{
			_actions.Clear();
			_delayed.Clear();
			return;
		}
		var count = Mathf.Min(_actions.Count, 4000);
		lock (_actions)
		{
			_actions.CopyTo(0, _toRun, 0, count);
			if (count == _actions.Count)
				_actions.Clear();
			else
				_actions.RemoveRange(0, count);
		}
		for (var i = 0; i < count; i++)
		{
			try
			{
				_toRun[i]();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}
		
		lock (_delayed)
		{
			count = 0;
			for (var i = _delayed.Count - 1; i >= 0 && count < 3999; i--)
			{
				if (!(_delayed[i].time <= Time.time))
				{
					continue;
				}
				_toRun[count++] = _delayed[i].action;
				_delayed.RemoveAt(i);
			}
		}
		
		for (var i = 0; i < count; i++)
		{
			try
			{
				_toRun[i]();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}
		Profiler.EndSample();
	}	

	void OnLevelWasLoaded()
	{
		_actions.Clear();
		_delayed.Clear();
	}
	#endregion
	struct QeueuItem
	{
		public VoxelSystemChunkGreedy Chunk;
		public System.Action UpdateN;
		public System.Action GenMeshN;
	}
	
	public static void QueueVoxMeshUpdate(ref VoxelSystemChunkGreedy chunk,System.Action nUpdateMesh, System.Action nGenerateMesh ,  int priority)
	{

		QeueuItem q = new QeueuItem();
	

		q.GenMeshN += nGenerateMesh;
		q.UpdateN += nUpdateMesh;	
		q.Chunk = chunk;
		q.UpdateN += chunk.UpdateMesh;
		q.GenMeshN += chunk.GenerateThisMesh;
		switch(priority)
		{
			case 1:
				//if(!p1Chunks.Contains(chunk))
				p1Chunks.Enqueue(q);
				//else Debug.Log("Threading Warning: Chunk - "+chunk.chunkPos.ToString() + " attempted to queue itself more then once.");
				break;
			case 2:
				//if(!p2Chunks.Contains(chunk))
				p2Chunks.Enqueue(q);
				//else Debug.Log("Threading Warning: Chunk - "+chunk.chunkPos.ToString() + " attempted to queue itself more then once.");
				break;
			case 3:
				//if(!p3Chunks.Contains(chunk))
				p3Chunks.Enqueue(q);
				//else Debug.Log("Threading Warning: Chunk - "+chunk.chunkPos.ToString() + " attempted to queue itself more then once.");
				break;
		}

	}

	public static void QueueVoxMeshUpdate(ref VoxelSystemChunkGreedy chunk, int priority)
	{
		if(!chunk.Generating)
		{
			QeueuItem q = new QeueuItem();
			q.Chunk = chunk;
			q.GenMeshN = q.Chunk.GenerateThisMesh;
			q.UpdateN = q.Chunk.UpdateMesh;
			switch(priority)
			{
				case 1:
					//if(!p1Chunks.Contains(chunk))
					p1Chunks.Enqueue(q);
					//else Debug.Log("Threading Warning: Chunk - "+chunk.chunkPos.ToString() + " attempted to queue itself more then once.");
					break;
				case 2:
					//if(!p2Chunks.Contains(chunk))
					p2Chunks.Enqueue(q);
					//else Debug.Log("Threading Warning: Chunk - "+chunk.chunkPos.ToString() + " attempted to queue itself more then once.");
					break;
				case 3:
					//if(!p3Chunks.Contains(chunk))
					p3Chunks.Enqueue(q);
					//else Debug.Log("Threading Warning: Chunk - "+chunk.chunkPos.ToString() + " attempted to queue itself more then once.");
					break;
			}
		}	
	}

}
