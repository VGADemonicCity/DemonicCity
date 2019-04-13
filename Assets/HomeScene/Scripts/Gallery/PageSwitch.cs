using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class PageSwitch : MonoBehaviour
    {
        public GameObject targetObj = null;

        float width = 1000f;


        //TouchGestureDetector tGD;

        private void Awake()
        {
            //tGD = TouchGestureDetector.Instance;
            if (targetObj == null)
            {
                targetObj = gameObject;
            }
        }
        // Use this for initialization
        void Start()
        {
            //tGD.onGestureDetected.AddListener((gesture, touchInfo) =>
            //{
            //    if (gesture == TouchGestureDetector.Gesture.FlickLeftToRight)
            //    {
            //        Scroll(true, 1f);
            //    }
            //    else if (gesture == TouchGestureDetector.Gesture.FlickRightToLeft)
            //    {
            //        Scroll(true, -1f);
            //    }
            //    else if (gesture == TouchGestureDetector.Gesture.Click)
            //    {
            //        Debug.Log(touchInfo.Diff);
            //        if (touchInfo.Diff.x < -width / 3)
            //        {
            //            Scroll(true, -1f);
            //        }
            //        else if (touchInfo.Diff.x > width / 3)
            //        {
            //            Scroll(true, 1f);
            //        }
            //        else
            //        {
            //            Scroll(false);
            //        }
            //    }
            //});
        }

        /// <summary></summary>
        /// <param name="isScroll"></param>
        /// <param name="sign">+で左-で右</param>
        public void Scroll(bool isScroll, float sign = 0f)
        {
            Debug.Log("Scroll:" + isScroll + " : " + sign);

            Vector3 oriPos = targetObj.transform.localPosition;
            float x = 0f;
            if (isScroll)
            {
                sign = Mathf.Sign(sign);
                x = width * sign;
            }
            targetObj.transform.localPosition = new Vector3(x, oriPos.y, oriPos.z);

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}