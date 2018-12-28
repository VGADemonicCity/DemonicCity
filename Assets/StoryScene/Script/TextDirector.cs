using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
        enum StageTag
        {
            Type, Content
        }

        delegate void Stagings();
        string[] contents;

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
            DivideContent(storage.sentence);
            StageType type;
            if (!EnumCommon.TryParse<StageType>(contents[(int)StageTag.Type], out type))
            {

                return;
            }
            Debug.Log(type);
            switch (type)
            {
                case StageType.SceneTrans:
                    SceneTrans(contents[(int)StageTag.Content]);
                    break;
                case StageType.Appear:
                    Appear(contents[(int)StageTag.Content]);
                    break;
                case StageType.Leave:
                    Leave(contents[(int)StageTag.Content]);
                    break;
                case StageType.Coloring:
                    Coloring(contents[(int)StageTag.Content]);
                    break;
                case StageType.Clear:
                    Clear();
                    break;
                case StageType.SwitchBack:
                    SwitchBack();
                    break;
                case StageType.Item:
                    Item(contents[(int)StageTag.Content]);
                    break;
                default:
                    break;
            }
        }

        void SceneTrans(string content)
        {

        }

        void Appear(string content)
        {

        }

        void Leave(string content)
        {

        }

        void Coloring(string content)
        {

        }

        void Clear()
        {

        }

        void SwitchBack()
        {

        }

        void Item(string content)
        {

        }


        void DivideContent(string content)
        {
            //string[] tmpTexts = content.Split(':');
            contents = content.Split(':');
            Debug.Log(contents);
        }
    }
}