using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresencePhysics : MonoBehaviour
{
    public Transform controller;
    public Renderer nonPhysicalHand;
    public float showNonPhysicalHandDistance = 0.05f;

    private Rigidbody rigidBody;
    private Collider[] handColliders;

    public void EnableCollider()
    {
        foreach (var item in handColliders)
            item.enabled = true;
    }

    public void EnableColliderDelay(float delay)
    {
        Invoke("EnableCollider", delay);
    }

    public void DisableCollider()
    {
        foreach (var item in handColliders)
            item.enabled = false;
    }
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, controller.position);

        if (distance > showNonPhysicalHandDistance)
            nonPhysicalHand.enabled = true;
        else
            nonPhysicalHand.enabled = false;
    }

    void FixedUpdate()
    {
        rigidBody.velocity = (controller.position - transform.position) / Time.fixedDeltaTime;
        Quaternion rotationDiff = controller.rotation * Quaternion.Inverse(transform.rotation);
        rotationDiff.ToAngleAxis(out float angleDegree, out Vector3 rotationAxis);
        Vector3 rotationDiffDegree = angleDegree * rotationAxis;
        if(rotationDiffDegree.x == Mathf.Infinity || rotationDiffDegree.x == Mathf.NegativeInfinity)
            rigidBody.angularVelocity = new Vector3(0, 0, 0);
        else
            rigidBody.angularVelocity = (rotationDiffDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }


}
