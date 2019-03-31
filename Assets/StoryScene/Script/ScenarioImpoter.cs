#if UNITY_EDITOR



using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DemonicCity.StoryScene;


namespace DemonicCity
{
    public class ScenarioImpoter
    {

        [MenuItem("Assets/Scenario/JsonToAsset")]
        public static void JsonToAssets()
        {

            Object obj = Selection.activeObject;


            string objPath = AssetDatabase.GetAssetPath(obj).Split('.')[0];
            TextAsset text = (TextAsset)Selection.activeObject;

            Debug.Log(objPath);
            Debug.Log(text.text);


            string textsJson = text.text;
            string[] spritKey = { "><" };

            string[] tmpTexts = textsJson.Split(spritKey, System.StringSplitOptions.None);

            Scenario tmpScenario = new Scenario();

            foreach (string s in tmpTexts)
            {
                var tmpStorage = new TextStorage(JsonUtility.FromJson<TextStorage>(s));
                //Debug.Log(tmpStorage);



                tmpScenario.texts.Add(tmpStorage);
                tmpScenario.characters.Add(tmpStorage.cName);
            }
            tmpScenario.characters = tmpScenario.characters.Distinct().Where(item => item <= CharName.Ixmagina).ToList();



            AssetDatabase.CreateAsset(tmpScenario, objPath + ".asset");
        }




    }
}


#endif