using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.HomeScene
{
    public class Title : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector;
        SceneFader sceneFader;

        [SerializeField] Image touchText;
        [SerializeField] GameObject configBtn;
        [SerializeField] GameObject CreditIcon;
        [SerializeField] Transform parent;
        [SerializeField] GameObject windowObject;
        [SerializeField] GameObject returnObj;

        [SerializeField] Transform creditRect;

        WindowState Config { get; set; }
        bool isPop = false;
        bool isInited = false;
        bool isTrasing = false;

        bool IsPopUp
        {
            get
            {


                return isPop && (Config != null && Config.windowEnabled) || creditOpened;
            }
            set
            {
                isPop = value;
            }
        }

        void Awake()
        {
        }

        void Start()
        {
        }

        void Update()
        {
            if (!isInited)
            {
                StartCoroutine(WaitInit());
                isInited = true;
            }
        }

        [SerializeField] float waitTime = 2f;
        IEnumerator WaitInit()
        {
            yield return new WaitForSecondsRealtime(waitTime);

            Initialize();
        }

        void Initialize()
        {
            sceneFader = SceneFader.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;

            //Debug用にコメントアウト
            StartCoroutine(FlashObject(touchText));

            //touchGestureDetector = GameObject.Find("DemonicCity.TouchGestureDetector").GetComponent<TouchGestureDetector>();

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    if (isTrasing) return;
                    GameObject hitObj = null;
                    touchInfo.HitDetection(out hitObj);
                    Debug.Log(hitObj.name);

                    Debug.Log("Hit");
                    if (IsPopUp)
                    {
                        //if (touchInfo.HitDetection(out hitObj, returnObj))
                        if (returnObj.name == hitObj.name)
                        {
                            CreditChange(false);
                        }
                    }
                    else
                    {
                        //if (touchInfo.HitDetection(out hitObj, configBtn))
                        if (hitObj.name == configBtn.name)
                        {
                            ConfigOpen();
                        }
                        else if (hitObj == CreditIcon)
                        {
                            CreditChange(true);
                        }
                        else if (!creditOpened)/*if (hitObj == null || hitObj.tag != "Button")*/
                        {
                            isTrasing = true;
                            SceneTrans();
                        }
                    }

                }
            });
        }


        public void SceneTrans()
        {
            Progress progress = Progress.Instance;
            if (progress.MyStoryProgress == 0)
            {
                new StorySelectScene.ToStories().ToStory(Progress.StoryProgress.Prologue);
                //progress.ThisStoryProgress = Progress.StoryProgress.Prologue;
                //progress.ThisQuestProgress = Progress.QuestProgress.Prologue;
                //sceneFader.FadeOut(SceneFader.SceneTitle.Story);
                return;
            }
            sceneFader.FadeOut(SceneFader.SceneTitle.Home);
        }

        void ConfigOpen()
        {
            //Debug.Log("Open");
            IsPopUp = true;
            parent.gameObject.SetActive(true);
            if (Config)
            {
                DestroyImmediate(Config.gameObject);
                Config = null;
            }
            Config = Instantiate(windowObject, parent).GetComponent<WindowState>();
            Config.gameObject.SetActive(true);
        }

        public void CreditChange(bool isOpen)
        {
            creditOpened = isOpen;
            isPop = isOpen;

            StartCoroutine(ChangeScale(creditOpened));

        }

        IEnumerator FlashObject(Image targetRenderer)
        {
            Color origin = targetRenderer.color;
            //1フレーム辺りのangleの増加量
            int add = 3;

            float n = 0.5f;

            List<float> alphas = new List<float>();
            int angle = 0;
            float alpha = 0;

            while (angle <= 360)
            {
                alpha = Mathf.Sin(angle * Mathf.PI / 180) + n;

                targetRenderer.color = new Color(origin.r, origin.g, origin.b, alpha);
                alphas.Add(alpha);
                angle += add;

                yield return null;
            }

            int i = 0;
            while (true)
            {
                if (i < alphas.Count)
                {

                    targetRenderer.color = new Color(origin.r, origin.g, origin.b, alphas[i]);

                    i += 1;
                }
                else
                {
                    i = 0;
                }

                yield return null;
            }
        }

        bool creditOpened = false;
        IEnumerator ChangeScale(bool isOpen)
        {
            Debug.Log(isOpen);
            Vector3 targetScale = Vector3.one;
            if (isOpen)
            {
                creditRect.localScale = new Vector3(1, 0, 1);
                creditRect.gameObject.SetActive(true);
            }
            else
            {
                creditRect.localScale = Vector3.zero;
                creditRect.gameObject.SetActive(false);


                yield break;
                //enabled = false;
            }
            while (creditRect.localScale != targetScale)
            {
                creditRect.localScale = Vector3.Lerp(creditRect.localScale, targetScale, 0.02f * 10f);


                yield return null;
            }

        }

    }
}
