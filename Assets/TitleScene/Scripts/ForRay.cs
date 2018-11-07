using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class ForRay : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }
        GameObject beganObj = null;
        GameObject hitObj = null;
        // Update is called once per frame
        void Update()
        {

            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase != TouchPhase.Canceled
                    || Input.GetTouch(0).phase != TouchPhase.Stationary
                    || true)
                {


                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        //Debug.Log(TouchPosition.TouchToCanvas());
                        //beganObj = RaycastDetection.DetectHitGameObject(TouchPosition.TouchToRay());
                        beganObj = HitRay();

                        Debug.Log(beganObj.name);
                    }
                    //else if (beganObj == RaycastDetection.DetectHitGameObject(TouchPosition.TouchToRay()))
                    else if (beganObj == HitRay())
                    {
                        hitObj = beganObj;
                    }
                    else
                    {
                        hitObj = null;
                    }
                    Judge.JudgeObj(hitObj);
                }
            }

        }
        public GameObject HitRay()
        {

            RaycastHit2D hit = Physics2D.Raycast(TouchPosition.TouchToRay(),Vector2.zero );
            //Debug.DrawRay(TouchPosition.TouchToRay() ,Vector2.zero);
            
            return hit.collider.gameObject;
        }
    }

}
