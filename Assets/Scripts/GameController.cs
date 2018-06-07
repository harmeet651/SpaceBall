using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GameController controls the overall aspect of the game
// For example, generating tiles, obstacles should be handled here
// Game over, restart actions should also be defined here
public class GameController : MonoBehaviour {

	public KeyCode moveL;
	public KeyCode moveR;
	public KeyCode moveSlow;

    private GameObject player;
    private PlayerController playerController;

    public int numLanes = 5;                   // number of lanes
    public int score = 0;

    // Prefab objects
    public GameObject centerLanePrefab; 
    public GameObject outerLanePrefab; 
    public GameObject inBetweenLanePrefab; 
    public GameObject obstacleCylinderPrefab;
    public GameObject rewardCubePrefab;
    public GameObject rampPrefab; 

    // Canvas
    public GameObject gameCanvas;
    public GameObject gameOverCanvas;
    public Text scoreText;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        int zPos = -20; 

        PlaceFullFloor(zPos, 0);
        zPos += 10; 
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceUpAndDownRamp(3, zPos, 0, 1); 
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceBackAndForthRamp(2, zPos, 1, 5);
        zPos += 15;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 2, 3, 4 }, zPos, 1); 
        zPos += 10;
        PlaceLeftAndRightRamp(3, zPos, 1, 2);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 5 }, zPos, 1);
        zPos += 10; 

        for (int z = 10; z < 40; z += 10) {
            int lane = Mathf.RoundToInt(Random.Range(1.0f, 5.0f));

            PlaceRewardCube(lane, z, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(moveL))
		{
			playerController.MoveLeft();
		}
		if (Input.GetKeyDown(moveR))
		{
			playerController.MoveRight();
		}
		if (Input.GetKey(moveSlow))
		{
			playerController.MoveSlow();
		}
	}

    /*
     * Return the center position of a lane
     */
    public float GetLaneCenterXPos(int laneNum) {
        return (float)(laneNum - (numLanes / 2 + 1));
    }

    /*
     * Change score by passed amount
     */
    public void AddScore(int amount)
    {
        score += amount;

        // Update score display in screen
        scoreText.text = "Score " + score.ToString();
    }

    /*
     * Instantiate a prefab into a GameObject
     */
    GameObject InstantiatePrefab(GameObject prefab) {
        GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
      
        return instance; 
    }

    /*
     * Place a full set of floor blocks at given z position, and altitude 
     */
    private void PlaceFullFloor(int beginZ, int altitude) {
        for (int lane = 1; lane <= 5; lane++)
        {
            PlaceFloorByLane(lane, beginZ, altitude);
        }
    }

    /*
     * Place set of floor blocks at given lanes, z position, and altitude
     */
    private void PlaceFloorByLanes(int[] lanes, int beginZ, int altitude)
    {
        foreach (int lane in lanes)
        {
            PlaceFloorByLane(lane, beginZ, altitude);
        }
    }

    /*
     * Place a specific floor lane block at a given lane, z position, and altitude
     */
    private void PlaceFloorByLane(int lane, int beginZ, int altitude)
    {
        GameObject floorPrefab;

        if (lane == 1 || lane == 5) floorPrefab = outerLanePrefab;
        else if (lane == 2 || lane == 4) floorPrefab = inBetweenLanePrefab;
        else floorPrefab = centerLanePrefab;

        GameObject floorInstance = InstantiatePrefab(floorPrefab);
        floorInstance.transform.Translate(new Vector3(lane - 3, altitude - 0.5f, beginZ + 5.0f));
    }

    /*
     * Place a reward cube at a given lane, z position, and altitude
     */
    private void PlaceRewardCube(int lane, int beginZ, int altitude) {
        GameObject rewardCube = InstantiatePrefab(rewardCubePrefab);

        rewardCube.transform.Translate(new Vector3(lane - (int)(numLanes / 2 + 1), .2f + altitude, beginZ));
    }
    
    /*
     * Place a cylinder obstacle at a given lane, z position, and altitude
     */
    private void PlaceObstacleCylinder(int lane, int beginZ, int altitude) {
        GameObject cylinder = InstantiatePrefab(obstacleCylinderPrefab);

        cylinder.transform.Translate(new Vector3(lane - (int)(numLanes / 2 + 1), altitude, beginZ));
    }

    private void PlaceUpAndDownRamp(int lane, int beginZ, int bottomAltitude, int moveAmount)
    {
        GameObject ramp = InstantiatePrefab(rampPrefab);
        
        ramp.transform.Translate(new Vector3(lane - 3, bottomAltitude, beginZ + 5.0f));
        
        RampController rampController = ramp.GetComponent<RampController>();
        rampController.isMovingUpAndDown = true;
        rampController.moveAmount = moveAmount; 
    }

    private void PlaceBackAndForthRamp(int lane, int beginZ, int altitude, int moveAmount)
    {
        GameObject ramp = InstantiatePrefab(rampPrefab);
        
        ramp.transform.Translate(new Vector3(lane - 3, altitude, beginZ + 5.0f));

        RampController rampController = ramp.GetComponent<RampController>();
        rampController.isMovingBackAndForth = true;
        rampController.moveAmount = moveAmount;
    }

    private void PlaceLeftAndRightRamp(int beginLane, int beginZ, int altitude, int moveAmount)
    {
        GameObject ramp = InstantiatePrefab(rampPrefab);
        
        ramp.transform.Translate(new Vector3(beginLane - 3, altitude, beginZ + 5.0f));

        RampController rampController = ramp.GetComponent<RampController>();
        rampController.isMovingLeftAndRight = true;
        rampController.moveAmount = moveAmount;
    }

    public void GameOver() {
        gameOverCanvas.SetActive(true); 
    }

}
