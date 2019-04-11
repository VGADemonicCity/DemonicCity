using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class SwitchBackForProgress : MonoBehaviour
    {

        [SerializeField] SpriteRenderer renderer = null;
        [SerializeField] UnityEngine.UI.Image image = null;
        [SerializeField] Sprite[] backGrounds = new Sprite[2];

        // Use this for initialization
        void Start()
        {
            if ((Progress.Instance.MyStoryProgress & Progress.StoryProgress.Ixmagina) == Progress.StoryProgress.Ixmagina)
            {
                ReflectSprite(backGrounds[1]);
            }
            else
            {
                ReflectSprite(backGrounds[0]);
            }
        }


        void ReflectSprite(Sprite sprite)
        {
            if (renderer != null)
            {
                renderer.sprite = sprite;
            }
            else
            {
                image.sprite = sprite;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}