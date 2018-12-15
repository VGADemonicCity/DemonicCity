using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StorySelectScene
{
    public class StorySelecter : MonoBehaviour
    {
        Progress progress;
        Progress.StoryProgress MyStory;

        [SerializeField] RectTransform parent;
        [SerializeField]GameObject SelectButton;

        void Awake()
        {
            progress = Progress.Instance;
        }
        void Start()
        {
            MyStory = progress.MyStoryProgress;

            //for (Progress.StoryProgress i = Progress.StoryProgress.Prologue; i <= MyStory + 1; i++ )
            //{
            //    GameObject newSelectButton = Instantiate(SelectButton,parent);
            //    newSelectButton.GetComponent<SelectButton>().Initialize(i);
            //}
            for (int i = (int)MyStory ; i >= 1; i--
                /*int i=1;i<=(int)MyStory;i+=i*/)
            {
                GameObject newSelectButton = Instantiate(SelectButton, parent);
                newSelectButton.GetComponent<SelectButton>().Initialize((Progress.StoryProgress)i,this);
            }

        }


        public void ToStory(Progress.StoryProgress chapter)
        {
            Debug.Log(chapter.ToString());
            SceneChanger.SceneChange(SceneName.Story);
        }

    }
}
