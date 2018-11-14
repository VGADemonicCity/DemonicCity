using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DemonicCity.HomeScene
{
    public enum SceneName
    {
        Home,  Story, Battle, Last
    }
    public static class SceneChange
    {
        static SceneName scenes;
        public static void SceneChanger(string s)
        {
            SceneManager.LoadScene(s);
        }
        public static void SceneChanger(SceneName name)
        {
            SceneManager.LoadScene(name.ToString());
        }


    }

}