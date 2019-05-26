using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StoryScene
{
    public class StoryBGMPlayer : MonoBehaviour
    {
        SoundManager soundManager;
        ChapterManager chapterManager;

        void Awake()
        {
            soundManager = SoundManager.Instance;
            chapterManager = ChapterManager.Instance;
        }


        void Start()
        {
            soundManager.PlayWithFade(SoundManager.SoundTag.BGM, chapterManager.GetChapter().StoryBgm);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}