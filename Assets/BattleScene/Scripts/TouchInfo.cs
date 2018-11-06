using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class TouchInfo : MonoBehaviour
    {
        private Touch m_touch;
        private RaycastDetection m_raycastDetection;

        private void Start()
        {
            m_raycastDetection = GetComponent<RaycastDetection>();
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                m_touch = Input.GetTouch(0);
            }
        }

        public GameObject GetTouchedGameObject(TouchPhase touchPhase)
        {
            GameObject gameObject;
            switch (touchPhase)
            {
                case TouchPhase.Began:
                case TouchPhase.Ended:
                    gameObject = m_raycastDetection.DetectHitGameObject(m_touch.position);
                    if(gameObject != null)
                    {
                        return gameObject;
                    }
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Stationary:
                    break;

                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
