using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
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
        /// <summary>/// /// </summary>
        public string charName;
        /// <summary>/// /// </summary>
        public string sentence;
        /// <summary>/// /// </summary>
        public string face;
        /// <summary>/// /// </summary>
        public int faceIndex;
        /// <summary>/// /// </summary>
        public TextStorage(TextStorage storage)
        {
            charName = storage.charName;
            sentence = storage.sentence;
            FaceIndex tmpIndex;
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
