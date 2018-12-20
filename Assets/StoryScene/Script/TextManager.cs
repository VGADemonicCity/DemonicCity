using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity.StoryScene
{
    public enum CharName
    {
        Magia, Phoenix, Nafla, Amon, Ashmedy, Faulus, Barl, Ixmagina, Maou, Unknown, None, System
    }
    public class TextManager : MonoBehaviour
    {
        string[] CHARNAME = {
        "マギア",
        "フィニクス",
        "ナフラ",
        "アーモン",
        "アシュメダイ",
        "フォーラス",
        "バアル",
        "イクスマギア",
        "魔王",
        "？？？",
        ""
        };


        [SerializeField] TextDirector director;
        [SerializeField] FaceManager[] faceManagers = new FaceManager[2];

        bool isStaging = false;
        string buttonTag = "Button";
        bool flag;
        int textIndex = 0;
        public TouchGestureDetector touchGestureDetector;
        List<TextStorage> texts = new List<TextStorage>();
        [SerializeField] PutSentence putSentence;
        [SerializeField] TMPro.TMP_Text nameObj;
        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
        }
        void Start()
        {

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    GameObject hit;
                    touchInfo.HitDetection(out hit);
                    if (hit.tag != buttonTag)
                    {
                        if (isStaging)
                        {
                            Debug.Log("おわりだよー(*∂ｖ∂)");
                            DivideTexts();
                            return;
                        }
                        flag = putSentence.end;
                        if (flag)
                        {
                            if (texts.Count <= textIndex ||
                            texts[textIndex].cName == CharName.System)
                            {
                                isStaging = true;
                                //return;
                            }
                            else
                            {
                                textIndex += 1;
                            }
                            putSentence.end = false;
                        }
                        else
                        {
                            putSentence.FullTexts();
                        }
                        DivideTexts();

                        flag = putSentence.CallSentence(texts[textIndex].sentence);

                    }
                }
            });


            SetText();
            flag = putSentence.A(texts[textIndex].sentence);
        }


        /// <summary>ListのCharNameに応じた名前を出力するが、演出がある場合はその演出を再生する</summary>
        void DivideTexts()
        {
            if (texts[textIndex].cName == CharName.System)
            {
                director.Staging(texts[textIndex]);
            }

            if (texts[textIndex].isUnknown)
            {
                nameObj.text = CHARNAME[(int)CharName.Unknown];
            }
            else
            {
                nameObj.text = CHARNAME[(int)texts[textIndex].cName];
            }
        }

        void Staging(TextStorage storage)
        {
            SceneName outName;
            if (EnumCommon.TryParse<SceneName>(storage.sentence, out outName))
            {
                SceneChanger.SceneChange(outName);
            }
            else
            {
                textIndex += 1;
                isStaging = false;
            }

        }

        void Update()
        {
        }


        string filePath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Texts.json";
        public void SetText()
        {
            if (null == File.ReadAllText(filePath))
            {
                return;
            }
            string textsJson = File.ReadAllText(filePath);
            Debug.Log(textsJson);
            string[] spritKey = { "><" };

            string[] tmpTexts = textsJson.Split(spritKey, StringSplitOptions.None);
            //Debug.Log(tmpTexts);
            foreach (string s in tmpTexts)
            {

                Debug.Log(s);
                var sss = JsonUtility.FromJson<TextStorage>(s);
                Debug.Log(sss);
                if (sss.face == null || sss.face == "")
                {
                    sss.face = FaceIndex.Last.ToString();
                }
                var tmpStorage = new TextStorage(JsonUtility.FromJson<TextStorage>(s));
                Debug.Log(tmpStorage);
                texts.Add(tmpStorage);


            }
            /*for (int i = 0; i < 10; i++)
            {
                texts.Add(new TextStorage(i.ToString() + "123abc456def789ghi"));
                //Debug.Log(i.ToString() + "abcdEfgkfdajitoevaejko");
            }*/

        }

    }




    //touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
    //{

    //});
}
