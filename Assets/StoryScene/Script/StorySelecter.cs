using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StorySelectScene
{
    public class StorySelecter : MonoBehaviour
    {
        Progress progress;
        Progress.StoryProgress MyStory;

        void Awake()
        {
            progress = Progress.Instance;
        }
        void Start()
        {
            MyStory = progress.MyStoryProgress;
            foreach (var item in a )
            {
                
            }
        }

    }
}
