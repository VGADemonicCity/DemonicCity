using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity.HomeScene
{
    public class WindowManager : MonoBehaviour
    {
        public TouchGestureDetector touchGestureDetector;
        
        public enum Window
        {
            Growth, Config, Calender, PresentBox, Last
        }

        public GameObject[] parents;
        public GameObject[] windowObjects;
        public GameObject[] buttonObjects;

        // Use this for initialization
        void Start()
        {
            Debug.Log("Start");
            
            
            //touchGestureDetector = GetComponent<TouchGestureDetector>();
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                Debug.Log("sss");
                if (true)
                {
                    
                    for (int i = (int)Window.Growth; i < (int)Window.Last; i++)
                    {
                        if (/*touchInfo.m_hitResult==buttonObjects[i]*/true)
                        {
                            Debug.Log("aa");
                            WindowOpen(i);
                        }
                    }
                }


            });
            //WindowOpen(0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void WindowOpen(int i)
        {
            GameObject newPanel = Instantiate(windowObjects[i], parents[i].transform);
            newPanel.GetComponent<WindowState>().touchGestureDetector = touchGestureDetector;
        }

    }
}