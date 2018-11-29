using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DemonicCity
{
    public enum SceneName
    {
        Home,  Story, Battle, Last
    }
    public static class SceneChanger
    {
        static SceneName scenes;
        public static void SceneChange(string s)
        {
            SceneManager.LoadScene(s);
        }
        public static void SceneChange(SceneName name)
        {
            SceneManager.LoadScene(name.ToString());
        }


    }

}