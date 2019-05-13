using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.HomeScene
{
    public class TutoralClose : MonoBehaviour
    {
        bool isFirst = true;
        Progress progress;
        TouchGestureDetector tgd;
        [SerializeField] GameObject obj;
        // Use this for initialization
        void Awake()
        {


        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Close(GameObject _obj)
        {
            obj = _obj;
            Debug.Log("jfkgod;sghu");
            progress = Progress.Instance;
            tgd = TouchGestureDetector.Instance;
            tgd.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (isFirst)
                {
                    Destroy(obj);
                    isFirst = false;
                }
            });
        }
    }
}