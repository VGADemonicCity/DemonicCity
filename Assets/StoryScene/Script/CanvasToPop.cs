using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasToPop : MonoBehaviour
{

    void Awake()
    {
        if (!GetComponent<Canvas>().worldCamera)
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}
