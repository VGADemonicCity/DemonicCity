using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class touchTest : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector;
        [SerializeField] GameObject obj;
        private void Awake()
        {
            touchGestureDetector = TouchGestureDetector.Instance;
        }

        private void Start()
        {
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {

                    GameObject hitResultGameObject;
                    var HitResult = touchInfo.HitDetection(out hitResultGameObject);
                        Destroy(Instantiate(obj, touchInfo.LastPosition, Quaternion.identity), 3f);
                    if (HitResult)
                    {
                        Debug.Log(hitResultGameObject.name);
                    }
                }
            });
        }
    }
}

