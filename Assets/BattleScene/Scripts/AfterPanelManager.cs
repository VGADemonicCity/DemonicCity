using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class AfterPanelManager : MonoBehaviour
    {
        /// <summary>タップ,フリック,Raycast管理クラス</summary>
        TouchGestureDetector touchGestureDetector;
        /// <summary>パネルが処理中かどうか表すフラグ</summary>
        bool m_isPanelProcessing;

        void Start()
        {
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
             {

             });
        }


    }
}
