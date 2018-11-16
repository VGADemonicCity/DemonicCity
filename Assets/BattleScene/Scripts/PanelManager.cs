using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Panel manager.
    /// </summary>
    public class PanelManager : MonoBehaviour
    {
        /// <summary>タップ,フリック,Raycast管理クラス</summary>
        private TouchGestureDetector m_touchGestureDetector;

        /// <summary>同オブジェクトにアタッチされている[パネルを生成して同時にパネル種類の振り分けもしてくれるクラス]の参照</summary>
        InstantiatePanels m_instantiatePanels;
        /// <summary>パネルが処理中かどうか表すフラグ</summary>
        bool m_isPanelProcessing;
        /// <summary>パネルプレハブ</summary>
        GameObject m_panel;

        void Awake()
        {
            // 同オブジェクトにアタッチされているコンポーネントの取得
            m_touchGestureDetector = GetComponent<TouchGestureDetector>();
            m_instantiatePanels = GetComponent<InstantiatePanels>();
        }

        void Start()
        {
            m_instantiatePanels.GeneratePanels(); // パネル生成処理
            m_panel = Resources.Load<GameObject>("Battle_Panel"); // Battle_PanelをResourcesフォルダに入れてシーン外から取得

            // タッチによる任意の処理をイベントに登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {

                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.FlickLeftToRight:
                        Debug.Log("右フリックで呼ばれたよ");
                        break;
                    case TouchGestureDetector.Gesture.FlickRightToLeft:
                        Debug.Log("左フリックで呼ばれたよ");
                        break;
                    case TouchGestureDetector.Gesture.Click:
                        var go = touchInfo.m_hitResult;
                        if (go != null)
                        {
                            Debug.Log(go.tag);
                        }
                        else
                        {
                            Debug.Log("nullだお");
                        }
                        break;

                    default:
                        break;
                }
            
            });
        }
    }
}
