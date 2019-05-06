using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



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
        /// <summary>画面が揺れる</summary>
        Quake,

        QuestClear,
    }

    public class TextDirector : MonoBehaviour
    {
        enum StageTag
        {
            Type, Target, Content
        }
        public enum WindowTag
        {
            ToNext,
            Skip,
        }

        enum StageIndex
        {
            Edge,
            Castle,
            Ground,
        }

        enum ItemTag
        {
            Clown,
        }

        public Dictionary<CharName, GameObject> charas = new Dictionary<CharName, GameObject>();

        public List<GameObject> characters = new List<GameObject>();
        public List<FaceChanger> faces = new List<FaceChanger>();
        [SerializeField] TextManager textManager;
        [SerializeField] Image itemImage;
        [SerializeField] GameObject blackOut;
        [SerializeField] GameObject[] popWindows;
        [SerializeField] Transform popCanvas;
        [SerializeField] Image backStage;
        [SerializeField] List<Sprite> backStageSources = new List<Sprite>();
        [SerializeField] List<Sprite> itemSources = new List<Sprite>();
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
            new Vector2(2000,0),
            new Vector2(-2000,0),
            new Vector2(-2000,0),
        };
        public List<Cast> casts = new List<Cast>();

        public void Staging(string sentence)
        {
            fader = SceneFader.Instance;
            textManager.isStaging = true;
            string[] contents = sentence.Split(':'); ;
            StageType type;
            if (!EnumCommon.TryParse(contents[(int)StageTag.Type], out type))
            {
                StagingError(contents[(int)StageTag.Type]);

                return;
            }
            //Debug.Log(type);
            switch (type)
            {
                case StageType.SceneTrans:
                    SceneTrans(contents[(int)StageTag.Target]);
                    break;
                case StageType.Appear:
                    Appear(ref contents);
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
                    SwitchBack(ref contents[(int)StageTag.Target]);
                    break;
                case StageType.Item:
                    Item(contents);
                    break;
                case StageType.PopWindow:
                    PopWindow(contents[(int)StageTag.Target]);
                    break;
                case StageType.Quake:
                    Quake();
                    break;
                case StageType.QuestClear:
                    QuestClear(contents[(int)StageTag.Target]);
                    break;
                default:
                    break;
            }
        }


        #region Staging

        void StagingError(string s)
        {
            //Debug.Log("Error : " + s);
            EndStaging();
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
                fader.FadeOut(toScene);
            }
        }

        void Appear(ref string[] content)
        {
            CharName charName;
            PositionTag posTag;
            if (EnumCommon.TryParse(content[(int)StageTag.Target], out charName)
                && EnumCommon.TryParse(content[(int)StageTag.Content], out posTag))
            {
                casts.Add(new Cast(charName, posTag));
                GameObject appearCharObj = characters.Find(item => item.GetComponent<FaceChanger>().charName == charName);
                appearCharObj.transform.localPosition = startPositions[(int)posTag];

                //Debug.Log(charName + ":" + posTag + ":" + appearCharObj.name);

                if (textManager.talker[(int)posTag] == CharName.None)
                {
                    textManager.talker[(int)posTag] = charName;
                    if (posTag == PositionTag.Center)
                    {
                        GameObject leaveCharObj = characters.Find(item => item.GetComponent<FaceChanger>().charName == textManager.talker[0]);
                        StartCoroutine(MoveObject(leaveCharObj, startPositions[0], moveTime, false));
                        leaveCharObj = characters.Find(item => item.GetComponent<FaceChanger>().charName == textManager.talker[1]);
                        StartCoroutine(MoveObject(leaveCharObj, startPositions[1], moveTime, false));
                    }
                    StartCoroutine(MoveObject(appearCharObj, targetPositions[(int)posTag], moveTime));
                }
                else if (textManager.talker[(int)posTag] != charName)
                {
                    EndStaging();
                }

            }
        }

        public void SwitchTalker(Cast nowCast, Cast nextCast)
        {
            textManager.isStaging = true;


            GameObject leaveCharObj = charas[nowCast.name];
            if (leaveCharObj)
            {
                StartCoroutine(MoveObject(leaveCharObj, startPositions[(int)nowCast.posTag], moveTime, false));
            }

            GameObject appearCharObj = charas[nextCast.name];
            if (appearCharObj)
            {
                appearCharObj.transform.localPosition = startPositions[(int)nextCast.posTag];
                StartCoroutine(MoveObject(appearCharObj, targetPositions[(int)nextCast.posTag], moveTime, false));
            }
        }

        void Leave(string content)
        {
            CharName charName;
            PositionTag posTag;
            if (EnumCommon.TryParse(content, out charName))
            {
                posTag = casts.Find(item => item.name == charName).posTag;
                CharName leaveCharName = textManager.talker[(int)posTag];
                GameObject leaveCharObj = characters.Find(item => item.GetComponent<FaceChanger>().charName == charName);
                StartCoroutine(MoveObject(leaveCharObj, startPositions[(int)posTag], moveTime));
                if (leaveCharName == charName)
                {
                    Cast tmp = casts.Find(x => x.name != charName && x.posTag == posTag);
                    if (tmp != null)
                    {
                        StartCoroutine(MoveObject(characters.Find(item => item.GetComponent<FaceChanger>().charName == tmp.name), targetPositions[(int)tmp.posTag], moveTime, false));
                        textManager.talker[(int)posTag] = tmp.name;
                        //leaveCharName = tmp.name;
                    }
                    else
                    {
                        textManager.talker[(int)posTag] = CharName.None;
                        //leaveCharName = CharName.None;
                    }
                    if (posTag == PositionTag.Center)
                    {
                        GameObject appearCharObj = characters.Find(item => item.GetComponent<FaceChanger>().charName == textManager.talker[0]);
                        StartCoroutine(MoveObject(appearCharObj, targetPositions[0], moveTime, false));
                        appearCharObj = characters.Find(item => item.GetComponent<FaceChanger>().charName == textManager.talker[1]);
                        StartCoroutine(MoveObject(appearCharObj, targetPositions[1], moveTime, false));
                    }

                }
                casts.RemoveAll(x => x.name == charName);
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
        void Coloring(Color color, bool toNext = true)
        {
            StartCoroutine(ChangeColor(blackOut, color, 0.5f, toNext));
        }
        void Clear()
        {
            StartCoroutine(ChangeColor(blackOut, Color.clear, 0.5f));
        }



        #endregion



        [SerializeField] SpriteRenderer BackGround;
        [SerializeField] Sprite afterGround;
        void SwitchBack(ref string content)
        {
            StageIndex index;
            if (EnumCommon.TryParse(content, out index))
            {
                if (index == StageIndex.Ground)
                {
                    BackGround.sprite = afterGround;
                    EndStaging();
                    return;
                }
                if ((int)index < backStageSources.Count)
                {
                    backStage.sprite = backStageSources[(int)index];
                }
            }
            EndStaging();
        }

        #region Item

        void Item(string[] content)
        {
            StageType itemType;
            if (EnumCommon.TryParse(content[(int)StageTag.Target], out itemType))
            {
                switch (itemType)
                {
                    case StageType.Appear:
                        ItemAppear(content[(int)StageTag.Content]);
                        break;
                    case StageType.Leave:
                        ItemLeave(content[(int)StageTag.Content]);
                        break;
                    default:
                        EndStaging();
                        break;
                }
            }
        }

        void ItemAppear(string content)
        {
            ItemTag item;
            if (GetItem(content, out item))
            {
                StartCoroutine(FadeInObject(itemImage, itemSources[(int)item]));
            }
            else
            {
                EndStaging();
            }

        }


        void ItemLeave(string content)
        {
            ItemTag item;
            if (GetItem(content, out item))
            {
                StartCoroutine(FadeOutObject(itemImage, itemSources[(int)item]));
            }
            else
            {
                EndStaging();
            }
        }
        bool GetItem(string s, out ItemTag item)
        {
            return EnumCommon.TryParse(s, out item);
        }

        //アイテム用
        IEnumerator FadeInObject(Image targetImage, Sprite sprite, float fade = 1f)
        {

            targetImage.color = Color.clear;
            targetImage.sprite = sprite;
            float a = 0f;
            float time = 0f;

            while (time < fade)
            {
                a += Time.deltaTime / fade;
                targetImage.color = new Color(1, 1, 1, a);
                time += Time.deltaTime;
                yield return null;
            }
            targetImage.color = Color.white;

            EndStaging();
        }

        IEnumerator FadeOutObject(Image targetImage, Sprite sprite, float fade = 1f)
        {
            targetImage.sprite = sprite;
            targetImage.color = Color.white;
            float a = 1f;
            float time = 0f;

            while (time < fade)
            {
                a -= Time.deltaTime / fade;
                targetImage.color = new Color(1, 1, 1, a);
                time += Time.deltaTime;
                yield return null;
            }
            targetImage.color = Color.clear;

            EndStaging();
        }

        #endregion

        public void PopWindow(string content)
        {
            Coloring(Color.black, false);
            WindowTag windowTag;
            if (EnumCommon.TryParse(content, out windowTag))
            {
                StartCoroutine(WaitPopWindow(windowTag));
            }
        }
        IEnumerator WaitPopWindow(WindowTag windowTag)
        {
            yield return new WaitWhile(() => textManager.isStaging);
            textManager.isStaging = true;
            textManager.TextClear();

            popCanvas.gameObject.SetActive(true);
            Instantiate(popWindows[(int)windowTag], popCanvas);

            //EndStaging();
        }

        public void PopWindow(WindowTag windowTag)
        {
            Coloring(Color.black, false);
            StartCoroutine(WaitPopWindow(windowTag));
        }


        void Quake()
        {
            if (isQuake)
            {
                isQuake = false;
            }
            else
            {
                isQuake = true;
                StartCoroutine(QuakeObject(Camera.main.transform, 1f, 0.1f));
            }

            EndStaging();
        }



        void QuestClear(string content)
        {
            Progress.StoryProgress story;
            if (EnumCommon.TryParse(content, out story))
            {
                Progress.Instance.MyStoryProgress = Progress.Instance.MyStoryProgress | story;
            }
            //SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Home);

            PopWindow(WindowTag.ToNext);

        }


        bool isQuake = false;
        IEnumerator QuakeObject(Transform targetObj, float lim, float deflection)
        {
            Vector3 originPos = targetObj.localPosition;

            while (isQuake)
            {
                float x = originPos.x + UnityEngine.Random.Range(-1f, 1f) * deflection;
                float y = originPos.y + UnityEngine.Random.Range(-1f, 1f) * deflection;

                targetObj.localPosition = new Vector3(x, y, originPos.z);


                yield return null;
            }
            targetObj.localPosition = originPos;


        }



        IEnumerator MoveObject(GameObject targetObj, Vector3 targetPos, float time, bool isNext = true)
        {
            float start = Time.time;
            Vector3 diff = targetPos - targetObj.transform.localPosition;
            while ((0 < diff.x
                && 0 < (targetPos - targetObj.transform.localPosition).x)
                || (diff.x < 0
                && (targetPos - targetObj.transform.localPosition).x < 0)
                && Time.time < start + time)
            {
                targetObj.transform.localPosition += diff * Time.deltaTime / time;
                yield return null;
            }
            if (isNext)
            {
                EndStaging();
            }
            else
            {
                textManager.isStaging = false;
            }
        }

        IEnumerator ChangeColor(GameObject targetObject, Color toColor, float time, bool isNext = true)
        {
            Color fromColor;
            float start = Time.time;
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
                targetObject.GetComponent<Image>().color = fromColor;
                //Debug.Log(fromColor);
                yield return null;
            }
            //Debug.Log("end");
            targetObject.GetComponent<Image>().color = toColor;
            if (isNext)
            {
                EndStaging();
            }
            else
            {
                textManager.isStaging = false;
            }
        }


        void EndStaging()
        {
            textManager.isStaging = false;

            StartCoroutine(textManager.TextDraw());

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
        public Cast()
        {
            name = CharName.None;
            posTag = PositionTag.None;
        }
    }
}