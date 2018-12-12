using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StrengthenScene
{
    public class AllocationStatusPoint : MonoBehaviour
    {
        Magia referenceMagia;
        Magia editMagia;

        //Statistics result = new Statistics();
        TouchGestureDetector m_touchGestureDetector;

        void Awake()
        {
            referenceMagia = Magia.Instance;
            editMagia = new Magia();
            m_touchGestureDetector = TouchGestureDetector.Instance;
        }

        void Start()
        {
            referenceMagia.GetStats();
            Debug.Log(referenceMagia.GetStats());

            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        //PressedConfirm();
                        break;
                }
            });
        }

        /// <summary>
        /// 確定ボタンを押すと反映された割り振りポイントを確定
        /// </summary>
        public void PressedConfirm()
        {
            Debug.Log("aaaa");
        }

        /// <summary>
        /// 中止ボタンを押すと反映されたポイントを初期化
        /// </summary>
        public void PressedReset()
        {
            Debug.Log("bbbb");
        }
    }
}
