using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DemonicCity.StoryScene
{
    public enum StageType
    {
        SceneTrans, Appear, Coloring, Clear, SwitchBack
    }
    public class TextDirector : MonoBehaviour
    {
        enum StageTag
        {
            Type, content
        }

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

            switch (type)
            {
                case StageType.SceneTrans:
                    SceneTrans(storage.sentence);
                    break;
                case StageType.Appear:
                    Appear(storage.sentence);
                    break;
                case StageType.Coloring:
                    Coloring(storage.sentence);
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
            string[] tmpTexts = content.Split(':');
            contents = content.Split(':');
            
        }
    }
}