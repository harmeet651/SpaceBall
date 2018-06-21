using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {
    // Destroyer
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}