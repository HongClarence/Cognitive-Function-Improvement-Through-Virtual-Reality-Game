using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomObject : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private GameObject objectSpawn;
    private Color objectColor;
    private Renderer objectRenderer;
    public void Start()
    {
        switch(Random.Range(1,4))
        {
            case 1:
                objectSpawn = prefab1;
                break;

            case 2:
                objectSpawn = prefab2;
                break;

            case 3:
                objectSpawn = prefab3;
                break;
        }

        switch(Random.Range(1,4))
        {
            case 1:
                objectColor = Color.red;
                break;

            case 2:
                objectColor = Color.green;
                break;

            case 3:
                objectColor = Color.yellow;
                break;
        }

        objectSpawn = Instantiate(objectSpawn, spawnPoint.position, spawnPoint.rotation);
        objectRenderer = objectSpawn.GetComponent<Renderer>();
        objectRenderer.material.color = objectColor;
    }
}
