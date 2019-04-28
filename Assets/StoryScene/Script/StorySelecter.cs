using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StorySelectScene
{
    using Story = Progress.StoryProgress;
    public class StorySelecter : MonoBehaviour
    {
        Progress progress;
        Story MyStory;
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

            //progress.MyStoryProgress= Progress.StoryProgress.All;

            int progressCount = EnumCommon.GetLength<Story>() - 1;
            for (int i = progressCount - 1; 0 <= i; i--)
            {
                Story progressIndex = (Story)(1 << i);
                Story previewIndex = (Story)((int)progressIndex / 2);
                if ((previewIndex & MyStory) == previewIndex)
                {
                    GameObject newSelectButton = Instantiate(SelectButton, parent);
                    newSelectButton.GetComponent<SelectButton>().Initialize(chapterManager.GetChapter(progressIndex));
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
        public void ToStory(Progress.StoryProgress chapter)
        {
            Progress progress = Progress.Instance;
            //Debug.Log(chapter.ToString());
            progress.ThisStoryProgress = chapter;
            Chapter thisChapter = ChapterManager.Instance.GetChapter();
            if (thisChapter.isStory)
            {
                progress.ThisQuestProgress = Progress.QuestProgress.Prologue;
                SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Story);
            }
            else
            {
                progress.ThisQuestProgress = Progress.QuestProgress.Battle;
                SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Battle);
            }

        }
    }

}
