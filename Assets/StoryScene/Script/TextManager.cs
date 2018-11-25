using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity.StoryScene
{
    public class TextManager : MonoBehaviour
    {

        string buttonTag = "Button";
        bool flag;
        int textIndex = 0;
        public TouchGestureDetector touchGestureDetector;
        List<TextStorage> texts = new List<TextStorage>();
        [SerializeField] PutSentence putSentence;
        void Awake()
        {

        }
        void Start()
        {
            TouchGestureDetector t = TouchGestureDetector.Instance;
            touchGestureDetector = GameObject.Find("DemonicCity.TouchGestureDetector").GetComponent<TouchGestureDetector>();
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    Debug.Log("begin");
                    GameObject hit;
                    touchInfo.HitDetection(out hit);
                    if (true)
                    {
                        flag = putSentence.onoff;
                        if (flag)
                        {
                            textIndex += 1;
                        }
                        else
                        {
                            putSentence.Totrue();

                        }

                        flag = putSentence.A(texts[textIndex].sentence);

                    }
                }
            });


            SetText();
            flag = putSentence.A(texts[textIndex].sentence);
        }

        void Update()
        {
            //flag = putSentence.A(texts[textIndex].sentence);
            if (flag)
            {

            }
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

            string[] tmpTexts = textsJson.Split(spritKey,StringSplitOptions.None);
            Debug.Log(tmpTexts);
            foreach (string s in tmpTexts)
            {

                Debug.Log(s);
                var sss = JsonUtility.FromJson<TextStorage>(s);
                Debug.Log(sss);
                if (sss.face == null || sss.face == "")
                {
                    sss.face = TextStorage.FaceIndex.Last.ToString();
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
