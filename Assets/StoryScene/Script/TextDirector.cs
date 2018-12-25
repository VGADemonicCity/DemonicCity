using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DemonicCity.StoryScene
{
    public enum StageType
    {
        SceneTrans, Appear, Leave, Coloring, Clear, SwitchBack
    }
    public class TextDirector : MonoBehaviour
    {
        enum StageTag
        {
            Type, content
        }

        delegate void Stagings() ;
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
                    SceneTrans(contents[(int)StageTag.content]);
                    break;
                case StageType.Appear:
                    Appear(contents[(int)StageTag.content]);
                    break;
                case StageType.Leave:
                    Leave(contents[(int)StageTag.content]);
                    break;
                case StageType.Coloring:
                    Coloring(contents[(int)StageTag.content]);
                    break;
                case StageType.Clear:
                    Clear();
                    break;
                case StageType.SwitchBack:
                    SwitchBack();
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


        void DivideContent(string content)
        {
            //string[] tmpTexts = content.Split(':');
            contents = content.Split(':');
            Debug.Log(contents);
        }
    }
}