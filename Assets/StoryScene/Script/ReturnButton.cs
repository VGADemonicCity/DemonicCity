using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonicCity.StorySelectScene
{
    public class ReturnButton : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector;

        GameObject beginObject, endObject;



        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
        }
        void Start()
        {
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
                                SceneChanger.SceneChange(SceneName.Home);
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