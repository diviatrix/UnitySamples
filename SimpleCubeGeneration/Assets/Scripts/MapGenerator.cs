using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Vector3 mapSize;
    public int chunkSize;
    public GameObject blockPrefab;
	// Use this for initialization
	void Start () {
		for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    GameObject newChunkGo = new GameObject();
                    newChunkGo.transform.position = new Vector3(x,y,z);
                    var newChunk = newChunkGo.AddComponent<Chunk>();
                    newChunk.size = chunkSize;
                    newChunk.blockPrefab = blockPrefab;
                    newChunkGo.name = "Chunk " + newChunkGo.transform.position;
                    newChunk.Initialize();
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
