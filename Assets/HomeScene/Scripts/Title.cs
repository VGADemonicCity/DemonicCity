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
        [SerializeField] Transform parent;
        [SerializeField] GameObject windowObject;
        WindowState config = null;
        bool isPop = false;

        bool IsPopUp
        {
            get
            {
                return isPop && (config != null & config.enabled);
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
                if (gesture == TouchGestureDetector.Gesture.Click
                && !IsPopUp)
                {
                    Debug.Log("Click");
                    GameObject hitObj = null;
                    if (touchInfo.HitDetection(out hitObj, configBtn))
                    {
                        ConfigOpen();
                    }
                    //Debug.Log(hitObj.name);
                    if (hitObj == null || hitObj.tag != "Button")
                    {
                        ToHome();
                    }
                    //Debug.Log(gesture);
                }
            });
        }
        public void ToHome()
        {
            sceneFader.FadeOut(SceneFader.SceneTitle.Home);
        }

        void ConfigOpen()
        {
            Debug.Log("Open");
            IsPopUp = true;
            parent.gameObject.SetActive(true);
            if (config)
            {
                Destroy(config.gameObject);
            }
            config = Instantiate(windowObject, parent).GetComponent<WindowState>();
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

    }
}
