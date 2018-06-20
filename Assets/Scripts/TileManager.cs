addssusing System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    //author: Arpit; Changes: Adding infinite tiles in the game

    //list holding prefabs
    public GameObject[] tilePrefabs;

    //to track the player
    private Transform playerTransform;

    //spawn of tile updation variable
    private float spawnZ = -5.0f;

    //length of each tile in the prefab; each of the prefab in folder lanes is of length 20 units
    private float tileLength = 20.0f;

	private float tileLength40 = 40.0f;
    //how many tiles we want to see at any time in the background at one time
    private int amnTilesOnScreen = 10;

    //list of active game tiles
    private List<GameObject> activeTiles;

    //safe to destroy tiles after these units so player does not fall into empty space
    private float safeToDelete = 25.0f;

    //flag to save index to prefab
    private int lastPrefabIndex = 0;

    /// <summary>
    //change by arpit
    /// </summary>
    private int flag = 0;
	private int temp = 0;
    // Use this for initialization
    void Start()
    {

        //instantiate active tiles
        activeTiles = new List<GameObject>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //spawn tiles upto amount specified in var amnTilesOnScreen
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            //for the first 2 tiles just create starter tiles
            if (i < 2)
                SpawnTile(0);   // 0 is the index assigned in unity to starter tile
            else
                SpawnTile();    //now create the random ones
        }


    }

    // Update is called once per frame
    void Update()
    {


        if (playerTransform.position.z - safeToDelete > (spawnZ - amnTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();

        }
    }
    //method to spawn a tile 
    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject Obj;

        //create the tile on screen

        if (prefabIndex == -1)
            Obj = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        else
            Obj = Instantiate(tilePrefabs[prefabIndex]) as GameObject;

        //tag it as a child to 'TileManager' prefab
        Obj.transform.SetParent(transform);

		/// <summary>
		//change by arpit
		/// </summary>
		//if bbramp was spawned increase tilelength by another 20;change index here to match

		if (flag == 1)
		{
            Debug.Log(temp);
//			spawnZ += 20;
			Obj.transform.position = Vector3.forward * spawnZ;

            //update the length of the generated tiles
            spawnZ += tileLength40;

		}
		else { 
			Obj.transform.position = Vector3.forward * spawnZ;

            //update the length of the generated tiles
            spawnZ += tileLength;

		}
		      
        //add tile to list of active tiles
        activeTiles.Add(Obj);

    }
    //method to destroy a tile
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    //method to ensure we randomly generate tiles and do not repeat the same tile back to back
    private int RandomPrefabIndex()
    {

        if (tilePrefabs.Length <= 1)    //if the length is 1; which means we have generated 0 or 1 tile then return 0 which corresponds to starter tile
            return 0;

        int randIndex = lastPrefabIndex;

        //if we generate the same index as that of last prefab tile, keep looping untill we generate a different one

        while (randIndex == lastPrefabIndex)
        {
            randIndex = Random.Range(0, tilePrefabs.Length);

        }

        /// <summary>
        //change by arpit
        /// </summary>
        //bbramp checker use index of bbramp assigned to tilemanager in unity
		if (randIndex == 1||randIndex==2)//change index here to match
        {
            flag = 1;
        }
        else
        {
            flag = 0;
        }

        // TODO: remove this flag if length 40 prefabs are used
        flag = 0; 
            
        //update the last index and return its index to generate the corresponding tile
        lastPrefabIndex = randIndex;
		temp = randIndex;


        return randIndex;
    }
}