using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class TouchEffecter : MonoBehaviour
    {


        ParticleSystem effect;
        TouchGestureDetector touchGestureDetector;
        //UnityEngine.ParticleSystem.MainModule psMain;

        // Use this for initialization
        void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
        }
        void Start()
        {
            SoundManager soundM = SoundManager.Instance;
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    TouchCheck();
                }
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                    case TouchGestureDetector.Gesture.TouchMove:
                    case TouchGestureDetector.Gesture.TouchStationary:

                        Vector3 pos = TouchPosition.TouchToCanvas();
                        transform.localPosition = pos;
                        effect.Emit(1);
                        break;
                }
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    GameObject hit;
                    if (touchInfo.HitDetection(out hit))
                    {
                        if (!hit.GetComponent<NegativeTouched>())
                        {
                            soundM.PlayWithFade(SoundAsset.SETag.PositiveButton);
                        }
                    }
                }
            });
            GameObject effectObj = ResourcesLoad.Load<GameObject>("Effects/TouchEffect");
            //GetComponent<ParticleSystem>();
            GameObject newObj = Instantiate(effectObj.gameObject, transform);
            effect = newObj.GetComponent<ParticleSystem>();
        }

        void TouchCheck()
        {

        }

        // Update is called once per frame
        void Update()
        {



            if (Input.touchCount > 0)
            {
                Vector3 pos = TouchPosition.TouchToCanvas();

                transform.localPosition = pos;

                //Touch touch = Input.GetTouch(0);
                //if (touch.phase == TouchPhase.Began
                //    || touch.phase == TouchPhase.Moved)
                //{
                //    effect.Emit(1);
                //}

            }


        }

    }
}