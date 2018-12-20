using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.StoryScene
{
    public class FaceManager : MonoBehaviour
    {

        public Sprite[] faceSprites = new Sprite[6];
        Image myFace;
        void Awake()
        {
            myFace = GetComponent<Image>();
        }

        public void InportSprite(Sprite[] sprites)
        {
            faceSprites = sprites;
            myFace.sprite = faceSprites[(int)FaceIndex.Normal];
        }

        public void ChangeFace(FaceIndex faceIndex)
        {
            myFace.sprite = faceSprites[(int)faceIndex];
        }
        
    }
}