using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace DemonicCity.HomeScene
{
    public enum Window
    {
        Growth, Story, Magia, SoundConfig, Config, Calender, PresentBox, Last
    }
    public class WindowManager : MonoBehaviour
    {
        public TouchGestureDetector touchGestureDetector;


        //Color windowColor = new Color(1, 1, 1, 0.9f);
        public GameObject[] parents;
        public GameObject[] windowObjects;
        public GameObject[] buttonObjects;
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
                    touchInfo.HitDetection(out beginObject);

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
                                if (i == (int)Window.Growth)
                                {
                                    SceneChanger.SceneChange(SceneName.Strengthen);
                                }
                                if (i == (int)Window.Story)
                                {
                                    SceneChanger.SceneChange(SceneName.StorySelect);
                                }
                                else if (windowObjects[i])
                                {
                                    WindowOpen(i);
                                    if (i != (int)Window.Magia)
                                    {
                                        newPanel = null;
                                    }
                                }
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
            newPanel = Instantiate(windowObjects[i], parents[i].transform);
            newPanel.GetComponent<WindowState>().touchGestureDetector = touchGestureDetector;
            //newPanel.GetComponent<WindowState>().closeAnimation = WindowScaling;
            //nowWindow = newPanel;
            //WindowScaling(true);
        }

        //public void OnPointerClick(PointerEventData eventData)
        //{

        //}
        //public void WindowScaling(bool isOpen)
        //{
        //    if (isOpen)
        //    {
        //        StartCoroutine(ChangeScale(Vector3.one));
        //        StartCoroutine(ChangeColor(windowColor));
        //    }
        //    else
        //    {
        //        StartCoroutine(ChangeScale(Vector3.zero));
        //        StartCoroutine(ChangeColor(Color.clear));
        //    }
        //}

        //IEnumerator ChangeScale(Vector3 targetScale)
        //{
        //    while (nowWindow.transform.localScale != targetScale)
        //    {
        //        nowWindow.transform.localScale = Vector3.Lerp(nowWindow.transform.localScale, targetScale, 0.02f * 10f);


        //        yield return new WaitForSecondsRealtime(0.01f);
        //    }

        //}
        //IEnumerator ChangeColor(Color targetColor)
        //{
        //    while (nowWindow.GetComponent<SpriteRenderer>().color != targetColor)
        //    {
        //        nowWindow.GetComponent<SpriteRenderer>().color = Color.Lerp(nowWindow.GetComponent<SpriteRenderer>().color, targetColor, 0.02f * 10f);


        //        yield return new WaitForSecondsRealtime(0.01f);
        //    }
        //}

    }
}