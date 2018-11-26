using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace DemonicCity.HomeScene
{
    public class WindowManager : MonoBehaviour,IPointerClickHandler
    {
        public TouchGestureDetector touchGestureDetector;

        public enum Window
        {
            Growth, Story, Magia, Config, Calender, PresentBox, Last
        }

        public GameObject[] parents;
        public GameObject[] windowObjects;
        public GameObject[] buttonObjects;
        GameObject outObject;

        // Use this for initialization
        void Start()
        {
            //Debug.Log("Start");
            touchGestureDetector = TouchGestureDetector.Instance;
            touchGestureDetector = GameObject.Find("DemonicCity.TouchGestureDetector").GetComponent<TouchGestureDetector>();


            //touchGestureDetector = GetComponent<TouchGestureDetector>();
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {

                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    Debug.Log("click");
                    for (int i = (int)Window.Growth; i < (int)Window.Last; i++)
                    {
                        if (touchInfo.HitDetection(out outObject, buttonObjects[i])
                        && buttonObjects[i])
                        {
                            if (windowObjects[i])
                            {
                                WindowOpen(i);
                            }
                            else if (i == (int)Window.Story)
                            {
                                SceneChange.SceneChanger(SceneName.Battle);
                            }
                        }
                        //Debug.Log(touchInfo.HitDetection(out outObject, buttonObjects[i]));
                        //touchInfo.HitDetection(out outObject, buttonObjects[i]);
                        //Debug.Log(outObject.name);
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

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }
    }
}