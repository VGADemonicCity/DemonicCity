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
        [SerializeField] Image m_halflyGaugeIcon;
        /// <summary>スキルゲージ100%時の画像</summary>
        [SerializeField] Image m_fullyGaugeIcon;
        /// <summary>UniqueSkillGaugeのアルファカラーチャンネル(不透明度)</summary>
        [SerializeField, Range(0, 1)] float m_alphaChannel;
        /// <summary>開いたパネル枚数</summary>
        [SerializeField] float m_panelCount;

        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;
        /// <summary>UniqueSkillManagerの参照</summary>
        UniqueSkillManager m_uniqueSkillManager;
        /// <summary>ゲージ満タン時のエフェクトアニメーターコントローラー</summary>
        Animator m_effectAnimator;

        /// <summary>UniqueSkillGaugeのアルファカラーチャンネル(不透明度)</summary>
        public float AlphaChannel
        {
            get
            {
                return m_alphaChannel;
            }

            set
            {
                if (value < 0) // マイナスにさせない為の例外処理
                {
                    value = 0;
                }

                if (value < 1f) // 条件に対する現在の割合を透明度に反映する
                {
                    m_halflyGaugeIcon.color = new Color(1, 1, 1, value); // alphaChannelに現在のパネル枚数/条件値の割合値を代入
                    m_alphaChannel = value;
                }
                else
                {
                    // halfゲージを透明にして満タン状態の画像に切り替える
                    m_halflyGaugeIcon.color = Color.clear;
                    m_fullyGaugeIcon.color = Color.white;
                    m_alphaChannel = value;
                    OnConditionCompleted();
                }
            }
        }

        private void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得;
            m_uniqueSkillManager = UniqueSkillManager.Instance; // 参照取得
            m_effectAnimator = GetComponentInChildren<Animator>();


            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                if (state == BattleManager.StateMachine.State.Init) // init時に初期化する
                {
                    Initialize();
                }
            });
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            m_fullyGaugeIcon.color = Color.clear;
            m_halflyGaugeIcon.color = Color.clear;
            AlphaChannel = 0f;
            m_panelCount = 0;
            m_uniqueSkillManager.SkillFlag = false;
        }

        /// <summary>
        /// 同期
        /// </summary>
        public void Sync()
        {
            m_panelCount++;
            if (AlphaChannel < 1f) // AlphaChannelが最大値以外の時だけ
            {
                AlphaChannel = m_panelCount / m_uniqueSkillManager.Condition; // AlphaChannelに反映
            }
        }

        /// <summary>
        /// 条件を満たした時
        /// </summary>
        public void OnConditionCompleted()
        {
            // =============条件を満たした時の任意の処理や演出を実装する=============
            m_effectAnimator.SetTrigger(AnimParam.Activate.ToString());
            m_uniqueSkillManager.OnConditionCompleted();
        }

        /// <summary>
        /// 固有スキルを使用した時
        /// </summary>
        public void SkillActivated()
        {
            Initialize();
        }

        /// <summary>
        /// AnimatorControllerのパラメータ
        /// </summary>
        private enum AnimParam
        {
            Activate,
        }
    }
}
