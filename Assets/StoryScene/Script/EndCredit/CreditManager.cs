using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TmpAccele()
    {
        Time.timeScale = 10;
        bool isAccele = true;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += ((scene, mode) =>
        {
            if (isAccele)
            {
                Time.timeScale = 1f;
                isAccele = false;
            }
        });
    }
}
