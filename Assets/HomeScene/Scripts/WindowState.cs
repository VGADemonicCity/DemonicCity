using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.HomeScene
{
    public class WindowState : MonoBehaviour
    {
        [SerializeField] GameObject exitButton=null;
        public TouchGestureDetector touchGestureDetector;
        string key = "IsOpen";
        // Use this for initialization
        void Start()
        {
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (touchInfo.IsHit(exitButton))
                {
                    ChangeState(key, false);
                }
            });
            ChangeState(key, true);
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
    }
}