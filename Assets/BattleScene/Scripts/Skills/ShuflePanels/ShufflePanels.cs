﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Shuffle panels.
    /// スキル : シャッフルパネル
    /// 引いたパネル枚数が30枚以上の時、画面に表示されている3*3のパネルを引いていない状態に戻してシャッフルする
    /// </summary>
    public class ShufflePanels : MonoBehaviour
    {
        BattleManager m_battleManager;

        /// <summary>
        /// パネルシャッフルスキルが発動可能かどうかの状態を表す
        /// </summary>
        public bool IsActivatable
        {
            get
            {
                if (m_panelCounter.CounterForShuffleSkill >= m_conditions)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int Conditions
        {
            get { return m_conditions; }
        }

        /// <summary>colliderを検出する為のcollider</summary>
        [SerializeField] BoxCollider2D m_sensor;
        /// <summary>Conditionsのバッキングフィールド</summary>
        [SerializeField] int m_conditions = 30;
        /// <summary>Positive button</summary>
        [SerializeField] Button PositiveButton;
        /// <summary>Negative button</summary>
        [SerializeField] Button NegativeButton;
        /// <summary>popup system</summary>
        [SerializeField] PopupSystem popupSystem;

        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;


        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_panelCounter = PanelCounter.Instance; // PannelCounterの参照取得
            m_battleManager = BattleManager.Instance;
        }
        private void Start()
        {
            GameObject hitResult;

            TouchGestureDetector.Instance.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.Click
                && m_battleManager.m_StateMachine.m_State == BattleManager.StateMachine.State.PlayerChoice
                && touchInfo.HitDetection(out hitResult)
                && m_panelCounter.CounterForShuffleSkill >= m_conditions)
                {
                    if (hitResult.tag != "ShufflePanels")
                    {
                        return;
                    }
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.Pause);
                    popupSystem.Popup();
                    popupSystem.SubscribeButton(new PopupSystemMaterial(PanelShuffle, PositiveButton.gameObject.name, true));
                    popupSystem.SubscribeButton(new PopupSystemMaterial(Cancel, NegativeButton.gameObject.name, true));
                }
            });
        }

        void Cancel()
        {
            m_battleManager.SetStateMachine(m_battleManager.m_StateMachine.m_PreviousState);
        }


        /// <summary>
        /// 現在画面に表示されている3*3のパネルを全て非表示の状態に戻して、その3*3のパネル内でまたシャッフルさせる
        /// </summary>
        void PanelShuffle()
        {
            m_sensor.enabled = true; // colliderをactiveにする
            var results = new Collider2D[9]; // 結果を受け取るための配列
            m_sensor.OverlapCollider(new ContactFilter2D(), results); // 設定したcolliderと重なっているcolliderを検出し配列に代入する
            var panelList = new List<Panel>(); // colliderに検出されたオブジェクトのPanelの参照リスト
            var panelTypes = new List<PanelType>(); // colliderに検出されたパネルのPanelTypeをリストで格納

            foreach (var panel in results) // colliderに検出されたパネルとそのPanelTypeを全てリストに格納する
            {
                var panelObject = panel.gameObject.GetComponent<Panel>(); // Panelの参照取得
                panelList.Add(panelObject); // Panelをリストに追加
                panelTypes.Add(panelObject.MyPanelType); // PanelTypeをリストに追加
            }

            var result = panelTypes.OrderBy((arg1) => Guid.NewGuid()).ToArray(); // Guid配列に変換、OrderByでアルファベット順に並び替える
            var count = 0; // ForEachに使うresult配列の要素指定用のカウンター

            panelList.ForEach((panel) => // リストに格納した各パネルにGuidでランダム化したPanelTypeを順番に代入の後パネルを引いていない状態に戻す
            {
                panel.MyPanelType = result[count]; // PanelTypeの代入
                panel.ResetPanel(); // パネルを引いていない状態に戻す
                count++; // カウントアップ
            });

            m_sensor.enabled = false; // colliderをdisableにする
            m_panelCounter.ResetShuffleSkillCounter(); // カウンターをリセット
            m_battleManager.SetStateMachine(m_battleManager.m_StateMachine.m_PreviousState); // stateを元に戻す
        }
    }
}

