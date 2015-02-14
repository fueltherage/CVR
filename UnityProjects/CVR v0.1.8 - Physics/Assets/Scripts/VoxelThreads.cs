using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;
[ExecuteInEditMode]
public class VoxelThreads : MonoBehaviour {

    static bool _quitting;
    public static VoxelThreads _current;
    private int _count;
    
    private static List<QeueuItem> chunksGenerating = new List<QeueuItem>();
    
    private static Queue<QeueuItem> p1Chunks = new Queue<QeueuItem>();
    private static Queue<QeueuItem> p2Chunks = new Queue<QeueuItem>();
    private static Queue<QeueuItem> p3Chunks = new Queue<QeueuItem>();
    public static int MaxVoxelMeshUpdateThreads = 6;

    public int ActiveThreads =0;

    public static bool _initialized;
    static int _threadId; 


	//Loom Code
	#region LoomCode
	void OnApplicationQuit()
	{
		_quitting = true;
	}
	
	public static VoxelThreads Current
	{
		get
		{
			Initialize();
			return _current;
		}
	}

	public static void Initialize()
	{
		if (!Application.isPlaying || _quitting)
			return;

		bool go = false;
		if (!_initialized && _current == null)
			go = true;
		
		if (go)
		{
			foreach (var voxelThreads in Resources.FindObjectsOfTypeAll(typeof(VoxelThreads)).Cast<VoxelThreads>())
				DestroyImmediate(voxelThreads.gameObject);
			GameObject g = new GameObject("VoxelThreads GO");

			_current = g.AddComponent<VoxelThreads>();
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
  
	void Update()
	{
		Profiler.BeginSample("GenerateThisMesh");
		for (int i = 0; i < chunksGenerating.Count; i++) {
			if(!chunksGenerating[i].Chunk.Generating && !chunksGenerating[i].Chunk.MeshBaking)
			{
				//Debug.Log ("Chunk "+ chunksGenerating[i].Chunk.gameObject.name +" finished");
				lock(chunksGenerating[i].Chunk){
					QueueOnMainThread(chunksGenerating[i].GenMeshN);
				}
				chunksGenerating[i].Chunk.queuedForUpdate = false;
				
				chunksGenerating.Remove(chunksGenerating[i]);
				ActiveThreads--;
				i = i - 1;
			}
		}
		Profiler.EndSample();
		Profiler.BeginSample("p1Chunks queue");
		int countQ = 0;//used in qeue checking
		while (p1Chunks.Count!=0 && chunksGenerating.Count < MaxVoxelMeshUpdateThreads)
		{
			if(!p1Chunks.ElementAt(countQ).Chunk.Generating&& !p1Chunks.ElementAt(countQ).Chunk.MeshBaking )
			{
				chunksGenerating.Add(p1Chunks.Dequeue());
				RunAsync(chunksGenerating[chunksGenerating.Count-1].UpdateN);
				ActiveThreads++;
			}else countQ++;
			if(countQ>=p1Chunks.Count-1)break;
		}
		Profiler.EndSample();
		Profiler.BeginSample("p2Chunks queue");
		countQ = 0;
		while (p2Chunks.Count!=0 && chunksGenerating.Count < MaxVoxelMeshUpdateThreads)
		{

			if(!p2Chunks.ElementAt(countQ).Chunk.Generating&& !p2Chunks.ElementAt(countQ).Chunk.MeshBaking)
			{
				chunksGenerating.Add(p2Chunks.Dequeue());
				RunAsync(chunksGenerating[chunksGenerating.Count-1].UpdateN);
				ActiveThreads++;
			}else countQ++;
			if(countQ>=p2Chunks.Count)break;
		}
		Profiler.EndSample();
		Profiler.BeginSample("p3Chunks queue");
		countQ =0;
		while (p3Chunks.Count!=0 && chunksGenerating.Count < MaxVoxelMeshUpdateThreads)
		{
			if(!p3Chunks.ElementAt(countQ).Chunk.Generating&& !p3Chunks.ElementAt(countQ).Chunk.MeshBaking)
			{
				chunksGenerating.Add(p3Chunks.Dequeue());
				RunAsync(chunksGenerating[chunksGenerating.Count-1].UpdateN);
				ActiveThreads++;
			}else countQ++;
			if(countQ>=p3Chunks.Count)break;

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
	
	public void QueueVoxMeshUpdate(ref VoxelSystemChunkGreedy chunk,System.Action nUpdateMesh, System.Action nGenerateMesh,  int priority)
	{
		if(!chunk.queuedForUpdate)
		{
			chunk.queuedForUpdate = true;

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

	}

	public void QueueVoxMeshUpdate(ref VoxelSystemChunkGreedy chunk, int priority)
	{
		if(!chunk.queuedForUpdate)
		{
			chunk.queuedForUpdate = true;
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
		}else Debug.Log("Chunk Update Rejected");	
	}

}
