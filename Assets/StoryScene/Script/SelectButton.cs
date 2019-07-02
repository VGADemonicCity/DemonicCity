using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.StorySelectScene
{
    public class SelectButton : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector;
        ToStories toStories;
        [SerializeField] TMPro.TMP_Text text = null;
        [SerializeField] Text level = null;
        GameObject beginObject;
        GameObject endObject;

        string chapterName;
        Progress.StoryProgress chapterTag;

        //Progress progress;


        public void Initialize(Progress.StoryProgress chapter)
        {
            chapterTag = chapter;
            chapterName = chapterTag.ToString();
        }
        public void Initialize(string title, StorySelecter selecter)
        {
            chapterName = title;
        }
        public void Initialize(Progress.StoryProgress chapter, string title)
        {
            chapterTag = chapter;
            chapterName = title;
        }
        public void Initialize(Chapter chapter, bool isActive)
        {
            chapterTag = chapter.storyProgress;
            chapterName = chapter.chapterTitle;
            if (chapter.levelRange[0] == 0 && chapter.levelRange[1] == 0)
            {
                level.text = "";
            }
            else
            {
                level.text = "推奨Lv. " + chapter.levelRange[0] + "～" + chapter.levelRange[1];
            }
            //if (isActive) Debug.Log(chapterName);
            gameObject.SetActive(isActive);


            text.text = chapterName;
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        touchInfo.HitDetection(out beginObject, gameObject);
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

        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
            //progress = Progress.Instance;
            toStories = new ToStories();
        }

        void Start()
        {

        }

    }
}
