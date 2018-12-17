using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Debugger
{
    public class UIDetectionTest : MonoBehaviour
    {
        TouchGestureDetector m_touchGestureDetector;

        private void Awake()
        {
            m_touchGestureDetector = TouchGestureDetector.Instance;
        }

        private void Start()
        {
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    GameObject hitResult;
                    touchInfo.HitDetection(out hitResult);
                    if(hitResult != null)
                    {
                        Debug.Log(hitResult.name);
                    }
                    else
                    {
                        Debug.Log("ぬるだお");
                    }
                }
            });
        }
    }
}