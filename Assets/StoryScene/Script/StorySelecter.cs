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

            for (int i = (int)MyStory; i >= 1; i /= 2)
            {
                GameObject newSelectButton = Instantiate(SelectButton, parent);
                newSelectButton.GetComponent<SelectButton>().Initialize((Progress.StoryProgress)i, chapterManager.GetTitle((Progress.StoryProgress)i), this);
            }

        }


        public void ToStory(Progress.StoryProgress chapter)
        {
            Debug.Log(chapter.ToString());
            progress.ThisStoryProgress = chapter;
            progress.MyQuestProgress = Progress.QuestProgress.Prologue;
            SceneChanger.SceneChange(SceneName.Story);
        }

    }
}
