using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

	//author: Arpit; Changes: Adding infinite tiles in the game

	//list holding prefabs
	public GameObject[] tilePrefabs;

	//to track the player
	private Transform playerTransform;

	//spawn of tile updation variable
	private float spawnZ = 0.0f;

	//length of each tile in the prefab; each of the prefab in folder lanes is of length 20 units
	private float tileLength = 20.0f;

	//how many tiles we want to see at any time in the background at one time
	private int amnTilesOnScreen = 10;

	// Use this for initialization
	void Start () {

		playerTransform=GameObject.FindGameObjectWithTag("Player").transform;
		//spawn tiles upto amount specified in var amnTilesOnScreen
		for (int i=0; i < amnTilesOnScreen; i++) {
			SpawnTile ();
		
		}


	}
	
	// Update is called once per frame
	void Update () {


		if (playerTransform.position.z > (spawnZ - amnTilesOnScreen * tileLength)) {
			SpawnTile ();
		}
	}
	//method to spawn a tile 
	private void SpawnTile(int prefabIndex=-1)
	{
		GameObject Obj;

		//create the tile on screen
		Obj = Instantiate (tilePrefabs [0]) as GameObject;

		//tag it as a child to 'TileManager' prefab
		Obj.transform.SetParent (transform);
		Obj.transform.position = Vector3.forward*spawnZ;

		//update the length of the generated tiles
		spawnZ += tileLength;

		
	}
}
