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
            //touchGestureDetector = GetComponent<TouchGestureDetector>();
        }
        void Start()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
            touchGestureDetector = GameObject.Find("DemonicCity.TouchGestureDetector").GetComponent<TouchGestureDetector>();

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
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


            });
            GameObject effectObj= ResourcesLoad.Load<GameObject>("Effects/TouchEffect");
            //GetComponent<ParticleSystem>();
            GameObject newObj = Instantiate(effectObj.gameObject, transform);
            effect = newObj.GetComponent<ParticleSystem>();
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