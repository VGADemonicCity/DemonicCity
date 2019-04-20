using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity.StoryScene
{
    public enum CharName
    {
        Magia,
        Phoenix,
        Nafla,
        Amon,
        Ashmedy,
        Faulus,
        Barl,
        InvigoratedPhoenix,
        Maou,
        Ixmagina,
        Unknown,
        None,
        System,
    }
    public class TextManager : MonoBehaviour
    {

        [SerializeField] Transform parentTransform;
        [SerializeField] GameObject characterObject;
        [SerializeField] Image backStage;
        [SerializeField] TMPro.TMP_Text nameObj;
        [SerializeField] Button skipButton;

        [SerializeField] TextDirector director;
        [SerializeField] PutSentence putSentence;

        List<TextActor> actors = new List<TextActor>();
        List<TextStorage> texts = new List<TextStorage>();
        List<FaceChanger> faceChangers = new List<FaceChanger>();
        List<CharName> characters = new List<CharName>();

        TouchGestureDetector touchGestureDetector;
        Progress progress;
        SoundManager soundM;
        ChapterManager chapterM;

        string unknownName = "???";
        string buttonTag = "Button";

        public bool isStaging = false;
        int textIndex = 0;
        public bool DrawEnd { get { return !isStaging & putSentence.End; } }
        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
            progress = Progress.Instance;
            chapterM = ChapterManager.Instance;
            soundM = SoundManager.Instance;
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


            SetText(progress.ThisQuestProgress);
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
                    PlayVoice();
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

        public CharName[] talker = new CharName[3] { CharName.None, CharName.None, CharName.None };



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
                nameObj.text = "？？？";
            }
            else if (storage.cName == CharName.None)
            {
                nameObj.text = "";
            }
            else
            {
                nameObj.text = actors.Find(x => x.id == storage.cName).name;
            }
            return true;


        }


        /// <summary>ListのCharNameに応じた名前を出力するが、演出がある場合はその演出を再生する</summary>
        bool DivideTexts()
        {
            TextStorage currentText = texts[textIndex];
            Debug.Log(currentText.cName);
            if (currentText.cName == CharName.System)
            {
                return false;
            }
            else if (currentText.cName != CharName.None)
            {
                Debug.Log(currentText.faceIndex);
                faceChangers.Find(x => x.charName == currentText.cName).ChangeFace(currentText.faceIndex);
            }

            Cast cast = new Cast();
            if (MissMatchTalker(out cast))
            {
                director.SwitchTalker(director.casts.Find(x => x.name == talker[(int)cast.posTag]), cast);
                talker[(int)cast.posTag] = cast.name;
            }


            if (currentText.isUnknown)
            {
                nameObj.text = unknownName;
            }
            else if (currentText.cName == CharName.None)
            {
                nameObj.text = "";
            }
            else
            {
                nameObj.text = actors.Find(x => x.id == currentText.cName).name;
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
                    if (talker[0] == cast.name
                        || talker[0] == CharName.None)
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
                    if (talker[1] == cast.name
                        || talker[1] == CharName.None)
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


        void PlayVoice()
        {
            if (texts[textIndex].voiceData != null)
            {
                soundM.PlayWithFade(SoundManager.SoundTag.Voice, texts[textIndex].voiceData);
            }
        }

        /// <summary>
        /// 現在のシナリオの最終行(シーン遷移などのはず)に飛ぶ
        /// </summary>
        public void TextSkip()
        {
            skipButton.interactable = false;
            director.Staging(texts[texts.Count - 1].sentence);
        }

        #region Settings

        public void SetText(Progress.QuestProgress currentState)
        {
            Chapter chapter = chapterM.GetChapter();
            Scenario tmp = chapter.scenario[currentState];
            characters = tmp.characters;
            texts = tmp.texts;
            SetCharacter(ref characters);

            int index = (int)progress.ThisQuestProgress;
            if (index < chapter.BattleStage.Count)
            {
                backStage.sprite = chapterM.GetChapter().BattleStage[(int)progress.ThisQuestProgress];
                backStage.enabled = true;
            }
            else
            {
                backStage.enabled = false;
            }

        }


        void SetCharacter(ref List<CharName> names)
        {
            director.charas.Clear();
            foreach (var item in names)
            {
                TextActor tmpActor = Resources.Load<TextActor>("Sources/StoryActors/" + item.ToString());
                GameObject charObj = Instantiate(characterObject, parentTransform);
                charObj.AddComponent(typeof(FaceChanger));
                charObj.GetComponent<FaceChanger>().Init(tmpActor);
                director.characters.Add(charObj);
                director.charas.Add(item, charObj);
                faceChangers.Add(charObj.GetComponent<FaceChanger>());
                actors.Add(tmpActor);
            }

        }

        #endregion

    }




}
