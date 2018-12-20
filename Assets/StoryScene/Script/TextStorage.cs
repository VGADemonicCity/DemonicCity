using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StoryScene
{
    public enum FaceIndex
    {
        Normal, Fun, Angry,Surprise,Suffer,Shout,Last
    }
    [System.Serializable]
    public class TextStorage
    {
        /// <summary>/// /// </summary>
        public enum CharSpriteIndex
        {
            Magia
        }
        /// <summary>キャラ名</summary>
        public string charName;
        public CharName cName;
        /// <summary>内容</summary>
        public string sentence;
        /// <summary>表情</summary>
        public string face;
        /// <summary>表情</summary>
        public int faceIndex;
        /// <summary>正体が判明しているか</summary>
        public string unknown;
        public bool isUnknown;
        /// <summary>演出内容</summary>
        public string stage;
        /// <summary>演出の種類</summary>
        public StageType stageTag;

        /// <summary>コンストラクタ</summary>
        public TextStorage(TextStorage storage)
        {
            charName = storage.charName;
            sentence = storage.sentence;
            stage = storage.stage;
            FaceIndex tmpIndex;
            CharName tmpName;
            StageType tmpStage;


            if (storage.unknown == "")
            {
                isUnknown = false;
            }
            else
            {
                isUnknown = true;
            }
            if (EnumCommon.TryParse<CharName>(storage.charName, out tmpName))
            {
                cName = tmpName;
            }
            else
            {
                cName = CharName.None;
            }

            if (EnumCommon.TryParse<FaceIndex>(storage.face, out tmpIndex))
            {
                faceIndex = (int)tmpIndex;
            }
            else
            {
                faceIndex = (int)FaceIndex.Last;
            }
        }
        public TextStorage(string s)
        {
            sentence = s;
        }
        public TextStorage()
        {

        }
        public TextStorage(int i)
        {

        }

    }
}
