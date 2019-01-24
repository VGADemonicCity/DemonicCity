using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;



namespace DemonicCity.StoryScene
{
    public enum StageType
    {
        /// <summary>シーン遷移</summary>
        SceneTrans,
        /// <summary>登場</summary>
        Appear,
        /// <summary>退場</summary>
        Leave,
        /// <summary>画面の色変更</summary>
        Coloring,
        /// <summary>画面の色変更解除</summary>
        Clear,
        /// <summary>背景変更</summary>
        SwitchBack,
        /// <summary>アイテム出現</summary>
        Item,
        /// <summary>ウィンドウがポップする</summary>
        PopWindow,
    }

    public class TextDirector : MonoBehaviour
    {
        //public List<GameObject> Characters = new List<GameObject>();

        enum StageTag
        {
            Type, Target, Content
        }
        enum WindowTag
        {
            ToNext
        }

        enum BackIndex
        {
            Evening,
            Noon,
            Edge,

        }


        delegate void Stagings();
        string[] contents;
        public List<GameObject> characters = new List<GameObject>();
        [SerializeField] TextManager textManager;
        [SerializeField] GameObject blackOut;
        [SerializeField] GameObject[] popWindows;
        [SerializeField] Transform popCanvas;
        [SerializeField] SpriteRenderer backGround;

        SceneFader fader;
        float moveTime = 0.5f;

        List<Vector3> targetPositions = new List<Vector3>()
        {
            new Vector2(300,0),
            new Vector2(-300,0),
            new Vector2(0,0),
        };
        List<Vector3> startPositions = new List<Vector3>()
        {
            new Vector2(700,0),
            new Vector2(-700,0),
            new Vector2(0,0),
        };
        List<Cast> casts = new List<Cast>();

        //public void Staging(StageType type, string content)
        //{
        //    switch (type)
        //    {
        //        case StageType.SceneTrans:
        //            SceneTrans(content);
        //            break;
        //        case StageType.Appear:
        //            Appear(content);
        //            break;
        //        case StageType.Coloring:
        //            Coloring(content);
        //            break;
        //        case StageType.Clear:
        //            Clear();
        //            break;
        //        case StageType.SwitchBack:
        //            SwitchBack();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public void Staging(TextStorage storage)
        {
            fader = SceneFader.Instance;
            textManager.isStaging = true;
            DivideContent(storage.sentence);
            StageType type;
            if (!EnumCommon.TryParse(contents[(int)StageTag.Type], out type))
            {
                Debug.Log("Error : " + contents[(int)StageTag.Type]);

                return;
            }
            Debug.Log(type);
            switch (type)
            {
                case StageType.SceneTrans:
                    SceneTrans(contents[(int)StageTag.Target]);
                    break;
                case StageType.Appear:
                    Appear(contents);
                    break;
                case StageType.Leave:
                    Leave(contents[(int)StageTag.Target]);
                    break;
                case StageType.Coloring:
                    Coloring(contents[(int)StageTag.Target]);
                    break;
                case StageType.Clear:
                    Clear();
                    break;
                case StageType.SwitchBack:
                    SwitchBack(contents[(int)StageTag.Target]);
                    break;
                case StageType.Item:
                    Item(contents);
                    break;
                case StageType.PopWindow:
                    PopWindow(contents[(int)StageTag.Target]);
                    break;
                default:
                    break;
            }
        }

        public void SceneTrans(string content)
        {
            SceneFader.SceneTitle toScene;

            if (EnumCommon.TryParse(content, out toScene))
            {
                Progress progress = Progress.Instance;
                switch (toScene)
                {
                    case SceneFader.SceneTitle.Battle:
                        progress.ThisQuestProgress = Progress.QuestProgress.Battle;
                        break;
                    case SceneFader.SceneTitle.Story:
                        if (progress.ThisQuestProgress == Progress.QuestProgress.Prologue)
                        {
                            progress.ThisQuestProgress = Progress.QuestProgress.Epilogue;
                        }
                        else
                        {
                            progress.ThisQuestProgress = Progress.QuestProgress.Prologue;
                        }
                        break;
                    default:
                        progress.ThisQuestProgress = Progress.QuestProgress.None;
                        break;
                }
                Debug.Log(toScene);
                fader.FadeOut(toScene, 0.5f);
            }
            else
            {
                Debug.Log(toScene);
            }
            //EndStaging();
        }

        void Appear(string[] content)
        {
            CharName charName;
            PositionTag posTag;
            if (EnumCommon.TryParse(content[(int)StageTag.Target], out charName)
                && EnumCommon.TryParse(content[(int)StageTag.Content], out posTag))
            {
                casts.Add(new Cast(charName, posTag));
                GameObject appearCharObj = characters.Find(item => item.GetComponent<FaceChanger>().charName == charName);
                appearCharObj.transform.localPosition = startPositions[(int)posTag];

                Debug.Log(charName + ":" + posTag + ":" + appearCharObj.name);


                StartCoroutine(MoveObject(appearCharObj, targetPositions[(int)posTag], moveTime));
            }
        }

