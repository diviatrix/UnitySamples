using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// actually it is not real chunk.. 
// just class for block of cubes Generator
// you set size
public class Chunk : MonoBehaviour {

    // each axis size of chunk
    // i.e. if size is 8 there will be 8*8*8=512 blocks inside.
    public int size;

    // prefab of block
    public GameObject blockPrefab;

    // 3 axis array to store block, actually its not needed.
    public GameObject[,,] blocks;
    
    // Use this for initialization after you set all settings (i.e. MapGenerator does it)
    // creates chunk with blocks
    public void Initialize () {
        
        // initialize array to add or remove blocks. 
        blocks = new GameObject[size, size, size];

        for(int x = 0;  x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    // calculate vector3 position to spawn block at exact place. 
                    // Position of block depends on where Chunk object is and what is size of chunk
                    Vector3 spawnVector = new Vector3(transform.position.x*size + x, transform.position.y*size + y, transform.position.z*size + z);
                    
                    // spawn new block with other method
                    GameObject newBlock = NewBlock(spawnVector);

                    // make spawned block child for Chunk
                    newBlock.transform.SetParent(transform);

                    // add block to array
                    blocks[x, y, z] = newBlock;
                }
            }
        }
    }

    public void AddColliders()
    {
        foreach (GameObject go in blocks)
        {
            go.AddComponent<BoxCollider>();
        }
    }


    // Function to create block, it accepts Vector3 as spawn position.
    GameObject NewBlock(Vector3 coords)
    {
        // You can use CreatePrimitive for newBlock creation, or use your own object.
        // Just make sure your object size equals Size 1x1x1 primitive cube. 

        // create empty GameObject to work with it
        GameObject newBlock;

        // Check if blockPrefab assigned. 
        // and spawn it.              
        if (blockPrefab != null){
            newBlock = GameObject.Instantiate(blockPrefab);
        }
        else 
        {
            // if not, just spawn primitive from unity engine  
            newBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);            
        }

        // set block position
        newBlock.transform.position = coords;

        // set block name with position in array
        newBlock.name = "Block: " + coords; 
        
        // randomize color of block
        newBlock.GetComponent<Renderer>().material.color = new Color(Random.value,Random.value,Random.value,Random.value);

        // return generated block back
        return newBlock;        
    }
}
