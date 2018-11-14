using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class TouchManager : MonoBehaviour
    {

        
        ParticleSystem effect;

        //UnityEngine.ParticleSystem.MainModule psMain;

        // Use this for initialization
        void Start()
        {
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

                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began
                    || touch.phase == TouchPhase.Moved)
                {
                    effect.Emit(1);
                }
                
            }
            

        }

    }
}