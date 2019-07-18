using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTitle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(DemonicCity.SceneFader.SceneTitle.Title.ToString());
    }
}
