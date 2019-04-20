using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[S]キーで章選択");
        Debug.Log("[W]キーで次の章へ");
    }
    [SerializeField] GameObject[] testObject;
    GameObject[] testInstance = new GameObject[2];
    [SerializeField] Transform parent;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StoryTestDebugerOpen();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            parent.gameObject.SetActive(true);
            if (testInstance[1] == null)
            {
                testInstance[1] = Instantiate(testObject[1], parent);
            }
            else
            {
                testInstance[1].SetActive(!testInstance[1].activeSelf);
            }
        }
    }


    public void StoryTestDebugerOpen()
    {
        parent.gameObject.SetActive(true);
        if (testInstance[0] == null)
        {
            testInstance[0] = Instantiate(testObject[0], parent);
        }
        else
        {
            testInstance[0].SetActive(!testInstance[0].activeSelf);
        }
    }
}
