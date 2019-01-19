using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StorySelectScene
{
    public class SelectButton : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector;
        StorySelecter storySelecter;
        ToStories toStories;
        [SerializeField] TMPro.TMP_Text text = null;

        GameObject beginObject;
        GameObject endObject;

        string chapterName;
        Progress.StoryProgress chapterTag;

        Progress progress;


        public void Initialize(Progress.StoryProgress chapter, StorySelecter selecter)
        {
            storySelecter = selecter;
            chapterTag = chapter;
            chapterName = chapterTag.ToString();
        }
        public void Initialize(string title, StorySelecter selecter)
        {
            storySelecter = selecter;
            chapterName = title;
        }
        public void Initialize(Progress.StoryProgress chapter, string title, StorySelecter selecter)
        {
            chapterTag = chapter;
            storySelecter = selecter;
            chapterName = title;
        }

        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
            progress = Progress.Instance;
            toStories = new ToStories();
        }

        void Start()
        {
            text.text = chapterName;
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        touchInfo.HitDetection(out beginObject);
                        break;

                    case TouchGestureDetector.Gesture.Click:
                        if (touchInfo.HitDetection(out endObject, gameObject))
                        {
                            if (beginObject == endObject)
                            {
                                toStories.ToStory(chapterTag);
                            }


                        }
                        break;

                    default:
                        break;
                }
                //Debug.Log(beginObject+"" +endObject);
            });
        }


    }
}
