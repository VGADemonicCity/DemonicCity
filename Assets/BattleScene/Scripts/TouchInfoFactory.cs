using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>タッチ情報を元にTouch.phaseの値に合わせて処理を呼び出す</summary>
    public class TouchInfoFactory : MonoBehaviour
    {
        /// <summary>TouchPhase.Began時の処理</summary>
        private TouchPhaseBeganProcess m_touchPhaseBeganProc;

        /// <summary>タッチ判別処理</summary>
        /// <param name="touch">Input.GetTouchのタッチ情報</param>
        public void OperationPerTouch(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    m_touchPhaseBeganProc = GetComponent<TouchPhaseBeganProcess>();
                    m_touchPhaseBeganProc.Rotation();
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                default:
                    break;
            }
        }
    }
}