        void Leave(string content)
        {
            CharName charName;
            PositionTag posTag;
            if (EnumCommon.TryParse(content, out charName))
            {
                posTag = casts.Find(item => item.name == charName).posTag;
                GameObject leaveCharObj = characters.Find(item => item.GetComponent<FaceChanger>().charName == charName);
                StartCoroutine(MoveObject(leaveCharObj, startPositions[(int)posTag], moveTime));
            }
        }

        void Coloring(string content)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(content, out color))
            {
                StartCoroutine(ChangeColor(blackOut, color, 0.5f));

            }
        }
        void Coloring(Color color)
        {
            StartCoroutine(ChangeColor(blackOut, color, 0.5f));
        }
        void Clear()
        {
            StartCoroutine(ChangeColor(blackOut, Color.clear, 0.5f));
        }

        void SwitchBack(string content)
        {
            BackIndex index;
            if (EnumCommon.TryParse(content, out index))
            {
                Debug.Log("Assets/StoryScene/Sources/" + "BackGrounds/" + index.ToString() + ".jpg");
                Sprite back = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/StoryScene/Sources/" + "BackGrounds/" + index.ToString() + ".jpg");
                if (back)
                {
                    backGround.sprite = back;
                }
            }
            EndStaging();
        }

        void Item(string[] content)
        {
            StageType itemType;
            if (EnumCommon.TryParse(content[(int)StageTag.Target], out itemType))
            {
                switch (itemType)
                {
                    case StageType.Appear:
                        break;
                    case StageType.Leave:
                        break;
                    default:
                        break;
                }
            }

            EndStaging();

        }

        public void PopWindow(string content)
        {
            WindowTag windowTag;
            if (EnumCommon.TryParse(content, out windowTag))
            {
                popCanvas.gameObject.SetActive(true);
                GameObject newPopWindow = Instantiate(popWindows[(int)windowTag], popCanvas);
            }
            EndStaging();
        }

        void DivideContent(string content)
        {
            //string[] tmpTexts = content.Split(':');
            contents = content.Split(':');
            //Debug.Log(contents);
        }




        IEnumerator MoveObject(GameObject targetObj, Vector3 targetPos, float time)
        {
            float start = Time.time;
            Vector3 diff = targetPos - targetObj.transform.localPosition;
            while ((0 < diff.x
                && 0 < (targetPos - targetObj.transform.localPosition).x)
                || (diff.x < 0
                && (targetPos - targetObj.transform.localPosition).x < 0)
                && Time.time < start + time)
            {
                // ContinueStaging();
                targetObj.transform.localPosition += diff * Time.deltaTime / time;
                yield return null;
            }
            EndStaging();
            //while (dif.x < 0
            //    && (targetPos - targetObj.transform.localPosition).x < 0)
            //{
            //    targetObj.transform.localPosition += dif / 100;
            //    yield return new WaitForSeconds(0.01f);
            //}
        }

        IEnumerator ChangeColor(GameObject targetObject, Color toColor, float time)
        {
            Color fromColor;
            float start = Time.time;
            //if (targetObject.GetComponent<SpriteRenderer>())
            //{
            //    fromColor = targetObject.GetComponent<SpriteRenderer>().color;
            //}
            //else 
            if (targetObject.GetComponent<Image>())
            {
                fromColor = targetObject.GetComponent<Image>().color;
            }
            else
            {
                yield break;
            }
            float diffR = toColor.r - fromColor.r;
            float diffG = toColor.g - fromColor.g;
            float diffB = toColor.b - fromColor.b;
            float diffA = toColor.a - fromColor.a;

            while (Time.time < start + time)
            {
                //ContinueStaging();
                fromColor.r += diffR * Time.deltaTime / time;
                fromColor.g += diffG * Time.deltaTime / time;
                fromColor.b += diffB * Time.deltaTime / time;
                fromColor.a += diffA * Time.deltaTime / time;
                targetObject.GetComponent<SpriteRenderer>().color = fromColor;
                Debug.Log(fromColor);
                yield return null;
            }
            Debug.Log("end");
            targetObject.GetComponent<SpriteRenderer>().color = toColor;
            EndStaging();
        }


        void EndStaging()
        {
            textManager.isStaging = false;
            textManager.TextsDraw();
        }
        void ContinueStaging()
        {
            textManager.isStaging = true;
        }

    }



    public class Cast
    {
        public CharName name;
        public PositionTag posTag;

        public Cast(CharName charName, PositionTag positionTag)
        {
            name = charName;
            posTag = positionTag;
        }

    }
}