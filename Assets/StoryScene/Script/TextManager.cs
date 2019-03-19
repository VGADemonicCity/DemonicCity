using System;
using System.Linq;
//using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity.StoryScene
{
    public enum CharName
    {
        Magia, Phoenix, Nafla, Amon, Ashmedy, Faulus, Barl, InvigoratedPhoenix, Maou, Ixmagina, Unknown, None, System
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
        "フィニクス",
        "イクスマギア",
        "魔王",
        "???",
        "",
        };


        [SerializeField] TextDirector director;
        [SerializeField] Transform parentTransform;
        [SerializeField] List<GameObject> characterObjects = new List<GameObject>();
        [SerializeField] GameObject characterObject;
        [SerializeField] List<TextActor> actors = new List<TextActor>();
        List<FaceChanger> faceChangers = new List<FaceChanger>();
        List<CharName> characters = new List<CharName>();
        Progress progress;

        public bool isStaging = false;
        string buttonTag = "Button";
        //bool flag;
        int textIndex = 0;
        public TouchGestureDetector touchGestureDetector;
        SceneFader sceneFader;
        List<TextStorage> texts = new List<TextStorage>();
        [SerializeField] PutSentence putSentence;
        [SerializeField] TMPro.TMP_Text nameObj;
        [SerializeField] UnityEngine.UI.Button skipButton;
        public bool DrawEnd { get { return !isStaging & putSentence.End; } }
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
            //if (progress.ThisQuestProgress != Progress.QuestProgress.Prologue)
            //{
            //    progress.ThisQuestProgress = Progress.QuestProgress.Epilogue;
            //}
            //


            //ファイル名からQuestProgressを素即して、Trueなら変換して代入する。
            //Progress.QuestProgress tmpQuest;
            //if (EnumCommon.TryParse())
            //{

            //}
            actors = Resources.LoadAll<TextActor>("Sources/StoryActors").ToList();



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


            SetText(progress.ThisQuestProgress.ToString());
            ThisTextDraw();
        }

        void Update()
        {
            bool isAuto = false;
            if (Input.GetKeyDown(KeyCode.A))
            {
                isAuto = true;
            }
            if (isAuto)
            {
                TextsDraw();
            }
        }


        IEnumerator TextDraw()
        {
            yield return new WaitWhile(() => !DrawEnd);
            textIndex += 1;
            if (DivideTexts())
            {
                putSentence.CallSentence(texts[textIndex].sentence);
            }
            else
            {
                director.Staging(texts[textIndex]);
            }
        }

        /// <summary>
        /// 次の行を再生する。
        /// </summary>
        public void TextsDraw()
        {
            if (putSentence.End)
            {
                if (textIndex < texts.FindLastIndex(text => text == texts.Last()))
                {
                    textIndex += 1;
                }
                else
                {
                    return;
                }
                if (DivideTexts())
                {
                    putSentence.CallSentence(texts[textIndex].sentence);
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



        /// <summary>
        /// 今の行を再生する。
        /// </summary>
        public void ThisTextDraw()
        {
            if (putSentence.End)
            {
                if (DivideTexts())
                {
                    putSentence.CallSentence(texts[textIndex].sentence);
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


        public void TextClear()
        {
            putSentence.Init();
            nameObj.text = "";
        }

        public CharName[] talker = new CharName[2] { CharName.None, CharName.None };

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

            Cast cast = new Cast();
            if (MissMatchTalker(out cast))
            {
                director.SwitchTalker(director.casts.Find(x => x.name == talker[(int)cast.posTag]), cast);
                talker[(int)cast.posTag] = cast.name;
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

        bool MissMatchTalker(out Cast cast)
        {
            cast = director.casts.Find(x => x.name == texts[textIndex].cName);
            if (cast != null)
            {
                if (cast.posTag == PositionTag.Ally)
                {
                    if (talker[0] == cast.name)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (cast.posTag == PositionTag.Enemy)
                {
                    if (talker[1] == cast.name)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
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

        void SetCharacter(List<CharName> names)
        {
            foreach (var item in names)
            {
                GameObject charObj = Instantiate(characterObject, parentTransform);
                charObj.AddComponent(typeof(FaceChanger));
                charObj.GetComponent<FaceChanger>().Init(actors.Find(x => x.id == item));
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
            if (progress.ThisQuestProgress == Progress.QuestProgress.Epilogue)
            {
                textIndex -= 1;
            }
            skipButton.interactable = false;
            ThisTextDraw();
        }

        string filePath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Texts.json";
        //public void SetText()
        //{
        //    if (null == File.ReadAllText(filePath))
        //    {
        //        return;
        //    }
        //    string textsJson = File.ReadAllText(filePath);
        //    Debug.Log(textsJson);
        //    string[] spritKey = { "><" };

        //    string[] tmpTexts = textsJson.Split(spritKey, StringSplitOptions.None);
        //    //Debug.Log(tmpTexts);
        //    foreach (string s in tmpTexts)
        //    {

        //        //Debug.Log(s);
        //        var sss = JsonUtility.FromJson<TextStorage>(s);
        //        //Debug.Log(sss);   
        //        if (sss.face == null || sss.face == "")
        //        {
        //            sss.face = FaceIndex.Last.ToString();
        //        }
        //        var tmpStorage = new TextStorage(JsonUtility.FromJson<TextStorage>(s));
        //        characters.Add((int)tmpStorage.cName);
        //        //Debug.Log(tmpStorage);
        //        texts.Add(tmpStorage);

        //    }
        //    characters = characters.Distinct().Where(item => item <= (int)CharName.Ixmagina).ToList();
        //    SetCharacter(characters);
        //}
        public string FolderPath { get { return "Sources/"; } }

        public void SetText(string fileName)
        {
            //string folderPath += progress.ThisStoryProgress.ToString() + "/";
            filePath = FolderPath + progress.ThisStoryProgress.ToString() + "/" + fileName;
            Debug.Log(filePath);

            if (null == Resources.Load(filePath))
            {
                Debug.LogError("ScenarioNotFound : " + filePath);

                filePath = FolderPath + progress.ThisStoryProgress.ToString() + "/Test";
            }

            TextAsset textAsset = Instantiate((TextAsset)Resources.Load(filePath));
            string textsJson = textAsset.text;
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
                characters.Add(tmpStorage.cName);
                //Debug.Log(tmpStorage);
                texts.Add(tmpStorage);

            }
            characters = characters.Distinct().Where(item => item <= CharName.Ixmagina).ToList();
            SetCharacter(characters);
        }
    }




}
