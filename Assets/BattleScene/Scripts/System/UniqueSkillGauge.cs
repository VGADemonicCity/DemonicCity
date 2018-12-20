using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Unique skill gauge.
    /// </summary>
    public class UniqueSkillGauge : MonoBehaviour
    {
        /// <summary>スキルゲージ50%時の画像</summary>
        [SerializeField] Image m_halflyGauge;
        /// <summary>スキルゲージ100%時の画像</summary>
        [SerializeField] Image m_fullyGauge;
        /// <summary>UniqueSkillGaugeのアルファカラーチャンネル(不透明度)</summary>
        [SerializeField, Range(0, 1)] float m_alphaChannel;
        /// <summary>固有スキル発動条件(開いたパネル枚数)</summary>
        [SerializeField] int m_uniqueSkillCondition;
        /// <summary>開いたパネル枚数</summary>
        [SerializeField] float m_panelCount;

        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;
        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;

        /// <summary>UniqueSkillGaugeのアルファカラーチャンネル(不透明度)</summary>
        public float AlphaChannel
        {
            set
            {
                if (value < 1f)
                {
                    m_halflyGauge.color = new Color(1, 1, 1, value);
                }
                else
                {
                    // halfゲージを透明にして満タン状態の画像に切り替える
                    m_halflyGauge.color = Color.clear;
                    m_fullyGauge.color = Color.white;
                    OnConditionCompleted();
                }
            }
        }

        private void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得;
            m_panelCounter = PanelCounter.Instance; // PanelCounterの参照取得
        }

        private void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) =>
            {
                if (state == BattleManager.StateMachine.State.Init)
                {
                    Initialize();
                    Sync();
                }
            });
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            m_halflyGauge.color = Color.clear;
            m_fullyGauge.color = Color.clear;
            m_panelCount = 0;
        }

        /// <summary>
        /// 同期
        /// </summary>
        public void Sync()
        {
            m_panelCount++;
            AlphaChannel = m_panelCount / m_uniqueSkillCondition; // AlphaChannelに反映
        }

        /// <summary>
        /// 条件を満たした時
        /// </summary>
        private void OnConditionCompleted()
        {
            // 条件を満たした時の任意の処理や演出を実装する
        }
        
        /// <summary>
        /// 固有スキルを使用した時
        /// </summary>
        private void OnSkillUsed()
        {
            Initialize();
        }
    }
}
