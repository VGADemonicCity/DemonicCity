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
        /// <summary>TouchGestureDetectorの参照</summary>
        TouchGestureDetector m_touchGestureDetector;
        /// <summary>同オブジェクトにアタッチされている[パネルを生成して同時にパネル種類の振り分けもしてくれるクラス]の参照</summary>
        InstantiatePanels m_instantiatePanels;
        /// <summary>パネルが処理中かどうか表すフラグ</summary>
        bool m_isPanelProcessing;


        void Awake()
        {
            // shingleton,TouchGestureDetectorインスタンスの取得
            m_touchGestureDetector = TouchGestureDetector.Instance;


            // 同オブジェクトにアタッチされているコンポーネントの取得
            m_instantiatePanels = GetComponent<InstantiatePanels>();
        }

        void Start()
        {
            m_instantiatePanels.GeneratePanels(); // パネル生成処理
            // タッチによる任意の処理をイベントに登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
            if (m_isPanelProcessing) //パネルが処理中なら処理終了
            {
                return;
            }
            switch (gesture)
            {
                case TouchGestureDetector.Gesture.Click: // クリックジェスチャをした時

                    GameObject hitResult;
                    touchInfo.HitDetection(out hitResult);
                        if (hitResult != null) Debug.Log(hitResult.name);
                        if (hitResult != null && hitResult.tag == "Panel") // タッチしたオブジェクトのタグがパネルなら
                        {
                            Debug.Log("ぱねる！");
                            //var panel = touchInfo.m_hitResult.GetComponent<Panel>(); // タッチされたパネルのPanelクラスの参照
                            //panel.Open(); // panelを開く
                        }
                        break;
                }
            });
        }
    }
}
