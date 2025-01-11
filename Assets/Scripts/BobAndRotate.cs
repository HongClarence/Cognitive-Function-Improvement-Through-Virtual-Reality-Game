using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAndRotate : MonoBehaviour
{
    public float rotationSpeed = 500f;
    public float bobSpeed = 3f;
    public float height = 0.025f;

    private Vector3 pos;

    void Start()
    {
        pos = transform.position;    
    }


    void Update()
    {
        float newY = Mathf.Sin(Time.time * bobSpeed) * height + pos.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }
}
