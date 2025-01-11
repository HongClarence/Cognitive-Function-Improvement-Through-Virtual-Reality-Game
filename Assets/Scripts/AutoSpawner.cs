using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawner : MonoBehaviour
{
    public GameObject prefab;
    private Transform spawnPoint;

    void Start()
    {
        spawnPoint = gameObject.transform;
    }

    void Update()
    {
        if(Time.frameCount % 30 == 0)
            if (!Physics.CheckSphere(spawnPoint.position, 0.05f))
                Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
