using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity
{
    using Story = Progress.StoryProgress;

    public class ProgressPanel : MonoBehaviour
    {
        ChapterManager chapterM;
        Progress progress;
        ProgressToggle toggle;
        [SerializeField] GameObject toggleObj;
        [SerializeField] Transform parent;
        void Awake()
        {
            progress = Progress.Instance;
            chapterM = ChapterManager.Instance;
        }

        // Use this for initialization
        void Start()
        {
            ToggleGenerate();
        }

        // Update is called once per frame
        void Update()
        {

        }
        Story currentStory = Story.Phoenix;
        void ToggleGenerate()
        {
            Story tmpstory;
            currentStory = progress.MyStoryProgress;
            Debug.Log(progress.MyStoryProgress);

            for (int i = (int)Story.Prologue; i < (int)Story.All; i = i << 1)
            {
                tmpstory = (Story)i;
                toggle = Instantiate(toggleObj, parent).GetComponent<ProgressToggle>();
                toggle.Init(this, tmpstory, (tmpstory & currentStory) == tmpstory, chapterM.GetChapter(tmpstory).chapterTitle);
            }
        }

        public void ToggleChanged(Story story, bool isActive)
        {
            if (isActive)
            {
                currentStory = story | currentStory;
            }
            else
            {
                currentStory = ~story & currentStory;
            }
            Debug.Log(currentStory);
        }

        public void Done()
        {
            progress.MyStoryProgress = currentStory;
            ExitPanel();
        }

        public void ExitPanel()
        {
            Debug.Log(progress.MyStoryProgress);
            DestroyImmediate(gameObject);
        }

    }
}