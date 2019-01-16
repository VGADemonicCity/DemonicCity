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
        Progress progress;

        public bool isStaging = false;
        string buttonTag = "Button";
        bool flag;
        int textIndex = 0;
        public TouchGestureDetector touchGestureDetector;
        SceneFader sceneFader;
        List<TextStorage> texts = new List<TextStorage>();
        [SerializeField] PutSentence putSentence;
        [SerializeField] TMPro.TMP_Text nameObj;
        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
            progress = Progress.Instance;
            sceneFader = SceneFader.Instance;
        }
        void Start()
        {
            //test用
            //progress.ThisQuestProgress = Progress.QuestProgress.Prologue;
            //progress.ThisStoryProgress = Progress.StoryProgress.Nafla;
            if (progress.ThisQuestProgress != Progress.QuestProgress.Prologue)
            {
                progress.ThisQuestProgress = Progress.QuestProgress.Epilogue;
            }
            //

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


            SetText(progress.ThisQuestProgress.ToString() + ".json");
            TextsDraw();
        }
        /// <summary>
        /// 次の行を再生する。
        /// </summary>
        public void TextsDraw()
        {
            if (putSentence.End)
            {
                if (DivideTexts())
                {
                    flag = putSentence.CallSentence(texts[textIndex].sentence);
                }
                else
                {
                    director.Staging(texts[textIndex]);
                }
                if (textIndex < texts.FindLastIndex(text => text == texts.Last()))
                {
                    textIndex += 1;
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
            else if (texts[textIndex].cName != CharName.None)
            {
                Debug.Log(texts[textIndex].faceIndex);
                faceChangers.Find(x => x.charName == texts[textIndex].cName).ChangeFace(texts[textIndex].faceIndex);
                //faceChangers[0].ChangeFace(texts[textIndex].faceIndex);
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
            SceneFader.SceneTitle outName;
            if (EnumCommon.TryParse(storage.sentence, out outName))
            {
                sceneFader.FadeOut(outName);
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

        /// <summary>
        /// 現在のシナリオの最終行(シーン遷移などのはず)に飛ぶ
        /// </summary>
        public void TextSkip()
        {
            textIndex = texts.FindLastIndex(text => text == texts.Last());
            TextsDraw();
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
        string folderPath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Sources/";
        public void SetText(string fileName)
        {
            folderPath += progress.ThisStoryProgress.ToString() + "/";
            filePath = folderPath + fileName;
            Debug.Log(filePath);
            if (null == File.ReadAllText(filePath))
            {
                Debug.LogError("error");
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
