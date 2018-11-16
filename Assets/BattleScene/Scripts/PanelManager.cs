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
        [HideInInspector] public TouchGestureDetector m_touchGestureDetector;

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
                if(touchInfo.IsHit(m_panel, Camera.main)) //Panelをタッチしたら
                {
                    Debug.Log("これはm_panelと認識されたよ");
                }
               
                if(!touchInfo.IsHit(m_panel, Camera.main))
                {
                    Debug.Log("これはm_panelと認識されなかったよ");
                }
            });
        }
    }
}
