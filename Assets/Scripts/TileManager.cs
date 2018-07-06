using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TileManager : NetworkBehaviour
{
    float positionForRespawn = 0;
    
    //list holding prefabs
    public GameObject[] tilePrefabs;

    //to track the player
    private Transform playerTransform;

    //spawn of tile updation variable
    private float spawnZ = 0;

    //length of each tile in the prefab; each of the prefab in folder lanes is of length 20 units
    private float tileLength = 20.0f;

    //how many tiles we want to see at any time in the background at one time
    private int amnTilesOnScreen = 10;

    //list of active game tiles
    private List<GameObject> activeTiles;

    //list of probabilities for each level of prefabs
    private List<float> probabilities;

    //list of spawned prefab indices;
    private SyncListInt prefabIndices = new SyncListInt();

    //current level
    private int level = 0;

    //safe to destroy tiles after these units so player does not fall into empty space
    private float safeToDelete = 25.0f;

    //Number of prefabs spawned by cuurent player.
    private int numberOfPrefabsSpawnedByCurrentPlayer = 0;

    private bool spawned = false;

    private int locFlag = 0;

    public int maxLevel = 9;

    // Use this for initialization
    void Start()
    {
        //instantiate active tiles
        activeTiles = new List<GameObject>();

        //instantiate probabilities and add the initial probability
        probabilities = new List<float>();
        probabilities.Add(100.0f);


        if(isServer)
        {
            Debug.Log("This is the server!");
            generateInitialTiles();
        }

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

    public void Spawn(){

    }

    // Update is called once per frame
    void Update()
    {
        
        if(GameObject.Find("Player(Clone)")!= null && !spawned){
            spawned = true;
            playerTransform = GameObject.FindWithTag("Player").transform;
            Debug.Log("Attached tile manager controller to player");
            //Debug.Log("Scene: " +GameObject.FindWithTag("Player").scene.name); 

        }
        if(spawned){
            if (playerTransform.position.z - safeToDelete > (spawnZ - amnTilesOnScreen * tileLength))
            {
                SpawnTile();
                DeleteTile();
            }    
        }
        
    }

    private void generateInitialTiles(){

    }

    // Spawn a tile 
    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject Obj;

        int temp = (int)spawnZ;

        //Level Up every 6 tiles
        if (temp > 0 && ((temp) % 120 == 0) && level < maxLevel)
        {
            level += 1;
            float lastprob = probabilities[probabilities.Count - 1];
            probabilities[probabilities.Count - 1] = lastprob / 2.0f;
            probabilities.Add(lastprob / 2.0f);
        }

        //create the tile on screen
        if (prefabIndex == -1)
        {
            Obj = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        }
        else
        {
            Obj = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
            CmdAddNextPrefabIndex(prefabIndex);
        }

        //tag it as a child to 'TileManager' prefab
        Obj.transform.SetParent(transform);
        Obj.transform.position = Vector3.forward * spawnZ;

        //update the length of the generated tiles
        spawnZ += tileLength;

        if (locFlag == 1)
        {
            locFlag = 0;
            setSpawnPos(Obj);
        }

        //add tile to list of active tiles
        activeTiles.Add(Obj);
        numberOfPrefabsSpawnedByCurrentPlayer++;
        Debug.Log("numberOfPrefabsSpawnedByCurrentPlayer is: " + numberOfPrefabsSpawnedByCurrentPlayer);
    }

     float getSpawnPos()
    {
        return (positionForRespawn + 21);
    }

    public void setSpawnPos(GameObject Obj)
    {
        positionForRespawn = Obj.transform.position.z;
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
        //if the length is 1; which means we have generated 0 or 1 tile then return 0 which corresponds to starter tile

        if(numberOfPrefabsSpawnedByCurrentPlayer < prefabIndices.Count){
            Debug.Log("Tile index already generated, returning");
            return prefabIndices[numberOfPrefabsSpawnedByCurrentPlayer];
        }

        
        Debug.Log("Generating New Tile Index");


        if (tilePrefabs.Length <= 1)
        {
            return 0;
        }

        int randIndex = numberOfPrefabsSpawnedByCurrentPlayer;

        int count = 0, levelChoose = level;
        float randomNumber, prevProbabilty = 0f;

        //Generating random number to select the level of prefab
        randomNumber = Random.Range(0.0f, 100.0f);

        //Select corresponding level
        foreach (float probability in probabilities)
        {
            randomNumber -= prevProbabilty;
            if (randomNumber > probability)
            {
                levelChoose--;
                prevProbabilty = probability;
            }
            else
            {
                break;
            }
        }

        List<GameObject> currentLevelPrefabs;
        currentLevelPrefabs = new List<GameObject>();

        foreach (GameObject gobj in tilePrefabs)
        {
            if (gobj.CompareTag(levelChoose.ToString()))
            {
                currentLevelPrefabs.Add(gobj);
                count++;
            }
        }

        int i;

        if(prefabIndices.Count > 0){
            Debug.Log("prefabIndices.Count is: " + prefabIndices.Count + "numberOfPrefabsSpawnedByCurrentPlayer-1 is : " + numberOfPrefabsSpawnedByCurrentPlayer);
            while (randIndex == prefabIndices[numberOfPrefabsSpawnedByCurrentPlayer-1])
            {
                randIndex = Random.Range(0, currentLevelPrefabs.Count);
                for (i = 0; i < tilePrefabs.Length; i++)
                {
                    if (tilePrefabs[i] == currentLevelPrefabs[randIndex])
                    {
                        randIndex = i;
                        break;
                    }
                }
                if (currentLevelPrefabs.Count < 2)
                    break;
            }
        }
        else{
             randIndex = Random.Range(0, currentLevelPrefabs.Count);
                for (i = 0; i < tilePrefabs.Length; i++)
                {
                    if (tilePrefabs[i] == currentLevelPrefabs[randIndex])
                    {
                        randIndex = i;
                        break;
                    }
                }
        }

        

        //update the last index and return its index to generate the corresponding tile
        //numberOfPrefabsSpawnedByCurrentPlayer = randIndex;
        Debug.Log("Calling Command.");
        CmdAddNextPrefabIndex(randIndex);

        return randIndex;
    }

    [Command]
    void CmdAddNextPrefabIndex(int nextTileIndex){
        prefabIndices.Add(nextTileIndex);
        Debug.Log("Added to list, List Size: " + prefabIndices.Count);
    }
}