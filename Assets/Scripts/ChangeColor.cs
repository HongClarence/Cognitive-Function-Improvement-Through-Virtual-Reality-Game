using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private Renderer objectRenderer;
    public void SetNextColor()
    {
        objectRenderer = GetComponent<Renderer>();
        Color objectColor = objectRenderer.material.color;
        if(objectColor == Color.red)
            objectRenderer.material.color = Color.green;
        else if (objectColor == Color.green)
            objectRenderer.material.color = Color.yellow;
        else
            objectRenderer.material.color = Color.red;
    }
}
