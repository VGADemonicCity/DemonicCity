using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace DemonicCity.HomeScene
{
    public enum Window
    {
        Growth, Story, Summon, Magia, Config, Gallery, Last
    }
    public class WindowManager : MonoBehaviour
    {
        SoundManager soundM;
        public TouchGestureDetector touchGestureDetector;
        SceneFader sceneFader;
        //Color windowColor = new Color(1, 1, 1, 0.9f);
        [SerializeField] GameObject[] parents = new GameObject[(int)Window.Last];
        [SerializeField] GameObject[] windowObjects = new GameObject[(int)Window.Last];
        GameObject[] windowInstance = new GameObject[(int)Window.Last];
        [SerializeField] GameObject[] buttonObjects = new GameObject[(int)Window.Last];
        GameObject beginObject;
        GameObject endObject;
        GameObject newPanel;
        //GameObject nowWindow;
        void Awake()
        {
            soundM = SoundManager.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            sceneFader = SceneFader.Instance;
        }
        // Use this for initialization
        Window touchedWindow = Window.Last;
        void Start()
        {
            //Debug.Log("Start");

            //touchGestureDetector = GetComponent<TouchGestureDetector>();
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {

                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    touchedWindow = Window.Last;
                    touchInfo.HitDetection(out beginObject);
                    for (int i = (int)Window.Growth; i < (int)Window.Last; i++)
                    {
                        if (touchInfo.HitDetection(out beginObject, buttonObjects[i]))
                        {
                            //重なってるときどうなるかわからない
                            touchedWindow = (Window)i;
                            break;
                        }
                    }
                }
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    //for (int i = (int)Window.Growth; i < (int)Window.Last; i++)
                    //{
                    if (touchedWindow != Window.Last
                       && touchInfo.HitDetection(out endObject, buttonObjects[(int)touchedWindow]))
                    {
                        //if (beginObject == endObject)
                        //{
                        parents[(int)touchedWindow].SetActive(true);
                        switch (touchedWindow)
                        {
                            case Window.Growth:
                                sceneFader.FadeOut(SceneFader.SceneTitle.Strengthen);
                                break;
                            case Window.Story:
                                sceneFader.FadeOut(SceneFader.SceneTitle.StorySelect);
                                break;
                            case Window.Summon:
                                if (true)//一部クリアフラグ
                                {
                                    sceneFader.FadeOut(SceneFader.SceneTitle.Home);
                                }
                                break;
                            case Window.Config:
                            case Window.Gallery:
                                WindowOpen((int)touchedWindow);
                                break;
                            case Window.Magia:
                                soundM.PlayWithFade(SoundManager.SoundTag.Voice, GetRandomVoice(Progress.Instance.IsClear));
                                break;
                            default:
                                Debug.Log("Error");
                                break;
                        }

                        //}
                    }
                    //}
                }


            });

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void WindowOpen(int i)
        {
            if (windowInstance[i])
            {
                windowInstance[i].SetActive(true);
                if (windowInstance[i].GetComponent<WindowState>())
                {
                    windowInstance[i].GetComponent<WindowState>().enabled = true;
                }
            }
            else
            {
                windowInstance[i] = Instantiate(windowObjects[i], parents[i].transform);
            }
            //newPanel.GetComponent<WindowState>().touchGestureDetector = touchGestureDetector;
            //if (i != (int)Window.Magia)
            //{
            //    newPanel = null;
            //}
        }


        [SerializeField] List<AudioClip> beforeVoicies;
        [SerializeField] List<AudioClip> afterVoicies;

        AudioClip GetRandomVoice(bool isClear)
        {
            List<AudioClip> tmp = new List<AudioClip>();
            tmp.AddRange(beforeVoicies);

            if (isClear)
            {
                tmp.AddRange(afterVoicies); ;
            }

            return tmp[Random.Range(0, tmp.Count)];
        }
    }
}