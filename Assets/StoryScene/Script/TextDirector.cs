using System;
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
    }

    public class TextDirector : MonoBehaviour
    {
        //public List<GameObject> Characters = new List<GameObject>();

        enum StageTag
        {
            Type, Target, Content
        }

        delegate void Stagings();
        string[] contents;
        public List<GameObject> characters = new List<GameObject>();
        [SerializeField] TextManager textManager;
        [SerializeField] GameObject blackOut;

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
            textManager.isStaging = true;
            DivideContent(storage.sentence);
            StageType type;
            if (!EnumCommon.TryParse(contents[(int)StageTag.Type], out type))
            {

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
                    SwitchBack();
                    break;
                case StageType.Item:
                    Item(contents[(int)StageTag.Target]);
                    break;
                default:
                    break;
            }
        }

        void SceneTrans(string content)
        {

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


                StartCoroutine(MoveObject(appearCharObj, targetPositions[(int)posTag]));
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
                StartCoroutine(MoveObject(leaveCharObj, startPositions[(int)posTag]));
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

        void Clear()
        {
            StartCoroutine(ChangeColor(blackOut, Color.clear, 0.5f));
        }

        void SwitchBack()
        {
            EndStaging();
        }

        void Item(string content)
        {

        }


        void DivideContent(string content)
        {
            //string[] tmpTexts = content.Split(':');
            contents = content.Split(':');
            //Debug.Log(contents);
        }




        IEnumerator MoveObject(GameObject targetObj, Vector3 targetPos)
        {
            Vector3 dif = targetPos - targetObj.transform.localPosition;
            while ((0 < dif.x
                && 0 < (targetPos - targetObj.transform.localPosition).x)
                || (dif.x < 0
                && (targetPos - targetObj.transform.localPosition).x < 0))
            {
                targetObj.transform.localPosition += dif / 100;
                yield return new WaitForSeconds(0.01f);
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
                fromColor.r += diffR * Time.deltaTime / time;
                fromColor.g += diffG * Time.deltaTime / time;
                fromColor.b += diffB * Time.deltaTime / time;
                fromColor.a += diffA * Time.deltaTime / time;
                targetObject.GetComponent<SpriteRenderer>().color = fromColor;
                yield return null;
            }
            targetObject.GetComponent<SpriteRenderer>().color = toColor;
            EndStaging();
        }


        void EndStaging()
        {
            textManager.isStaging = false;
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