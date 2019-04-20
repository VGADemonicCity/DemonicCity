using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class Title : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector;
        SceneFader sceneFader;

        [SerializeField] SpriteRenderer touchText;
        [SerializeField] GameObject configBtn;
        [SerializeField] GameObject CreditIcon;
        [SerializeField] Transform parent;
        [SerializeField] GameObject windowObject;
        [SerializeField] GameObject returnObj;

        [SerializeField] Transform creditRect;

        WindowState Config { get; set; }
        bool isPop = false;

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

        private void Awake()
        {
            sceneFader = SceneFader.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }

        void Start()
        {
            StartCoroutine(FlashObject(touchText));

            //touchGestureDetector = GameObject.Find("DemonicCity.TouchGestureDetector").GetComponent<TouchGestureDetector>();

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    GameObject hitObj = null;
                    if (IsPopUp)
                    {
                        if (touchInfo.HitDetection(out hitObj, returnObj))
                        {
                            CreditChange(false);
                        }
                    }
                    else
                    {
                        if (touchInfo.HitDetection(out hitObj, configBtn))
                        {
                            ConfigOpen();
                        }
                        else if (touchInfo.HitDetection(out hitObj, CreditIcon))
                        {
                            CreditChange(true);
                        }
                        else if (!creditOpened)/*if (hitObj == null || hitObj.tag != "Button")*/
                        {
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
                progress.ThisStoryProgress = Progress.StoryProgress.Prologue;
                progress.ThisQuestProgress = Progress.QuestProgress.Prologue;
                sceneFader.FadeOut(SceneFader.SceneTitle.Story);
                return;
            }
            sceneFader.FadeOut(SceneFader.SceneTitle.Home);
        }

        void ConfigOpen()
        {
            Debug.Log("Open");
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
            isPop = true;
            StartCoroutine(ChangeScale(creditOpened));
        }

        IEnumerator FlashObject(SpriteRenderer targetRenderer)
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
