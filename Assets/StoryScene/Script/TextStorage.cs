using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StoryScene
{
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
            Happy,Mad,Sad,Fun
        }
        /// <summary>/// /// </summary>
        public string sentence;
        /// <summary>/// /// </summary>
        public int face;
        /// <summary>/// /// </summary>

        /// <summary>/// /// </summary>

        /// <summary>/// /// </summary>
        public TextStorage(TextStorage storage)
        {
            
        }
        public TextStorage(string s)
        {
            sentence = s;
        }
        public TextStorage()
        {
         
        }
    }
}
