using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerationController : MonoBehaviour {
    public GameObject coinPrefab;
    public GameObject itemHealthBoxPrefab;
    public GameObject itemMagnetPrefab;
    public GameObject itemShieldPrefab;
    public GameObject itemWingPrefab;

    private PlayerController playerController; 

    // Use this for initialization
    void Start() {
        playerController = GetComponent<PlayerController>(); 

        StartCoroutine(GenerateContinuously());
    }
    
    void LateUpdate()
    {
        
    }

    void GenerateReward()
    {
        GameObject newRewardObj = Instantiate(coinPrefab) as GameObject;

        float spawnZPos = transform.position.z + playerController.GetSpeed() * 2;

        newRewardObj.transform.position = new Vector3(Mathf.Round(Random.Range(-2.0f, 2.0f)), 5, spawnZPos);
    }

    IEnumerator GenerateContinuously()
    {
        while(gameObject != null)
        {
            GenerateReward();

            yield return new WaitForSeconds(0.2f);
        }
    }

}
