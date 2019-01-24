using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTest : MonoBehaviour
{

    [SerializeField] GameObject[] testObject;
    [SerializeField] Transform parent;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            parent.gameObject.SetActive(true);
            Instantiate(testObject[0], parent);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            parent.gameObject.SetActive(true);
            Instantiate(testObject[1], parent);
        }
    }
}
