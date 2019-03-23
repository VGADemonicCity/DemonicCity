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


        public IEnumerator TextDraw()
        {
            yield return new WaitWhile(() => !DrawEnd);
            textIndex += 1;
            if (DivideTexts(texts[textIndex]))
            {
                putSentence.CallSentence(texts[textIndex].sentence);
            }
            else
            {
                director.Staging(texts[textIndex].sentence);
            }
        }

        /// <summary>
        /// 次の行を再生する。
        /// </summary>
        public void TextsDraw()
        {

            if (textIndex < texts.Count - 1)
            {
                if (putSentence.End)
                {
                    textIndex += 1;
                }
                TextStorage thisText = texts[textIndex];
                if (DivideTexts(thisText))
                {
                    putSentence.CallSentence(thisText.sentence);
                }
                else
                {
                    director.Staging(thisText.sentence);
                }

            }
        }



        /// <summary>
        /// 今の行を再生する。
        /// </summary>
        public void ThisTextDraw()
        {
            if (putSentence.End)
            {
                if (DivideTexts(texts[textIndex]))
                {
                    putSentence.CallSentence(texts[textIndex].sentence);
                }
                else
                {
                    director.Staging(texts[textIndex].sentence);
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



        bool DivideTexts(TextStorage storage)
        {
            if (storage.cName == CharName.System)
            {
                return false;
            }
            else if (storage.cName != CharName.None)
            {
                Debug.Log(storage.faceIndex);
                foreach (FaceChanger item in faceChangers)
                {
                    if (item.charName == storage.cName)
                    {
                        item.ChangeFace(storage.faceIndex);
                    }
                    else
                    {
                        item.DeSelect();
                    }
                }
                faceChangers.Find(x => x.charName == storage.cName).ChangeFace(storage.faceIndex);
            }

            Cast cast = new Cast();
            if (MissMatchTalker(out cast))
            {
                director.SwitchTalker(director.casts.Find(x => x.name == talker[(int)cast.posTag]), cast);
                talker[(int)cast.posTag] = cast.name;

            }


            if (storage.isUnknown)
            {
                //nameObj.text = CHARNAME[(int)CharName.Unknown];
                nameObj.text = "？？？";
            }
            else if (storage.cName == CharName.None)
            {
                nameObj.text = "";
            }
            else
            {
                //nameObj.text = CHARNAME[(int)storage.cName];
                nameObj.text = actors.Find(x => x.id == storage.cName).name;
            }
            return true;


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




        /// <summary>
        /// 現在のシナリオの最終行(シーン遷移などのはず)に飛ぶ
        /// </summary>
        public void TextSkip()
        {

            textIndex = texts.Count - 1;
            skipButton.interactable = false;
            //ThisTextDraw();
            director.Staging(texts[textIndex].sentence);
        }

        #region Settings

        string filePath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Texts.json";

        public string FolderPath { get { return "Sources/"; } }


        public void SetText(string fileName)
        {
            filePath = FolderPath + progress.ThisStoryProgress.ToString() + "/" + fileName;
            if (Resources.Load(filePath) == null)
            {
                filePath = FolderPath + progress.ThisStoryProgress.ToString() + "/Test";
            }

            Scenario tmp = Resources.Load<Scenario>(filePath);
            //tmp.texts.ForEach(x => characters.Add(x.cName));
            characters = tmp.characters;
            texts = tmp.texts;
            //characters = characters.Distinct().Where(item => item <= CharName.Ixmagina).ToList();
            SetCharacter(characters);
        }



        void SetCharacter(List<CharName> names)
        {
            foreach (var item in names)
            {
                TextActor tmpActor = Resources.Load<TextActor>("Sources/StoryActors/" + item.ToString());
                GameObject charObj = Instantiate(characterObject, parentTransform);
                charObj.AddComponent(typeof(FaceChanger));
                //charObj.GetComponent<FaceChanger>().Init(actors.Find(x => x.id == item));
                charObj.GetComponent<FaceChanger>().Init(tmpActor);
                director.characters.Add(charObj);
                faceChangers.Add(charObj.GetComponent<FaceChanger>());
                actors.Add(tmpActor);
            }

        }

        #endregion

    }




}
