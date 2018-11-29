using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public enum FaceIndex
    {
        Nomal, Happy, Mad, Sad, Fun, Last
    }
    [System.Serializable]
    public class TextStorage
    {
        /// <summary>/// /// </summary>
        public enum CharSpriteIndex
        {
            Magia
        }
        /// <summary>/// /// </summary>
        public enum FaceIndex
        {
            Nomal,Happy, Mad, Sad, Fun,Last
        }
        /// <summary></summary>
        public string charName;
        public CharName cName;
        /// <summary></summary>
        public string sentence;
        /// <summary></summary>
        public string face;
        /// <summary></summary>
        public int faceIndex;
        /// <summary></summary>
        public string unknown;
        public bool isUnknown;


        /// <summary>コンストラクタ</summary>
        public TextStorage(TextStorage storage)
        {
            charName = storage.charName;
            sentence = storage.sentence;
            FaceIndex tmpIndex;
            CharName tmpName;
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
