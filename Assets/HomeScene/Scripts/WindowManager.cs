using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace DemonicCity.HomeScene
{
    public enum Window
    {
        Growth, Story, Summon, Magia, Config, PresentBox, Last
    }
    public class WindowManager : MonoBehaviour
    {
        public TouchGestureDetector touchGestureDetector;
        
        //Color windowColor = new Color(1, 1, 1, 0.9f);
        [SerializeField] GameObject[] parents = new GameObject[(int)Window.Last];
        [SerializeField] GameObject[] windowObjects = new GameObject[(int)Window.Last];
        [SerializeField] GameObject[] buttonObjects = new GameObject[(int)Window.Last];
        GameObject beginObject;
        GameObject endObject;
        GameObject newPanel;
        //GameObject nowWindow;
        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
        }
        // Use this for initialization
        void Start()
        {
            //Debug.Log("Start");

            //touchGestureDetector = GetComponent<TouchGestureDetector>();
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {

                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    touchInfo.HitDetection(out beginObject,gameObject);

                    if (newPanel)
                    {
                        Destroy(newPanel);
                    }
                }
                if (gesture == TouchGestureDetector.Gesture.Click)
                {

                    //Debug.Log("click");
                    for (int i = (int)Window.Growth; i < (int)Window.Last; i++)
                    {
                        if (touchInfo.HitDetection(out endObject, buttonObjects[i])
                        && buttonObjects[i])
                        {
                            if (beginObject == endObject)
                            {
                                parents[i].SetActive(true);
                                switch ((Window)i)
                                {
                                    case Window.Growth:
                                        SceneChanger.SceneChange(SceneName.Strengthen);
                                        break;
                                    case Window.Story:
                                        SceneChanger.SceneChange(SceneName.StorySelect);
                                        break;
                                    case Window.Summon:
                                        if (true)//一部クリアフラグ
                                        {
                                            SceneChanger.SceneChange(SceneName.Home);
                                        }
                                        break;
                                    case Window.Config:
                                    case Window.PresentBox:
                                    case Window.Magia:
                                        WindowOpen(i);
                                        break;
                                    default:
                                        Debug.Log("Error");
                                        break;
                                }

                            }
                        }
                    }
                }


            });

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void WindowOpen(int i)
        {
            newPanel = Instantiate(windowObjects[i], parents[i].transform);
            newPanel.GetComponent<WindowState>().touchGestureDetector = touchGestureDetector;
            if (i != (int)Window.Magia)
            {
                newPanel = null;
            }
        }


    }
}