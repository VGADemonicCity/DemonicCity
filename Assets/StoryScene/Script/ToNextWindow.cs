using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    public class ToNextWindow : MonoBehaviour
    {
        Progress progress;
        StorySelectScene.ToStories toStories;
        SceneFader fader;

        public void ToNextChapter()
        {
            progress = Progress.Instance;
            toStories = new StorySelectScene.ToStories();
            toStories.ToStory(progress.NextStory(progress.ThisStoryProgress));
        }
        public void ToHome()
        {
            fader = SceneFader.Instance;
            fader.FadeOut(SceneFader.SceneTitle.Title);
        }
    }
}