using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity.StoryScene
{
    //public enum CharName
    //{
    //    Magia, Phoenix, Nafla, Amon, Ashmedy, Faulus, Barl, Ixmagina, Maou,  Unknown, None, System
    //}
    public class TalkManager : MonoBehaviour
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
        bool isStaging = false;
        string buttonTag = "Button";
        bool flag;
        int textIndex = 0;
        public TouchGestureDetector touchGestureDetector;
        List<TextStorage> texts = new List<TextStorage>();
        [SerializeField] PutSentence[] putSentence = new PutSentence[2];
        [SerializeField] TMPro.TMP_Text[] nameObj = new TMPro.TMP_Text[2];
        CharName talker;
        int talkPosition = 0;
        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
        }
        void Start()
        {
            //touchGestureDetector = GameObject.Find("DemonicCity.TouchGestureDetector").GetComponent<TouchGestureDetector>();
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
                        flag = putSentence[talkPosition].end;
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
                            putSentence[talkPosition].end = false;
                        }
                        else
                        {
                            putSentence[talkPosition].FullTexts();
                        }
                        DivideTexts();
                        if (talkPosition < putSentence.Length)
                        {
                            flag = putSentence[talkPosition].CallSentence(texts[textIndex].sentence);
                        }
                    }
                }









                //    if (gesture == TouchGestureDetector.Gesture.Click)
                //    {
                //        if (isStaging)
                //        {
                //            Debug.Log("おわりだよー(*∂ｖ∂)");
                //            DivideTexts();
                //            return;
                //        }
                //        //Debug.Log("begin");
                //        GameObject hit;
                //        touchInfo.HitDetection(out hit);
                //        if (hit.tag != buttonTag)
                //        {
                //            //DivideTexts();
                //            flag = putSentence[talkPosition].onoff;
                //            if (flag)
                //            {
                //                if (texts.Count <= textIndex ||
                //                texts[textIndex].cName == CharName.System)
                //                {
                //                    isStaging = true;
                //                    //return;
                //                }
                //                else
                //                {
                //                    textIndex += 1;
                //                }
                //            }
                //            else
                //            {
                //                putSentence[talkPosition].Totrue();
                //            }
                //            DivideTexts();
                //            if (talkPosition<putSentence.Length)
                //            {
                //                flag = putSentence[talkPosition].A(texts[textIndex].sentence);
                //            }


                //        }
                //    }
            });

            TextReset();
            SetText();
            DivideTexts();
            flag = putSentence[talkPosition].CallSentence(texts[textIndex].sentence);
        }

        void Update()
        {
            //flag = putSentence.A(texts[textIndex].sentence);
            if (flag)
            {

            }
        }


        void DivideTexts()
        {

            //Debug.Log(texts[textIndex].charName + ":" + CharName.Magia.ToString());
            talker = texts[textIndex].cName;
            if (talker == CharName.System)
            {
                Staging(texts[textIndex]);
                talkPosition = 3;
            }
            else if (talker == CharName.Magia
                || talker == CharName.Maou)
            {
                talkPosition = 0;
            }
            else
            {
                talkPosition = 1;
            }

            if (talkPosition < nameObj.Length)
            {
                if (texts[textIndex].isUnknown)
                {
                    nameObj[talkPosition].text = CHARNAME[(int)CharName.Unknown];
                }
                else
                {
                    nameObj[talkPosition].text = CHARNAME[(int)talker];
                }
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
                isStaging = false;
                textIndex += 1;
                isStaging = false;
            }

        }


        void TextReset()
        {
            foreach (var obj in nameObj)
            {
                obj.text = "";
            }
        }





        string filePath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Texts.json";
        string folderPath = "";
        string fileName = "";
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

                //Debug.Log(s);
                var sss = JsonUtility.FromJson<TextStorage>(s);
                //Debug.Log(sss);
                //if (sss.face == null || sss.face == "")
                //{
                //    sss.face = TextStorage.FaceIndex.Last.ToString();
                //}
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
