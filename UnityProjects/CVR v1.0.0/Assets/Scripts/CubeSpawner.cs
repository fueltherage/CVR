using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour {

    private static CubeSpawner _instance;
    private static bool _init = false;



    public static CubeSpawner Instance
    {
        get
        {
            if (!_init)
            return Initialize();
            else return _instance;
        }
    }
    private static CubeSpawner Initialize()
    {
        if (!_init)
        {
            GameObject g = new GameObject("CubeSpanwer GO");
            _instance = g.AddComponent<CubeSpawner>();
            _init = true;            
        }
        return _instance;
    }
	// Use this for initialization
	void Start () {	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// Spawn a floating cube
    /// </summary>
    /// <param name="_pos">World Position</param>
    /// <param name="_chunk">Chunk</param>    
    /// <param name="_type">Layer Type</param>
    public void SpawnCubeAt(Vector3 _pos, ref VoxelSystemChunkGreedy _chunk, int _type)
    {
        //Spawn a cubed at _pos world position,
        //use the info in _chunk to generate the proper UV Map
        //_VoxPos is the position of the voxel in the system
        //Use type to choose the right material


    }
}
