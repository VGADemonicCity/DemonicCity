using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity.StoryScene
{
    public enum CharName
    {
        Magia, Phoenix, Nafla, Amon, Ashmedy, Faulus, Barl, Maou, Ixmagina, Unknown, None, System
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
        [SerializeField] Transform parentTransform;
        [SerializeField] List<GameObject> characterObjects = new List<GameObject>();
        List<FaceChanger> faceChangers = new List<FaceChanger>();
        List<int> characters = new List<int>();


        public bool isStaging = false;
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
                    if (hit.tag != buttonTag
                        && !isStaging)
                    {
                        TextsDraw();
                    }
                }
            });


            SetText();
            DivideTexts();
            flag = putSentence.A(texts[textIndex].sentence);
        }

        void TextsDraw()
        {
            if (putSentence.end)
            {
                textIndex += 1;
                if (DivideTexts())
                {
                    flag = putSentence.CallSentence(texts[textIndex].sentence);

                }
                else
                {
                    director.Staging(texts[textIndex]);

                }
            }
            else
            {
                putSentence.FullTexts();
            }

        }






        /// <summary>ListのCharNameに応じた名前を出力するが、演出がある場合はその演出を再生する</summary>
        bool DivideTexts()
        {
            Debug.Log(texts[textIndex].cName);
            if (texts[textIndex].cName == CharName.System)
            {
                return false;
            }
            else
            {
                Debug.Log(texts[textIndex].faceIndex);
                faceChangers[0].ChangeFace(texts[textIndex].faceIndex);
            }

            if (texts[textIndex].isUnknown)
            {
                nameObj.text = CHARNAME[(int)CharName.Unknown];
            }
            else if (texts[textIndex].cName == CharName.None)
            {
                nameObj.text = "";
            }
            else
            {
                nameObj.text = CHARNAME[(int)texts[textIndex].cName];
            }
            return true;
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

        void SetCharacter(List<int> names)
        {
            foreach (var item in names)
            {
                GameObject charObj = Instantiate(characterObjects[item], parentTransform);
                director.characters.Add(charObj);
                faceChangers.Add(charObj.GetComponent<FaceChanger>());
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

                //Debug.Log(s);
                var sss = JsonUtility.FromJson<TextStorage>(s);
                //Debug.Log(sss);   
                if (sss.face == null || sss.face == "")
                {
                    sss.face = FaceIndex.Last.ToString();
                }
                var tmpStorage = new TextStorage(JsonUtility.FromJson<TextStorage>(s));
                characters.Add((int)tmpStorage.cName);
                //Debug.Log(tmpStorage);
                texts.Add(tmpStorage);

            }
            characters = characters.Distinct().Where(item => item <= (int)CharName.Ixmagina).ToList();
            SetCharacter(characters);
        }

    }




}
