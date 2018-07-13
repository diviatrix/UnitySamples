using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    public int size;
    public GameObject blockPrefab;
    // Use this for initialization
    public void Initialize () {
        
        GameObject[,,] blocks = new GameObject[size, size, size];

        for(int x = 0;  x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    Vector3 spawnVector = new Vector3(transform.position.x*size + x, transform.position.y*size + y, transform.position.z*size + z);
                    GameObject newBlock = NewBlock(spawnVector);
                    newBlock.transform.SetParent(transform);

                    blocks[x, y, z] = newBlock;
                }
            }
        }
    }

    GameObject NewBlock(Vector3 coords)
    {
        //GameObject newBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject newBlock = GameObject.Instantiate(blockPrefab);
        newBlock.transform.position = coords;
        newBlock.name = "Block: " + coords; 
        
        newBlock.GetComponent<Renderer>().material.color = new Color(Random.value,Random.value,Random.value,Random.value);
        return newBlock;
        
    }
	
	// Update is called once per frame
	void Update () {
	}
}
