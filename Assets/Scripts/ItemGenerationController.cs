using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ItemGenerationController : NetworkBehaviour {
    public GameObject coinPrefab;
    public GameObject itemHealthBoxPrefab;
    public GameObject itemMagnetPrefab;
    public GameObject itemShieldPrefab;
    public GameObject itemWingPrefab;
    
    //private PlayerController playerController;

    private float lastGeneratedPlayerZPos = 0; 

    // Use this for initialization
    void Start() {
        //playerController = GetComponent<PlayerController>(); 
        if(isServer)
            StartCoroutine(GenerateContinuously());
    }
    
    void LateUpdate()
    {
        
    }

    void Generate(GameObject prefab)
    {
        if(isServer){
            GameObject newRewardObj = Instantiate(prefab) as GameObject;

            float spawnZPos = GameController.currentMaxPosition + 10 * 4;
            newRewardObj.transform.position = new Vector3(Mathf.Round(Random.Range(-2.0f, 2.0f)), 5, spawnZPos);
            NetworkServer.Spawn(newRewardObj);
            
        }
    }

    IEnumerator GenerateContinuously()
    {
        while(gameObject != null)
        {
            if (GameController.currentMaxPosition > Mathf.Max(lastGeneratedPlayerZPos + 2.0f, 0))
            {
                float randVal = Random.Range(0, 100);

                if (randVal < 3)
                {
                    Generate(itemWingPrefab);
                }

                else if (randVal < 6)
                {
                    Generate(itemMagnetPrefab);
                }

                else if (randVal < 9)
                {
                    Generate(itemShieldPrefab); 
                }

                else if (randVal < 12)
                {
                    Generate(itemHealthBoxPrefab); 
                }

                else
                {
                    Generate(coinPrefab);
                }
                
                lastGeneratedPlayerZPos = GameController.currentMaxPosition; 
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

}
