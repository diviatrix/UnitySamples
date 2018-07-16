using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    // public values of map 
    // but actually this map is chunk of chunks =\
    public Vector3 mapSize;
    public int chunkSize;
    public GameObject blockPrefab;

	void Start () {
		for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    // create new Empty game object
                    GameObject newChunkGo = new GameObject();

                    // Set its position in world space correctly
                    newChunkGo.transform.position = new Vector3(x,y,z);

                    // craate new chunk
                    Chunk newChunk = newChunkGo.AddComponent<Chunk>();

                    // set size from settings above
                    newChunk.size = chunkSize;

                    // set prefab from settings above, could be null
                    newChunk.blockPrefab = blockPrefab;

                    // set name of chunk according to position
                    newChunkGo.name = "Chunk " + newChunkGo.transform.position;

                    // Generate chucnk with desired settings
                    newChunk.Initialize();
                    newChunk.AddColliders();
                }
            }
        }
	}
}
