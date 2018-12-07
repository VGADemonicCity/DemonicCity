using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StrengthenScene
{
    public class TouchEffect : MonoBehaviour
    {
        TouchGestureDetector touchGestureDetector = TouchGestureDetector.Instance;

        [SerializeField]
        ParticleSystem touchEffect;
        [SerializeField]
        Camera camera;

        void Start()
        {
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:

                        /*GameObject go;
                        touchInfo.HitDetection(out go);
                        Debug.Log(go);
                        break;
                        */
                    case TouchGestureDetector.Gesture.TouchMove:
                    case TouchGestureDetector.Gesture.TouchStationary:
                    
                        //マウスのワールド座標までパーティクルを移動し、パーティクルエフェクトを1つ生成する
                        var pos = camera.ScreenToWorldPoint(Input.mousePosition + camera.transform.forward * 10);
                        touchEffect.transform.position = pos;
                        touchEffect.Emit(1);
                        Debug.Log(pos);
                        break;
                }
            });
        }

        void Update()
        {

        }
    }
}
