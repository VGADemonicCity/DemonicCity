using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StrengthenScene
{
    public class Test : MonoBehaviour
    {
        TouchGestureDetector m_touchGestureDetector = TouchGestureDetector.Instance;
        /// <summary>
        /// aaa
        /// </summary>
        public string Prop { get; set;}


        void Start()
        {
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        GameObject go;
                        touchInfo.HitDetection(out go);
                        break;
                    case TouchGestureDetector.Gesture.TouchMove:
                        break;
                    case TouchGestureDetector.Gesture.TouchStationary:
                        break;
                    case TouchGestureDetector.Gesture.TouchEnd:
                       

                        break;
                }
            });
        }

        void Update()
        {
        }
    }
}
