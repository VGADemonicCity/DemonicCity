using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.HomeScene
{
    public class WindowState : MonoBehaviour
    {
        [SerializeField] GameObject exitButton = null;
        public TouchGestureDetector touchGestureDetector;
        //string key = "IsOpen";
        //public delegate void CloseAnimation(bool isOpen);
        //public CloseAnimation closeAnimation;
        Color windowColor = new Color(1, 1, 1, 0.9f);
        // Use this for initialization
        bool windowEnabled;
        RectTransform windowRect;
        IEnumerator[] WindowAnimation = new IEnumerator[2];

        GameObject beginObject;
        GameObject endObject;
        void Awake()
        {
            windowRect = GetComponent<RectTransform>();
            windowRect.localScale = Vector3.zero;
        }
        void Start()
        {
            //touchGestureDetector= GetComponent<TouchGestureDetector>();
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture==TouchGestureDetector.Gesture.TouchBegin)
                {
                    touchInfo.HitDetection(out beginObject);
                }
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    
                    if (touchInfo.HitDetection(out endObject, exitButton)
                    || exitButton == null)
                    {
                        if (beginObject==endObject
                        &&windowEnabled)
                        {
                            WindowScaling(false);

                        }
                        //ChangeState(key, false);
                    }
                    //Debug.Log(outObject.name);
                }

            });
            //ChangeState(key, true);
            WindowScaling(true);
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (true)
            //    {

            //    }
            //    ChangeState("IsOpen");
            //}
        }

        public void ChangeState(string s)
        {
            GetComponent<Animator>().SetBool(s, !GetComponent<Animator>().GetBool(s));
        }
        public void ChangeState(string s, bool state)
        {
            GetComponent<Animator>().SetBool(s, state);
        }
        public void OnDisable()
        {
            transform.parent.gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public void WindowScaling(bool isOpen)
        {
            if (isOpen)
            {
                WindowAnimation[0] = ChangeScale(Vector3.one);
                WindowAnimation[1] = ChangeColor(windowColor);
                windowEnabled = true;
            }
            else
            {
                WindowAnimation[0] = ChangeScale(Vector3.zero);
                WindowAnimation[1] = ChangeColor(Color.clear);
                windowEnabled = false;
            }
            StartCoroutine(WindowAnimation[0]);
            StartCoroutine(WindowAnimation[1]);
        }

        IEnumerator ChangeScale(Vector3 targetScale)
        {
            while (windowRect.localScale != targetScale)
            {
                windowRect.localScale = Vector3.Lerp(windowRect.localScale, targetScale, 0.02f * 10f);


                yield return new WaitForSecondsRealtime(0.01f);
            }
            if (!windowEnabled)
            {
                this.enabled = false;

            }
        }
        IEnumerator ChangeColor(Color targetColor)
        {
            while (GetComponent<SpriteRenderer>().color != targetColor)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, targetColor, 0.02f * 10f);


                yield return new WaitForSecondsRealtime(0.005f);
            }
            if (!windowEnabled)
            {
                this.enabled = false;

            }
        }
    }
}