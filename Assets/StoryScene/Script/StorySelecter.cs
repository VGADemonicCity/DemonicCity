using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StorySelectScene
{
    public class StorySelecter : MonoBehaviour
    {
        Progress progress;
        Progress.StoryProgress MyStory;
        [SerializeField] ChapterManager chapterManager;
        [SerializeField] RectTransform parent;
        [SerializeField] GameObject SelectButton;

        void Awake()
        {
            progress = Progress.Instance;
            chapterManager = ChapterManager.Instance;
        }
        void Start()
        {
            MyStory = progress.MyStoryProgress;
            int progressCount = EnumCommon.GetLength<Progress.StoryProgress>() - 1;
            for (int i = progressCount - 1; 0 <= i; i--)
            {
                int progressIndex = 1 << i;
                if (((Progress.StoryProgress)progressIndex & MyStory) == (Progress.StoryProgress)progressIndex)
                {
                    GameObject newSelectButton = Instantiate(SelectButton, parent);
                    newSelectButton.GetComponent<SelectButton>().Initialize((Progress.StoryProgress)progressIndex, chapterManager.GetTitle((Progress.StoryProgress)progressIndex), this);
                }
            }
            //for (int i = (int)MyStory; i >= 1; i /= 2)
            //{
            //    GameObject newSelectButton = Instantiate(SelectButton, parent);
            //    newSelectButton.GetComponent<SelectButton>().Initialize((Progress.StoryProgress)i, chapterManager.GetTitle((Progress.StoryProgress)i), this);
            //}

        }




    }
    public class ToStories
    {
        Progress progress = Progress.Instance;
        public void ToStory(Progress.StoryProgress chapter)
        {
            Debug.Log(chapter.ToString());
            progress.ThisStoryProgress = chapter;
            progress.ThisQuestProgress = Progress.QuestProgress.Prologue;
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Story);
        }
    }

}
