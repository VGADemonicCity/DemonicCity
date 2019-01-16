using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class Title : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector;
        SceneFader sceneFader;
        private void Awake()
        {
            sceneFader = SceneFader.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }

        void Start()
        {
            //touchGestureDetector = GameObject.Find("DemonicCity.TouchGestureDetector").GetComponent<TouchGestureDetector>();

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                Debug.Log(gesture);
                    ToHome();
                }
            });
        }
        public void ToHome()
        {
            sceneFader.FadeOut(SceneFader.SceneTitle.Home);
        }
    }
}
