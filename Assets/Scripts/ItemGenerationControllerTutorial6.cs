using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerationControllerTutorial6 : MonoBehaviour {
    public GameObject coinPrefab;
    
    public GameObject itemMagnetPrefab;
    
    
    private PlayerController playerController;

    private float lastGeneratedPlayerZPos = 0; 

    // Use this for initialization
    void Start() {
        playerController = GetComponent<PlayerController>(); 

        StartCoroutine(GenerateContinuously());
    }
    
    void LateUpdate()
    {
        
    }

    void Generate(GameObject prefab)
    {
        GameObject newRewardObj = Instantiate(prefab) as GameObject;

        float spawnZPos = transform.position.z + playerController.GetSpeed() * 4;

        newRewardObj.transform.position = new Vector3(Mathf.Round(Random.Range(-2.0f, 2.0f)), 5, spawnZPos);
    }

    IEnumerator GenerateContinuously()
    {
        while(gameObject != null)
        {
            if (transform.position.z > Mathf.Max(lastGeneratedPlayerZPos + 2.0f, 0))
            {
                float randVal = Random.Range(0, 100);

                if (randVal < 6)
                {
                    Generate(itemMagnetPrefab);
                }

                
                else
                {
                    Generate(coinPrefab);
                }
                
                lastGeneratedPlayerZPos = transform.position.z; 
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

}
