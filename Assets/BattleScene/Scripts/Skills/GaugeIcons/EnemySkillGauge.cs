using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class EnemySkillGauge : MonoBehaviour
    {

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
                    //m_effectAnimator.SetTrigger(AnimParam.Activate.ToString());
                }
            }
        }

        /// <summary>スキルゲージ50%時の画像</summary>
        [SerializeField] Image m_halflyGaugeIcon;
        /// <summary>スキルゲージ100%時の画像</summary>
        [SerializeField] Image m_fullyGaugeIcon;
        /// <summary>UniqueSkillGaugeのアルファカラーチャンネル(不透明度)</summary>
        [SerializeField, Range(0, 1)] float m_alphaChannel;
        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;
        /// <summary>ゲージ満タン時のエフェクトアニメーターコントローラー</summary>
        Animator m_effectAnimator;

        int m_condition = 3;
        int m_turnCount;
        /// <summary>上昇倍率</summary>
        [SerializeField] float m_magnification = 1.5f;
        public bool m_flag;
        [SerializeField] float m_drawSpeed = 2f;


        private void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得;
            m_effectAnimator = GetComponentInChildren<Animator>();

            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                switch (state)
                {
                    case BattleManager.StateMachine.State.Init:
                        Initialize();
                        break;
                    case BattleManager.StateMachine.State.EnemyAttack:
                        if (m_flag == true)
                            SkillActivate();
                        break;
                    case BattleManager.StateMachine.State.PlayerChoice:
                        break;
                    case BattleManager.StateMachine.State.NextWave:
                        Initialize();
                        break;
                    default:
                        break;
                }

            });
        }

        /// <summary>
        /// スキル解除
        /// </summary>
        public void SkillDeactivate()
        {
            Initialize();
            m_battleManager.CurrentEnemy.Stats.Attack = m_battleManager.CurrentEnemy.Stats.Temp.Attack;
            m_turnCount--;
        }

        /// <summary>
        /// スキルを使用
        /// </summary>
        public void SkillActivate()
        {
            m_battleManager.CurrentEnemy.Stats.Attack = (int)(m_battleManager.CurrentEnemy.Stats.Attack * m_magnification);
        }

        /// <summary>
        /// 同期
        /// </summary>
        public void Sync()
        {
            m_turnCount++;
            var targetRatio = (float)m_turnCount / m_condition;
            StartCoroutine(Drawing(targetRatio));
            if (targetRatio >= 1f)
            {
                OnConditionCompleted();
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            m_fullyGaugeIcon.color = Color.clear;
            m_halflyGaugeIcon.color = Color.clear;
            AlphaChannel = 0f;
            m_turnCount = 0;
            m_flag = false;
        }

        /// <summary>
        /// 条件を満たした時
        /// </summary>
        private void OnConditionCompleted()
        {
            m_flag = true;
        }

        IEnumerator Drawing(float targetValue)
        {
            float remainingProgress;
            var diff = remainingProgress = targetValue - m_alphaChannel;
            var changePerFrame = diff * Time.deltaTime * m_drawSpeed;
            if (diff < 0f)
            {
                remainingProgress = -remainingProgress;
            }
            while (remainingProgress > 0f)
            {
                AlphaChannel += changePerFrame;
                remainingProgress -= changePerFrame;
                yield return null; // 1frame待つ
            }
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