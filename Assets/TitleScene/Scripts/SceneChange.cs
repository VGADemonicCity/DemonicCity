using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DemonicCity.HomeScene
{
    enum SceneName
    {
        Home, Growth, Story, Battle, Last
    }
    public static class SceneChange
    {
        static SceneName scenes;
        public static void SceneChanger(string s)
        {
            SceneManager.LoadScene(s);
        }
        
    }

}