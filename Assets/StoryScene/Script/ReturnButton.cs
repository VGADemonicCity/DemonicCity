using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonicCity.StorySelectScene
{
    public class ReturnButton : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector;

        GameObject beginObject, endObject;
        SceneFader sceneFader;


        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
            sceneFader = SceneFader.Instance;
        }
        void Start()
        {
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        touchInfo.HitDetection(out beginObject);
                        break;

                    case TouchGestureDetector.Gesture.Click:
                        if (touchInfo.HitDetection(out endObject))
                        {
                            Debug.Log(beginObject.GetHashCode());
                            Debug.Log(gameObject.GetHashCode());
                            Debug.Log(endObject.GetHashCode());

                            if (beginObject == endObject
                            && beginObject == gameObject)
                            {
                                sceneFader.FadeOut(SceneFader.SceneTitle.Home);
                            }
                        }
                        break;

                    default:
                        break;
                }
            });
        }
    }
}