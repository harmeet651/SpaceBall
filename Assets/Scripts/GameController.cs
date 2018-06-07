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
    private int centerLane; 
    public int score = 0;

    // Prefab objects
    public GameObject centerLanePrefab; 
    public GameObject outerLanePrefab; 
    public GameObject inBetweenLanePrefab; 
    public GameObject obstacleCylinderPrefab;
    public GameObject rewardCubePrefab;
    public GameObject rampPrefab; 
    public GameObject greatAxePrefab;
    public GameObject bladeTrapLeftPrefab;
    public GameObject bladeTrapRightPrefab;
    public GameObject axeTrapPrefab;
    public GameObject trapNeedlePrefab;

    // Explosion object
    public Transform explodeObj; 

    // Canvas
    public GameObject gameCanvas;
    public GameObject gameOverCanvas;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        centerLane = numLanes / 2 + 1;

        int zPos = -20;
        
        PlaceFullFloor(zPos, 0);
        zPos += 10; 
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        PlaceAxeTrap(1, zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        PlaceAxeTrap(5, zPos, 0);
        zPos += 10;
        PlaceLeftAndRightRamp(2, zPos, 0, 2); 
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceObstacleGreatAxe(zPos, 0); 
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFullFloor(zPos, 0);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 2, 3, 4 }, zPos, 0);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 3, 4 }, zPos, 0);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 4 }, zPos, 0);
        zPos += 10;
        PlaceUpAndDownRamp(4, zPos, 0, 1);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 4 }, zPos, 1);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 3, 4 }, zPos, 1);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 2, 3, 4 }, zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        PlaceObstacleBladeTrapLeft(zPos, 1);
        PlaceObstacleBladeTrapRight(zPos, 1); 
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 2, 3, 4 }, zPos, 1); 
        zPos += 10;
        PlaceLeftAndRightRamp(3, zPos, 1, 2);
        zPos += 10;
        PlaceFloorByLanes(new int[] { 5 }, zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceAxeTrap(1, zPos, 1);
        PlaceAxeTrap(5, zPos, 1);
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceAxeTrap(3, zPos, 1);
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceAxeTrap(1, zPos, 1);
        PlaceAxeTrap(5, zPos, 1);
        PlaceFullFloor(zPos, 1);
        zPos += 10;
        PlaceFullFloor(zPos, 1);
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
        return (float)(laneNum - centerLane);
    }

    /*
     * Instantiate a prefab into a GameObject
     */
    GameObject InstantiatePrefab(GameObject prefab) {
        GameObject instance = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation) as GameObject;
      
        return instance; 
    }

    /*
     * Place a full set of floor blocks
     */
    private void PlaceFullFloor(int beginZ, int altitude) {
        for (int lane = 1; lane <= 5; lane++)
        {
            PlaceFloorByLane(lane, beginZ, altitude);
        }
    }

    /*
     * Place set of floor blocks
     */
    private void PlaceFloorByLanes(int[] lanes, int beginZ, int altitude)
    {
        foreach (int lane in lanes)
        {
            PlaceFloorByLane(lane, beginZ, altitude);
        }
    }

    /*
     * Place a specific floor block
     */
    private void PlaceFloorByLane(int lane, int beginZ, int altitude)
    {
        GameObject floorPrefab;

        if (lane == 1 || lane == 5) floorPrefab = outerLanePrefab;
        else if (lane == 2 || lane == 4) floorPrefab = inBetweenLanePrefab;
        else floorPrefab = centerLanePrefab;

        GameObject floorInstance = InstantiatePrefab(floorPrefab);
        floorInstance.transform.Translate(new Vector3(lane - centerLane, altitude - 0.5f, beginZ));
    }

    /*
     * Place a reward cube
     */
    private void PlaceRewardCube(int lane, int beginZ, int altitude) {
        GameObject rewardCube = InstantiatePrefab(rewardCubePrefab);

        rewardCube.transform.Translate(new Vector3(lane - centerLane, .2f + altitude, beginZ));
    }
    
    /*
     * Place a cylinder obstacle
     */
    private void PlaceObstacleCylinder(int lane, int beginZ, int altitude) {
        GameObject cylinder = InstantiatePrefab(obstacleCylinderPrefab);

        cylinder.transform.Translate(new Vector3(lane - centerLane, altitude, beginZ));
    }

    /*
     * Place a ramp going up and down (y-axis)
     */
    private void PlaceUpAndDownRamp(int lane, int beginZ, int bottomAltitude, int moveAmount)
    {
        GameObject ramp = InstantiatePrefab(rampPrefab);
        
        ramp.transform.Translate(new Vector3(lane - centerLane, bottomAltitude, beginZ));
        
        RampController rampController = ramp.GetComponent<RampController>();
        rampController.isMovingUpAndDown = true;
        rampController.moveAmount = moveAmount; 
    }

    /*
     * Place a ramp going back and forth (z-axis)
     */
    private void PlaceBackAndForthRamp(int lane, int beginZ, int altitude, int moveAmount)
    {
        GameObject ramp = InstantiatePrefab(rampPrefab);
        
        ramp.transform.Translate(new Vector3(lane - centerLane, altitude, beginZ));

        RampController rampController = ramp.GetComponent<RampController>();
        rampController.isMovingBackAndForth = true;
        rampController.moveAmount = moveAmount;
    }

    /*
     * Place a ramp going left and right (x-axis)
     */
    private void PlaceLeftAndRightRamp(int beginLane, int beginZ, int altitude, int moveAmount)
    {
        GameObject ramp = InstantiatePrefab(rampPrefab);
        
        ramp.transform.Translate(new Vector3(beginLane - centerLane, altitude, beginZ));

        RampController rampController = ramp.GetComponent<RampController>();
        rampController.isMovingLeftAndRight = true;
        rampController.moveAmount = moveAmount;
    }
    
    /*
     * Place a "Left Trap Blade" obstacle
     */
    private void PlaceObstacleBladeTrapLeft(int z, int altitude)
    {
        GameObject bladeTrapLeft = InstantiatePrefab(bladeTrapLeftPrefab);

        bladeTrapLeft.transform.Translate(new Vector3(0, altitude, z)); 
    }
    
    /*
     * Place a "Right Trap Blade" obstacle
     */
    private void PlaceObstacleBladeTrapRight(int z, int altitude)
    {
        GameObject bladeTrapRight = InstantiatePrefab(bladeTrapRightPrefab);

        bladeTrapRight.transform.Translate(new Vector3(0, altitude, z));
    }

    /*
     * Place a "Great Axe" obstacle
     */
    private void PlaceObstacleGreatAxe(int z, int altitude)
    {
        GameObject greatAxe = InstantiatePrefab(greatAxePrefab);

        greatAxe.transform.Translate(new Vector3(0, altitude, z));
    }

    /*
     * Place an "Axe Trap" obstacle
     */
    private void PlaceAxeTrap(int lane, int z, int altitude)
    {
        GameObject axeTrap = InstantiatePrefab(axeTrapPrefab);
        
        axeTrap.transform.Translate(new Vector3(lane - centerLane, altitude, z));
    }

    /*
     * Place a "Trap Needle" obstacle
     */
    private void PlaceTrapNeedle(int lane, int z, int altitude)
    {

    }

    /*
     * Game over event handler
     */
    public void GameOver() {
        // Deactivate the Player GameObject
        player.SetActive(false);

        // Display canvas for game over
        gameOverCanvas.SetActive(true);

        // Create explosion effect
        Instantiate(explodeObj, player.transform.position, explodeObj.rotation);
    }

}
