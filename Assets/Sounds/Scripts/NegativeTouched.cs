using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class NegativeTouched : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                Debug.Log("aaa");
                SoundManager.Instance.PlayWithFade(SoundAsset.SETag.NegativeButton);
            });
            //TouchGestureDetector.Instance.onGestureDetected.AddListener((gesture, touchInfo) =>
            //{
            //    if (gesture == TouchGestureDetector.Gesture.Click)
            //    {
            //        GameObject hit;
            //        if (touchInfo.HitDetection(out hit, gameObject))
            //        {
            //            if (hit.name == gameObject.name)
            //            {
            //                Debug.Log("NegaButton");
            //                SoundManager.Instance.PlayWithFade(SoundAsset.SETag.PositiveButton);
            //            }
            //        }
            //    }
            //});
        }
    }
}
