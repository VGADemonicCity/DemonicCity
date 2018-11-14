using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;


namespace DemonicCity.HomeScene
{
    
    public static class EnumStock
    {
         

        




    }



    //Scene[] scenes = new Scene[(int)SceneName.Last];
    //string nameA = "Assets/Scenes/";
    //string nameB = ""
    //string nameC = ".unity";

    //void SceneLoad()
    //{
    //    for (int i = 0; i < (int)SceneName.Last; i++)
    //    {
    //        nameB = ((SceneName)Enum.ToObject(typeof(SceneName), i)).ToString()
    //        scenes[i] = AssetDatabase.LoadAssetAtPath<Scene>(scenePath++unity);

    //    }

    //}




    //public static class Scenes
    //{
    //    // シーンのリストをenumで作る
    //    public enum MyScene { Battle, Home, Title, Growth, Story, Last }
    //    public static MyScene scene;

    //    // シーン名とenumのシーンとを対応させる
    //    static Dictionary<string, MyScene> sceneDic = new Dictionary<string, MyScene>() {
    //    {"Battle", MyScene.Battle },
    //    {"Home",     MyScene.Home },
    //    {"Title",   MyScene.Title },
    //    {"Growth",   MyScene.Growth },
    //    {"Story",   MyScene.Story },
    //};

    //    // 現在のシーンを取得する
    //    public static MyScene MyGetScene()
    //    {
    //        string sceneName = SceneManager.GetActiveScene().name;
    //        scene = sceneDic[sceneName];
    //        return scene;
    //    }

    //    // enumのシーンで指定したシーンをロードする
    //    public static void MyLoadScene(MyScene scene)
    //    {
    //        SceneManager.LoadScene(sceneDic.FirstOrDefault(x => x.Value == scene).Key);
    //    }

    //}
}