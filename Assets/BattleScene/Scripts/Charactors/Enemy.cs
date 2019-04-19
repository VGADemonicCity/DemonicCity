using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Enemy character.
    /// </summary>
    [Serializable]
    public class Enemy : MonoBehaviour
    {
        /// <summary>m_statsのプロパティ</summary>
        public Status Stats
        {
            get { return m_stats; }
            set { m_stats = value; }
        }

        /// <summary>敵キャラID取得プロパティ</summary>
        public EnemiesFactory.EnemiesId Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// Get Animator component
        /// </summary>
        public Animator AnimCtrl { get { return m_animator; } }

        /// <summary>敵キャラのID</summary>
        [SerializeField] private EnemiesFactory.EnemiesId m_id;
        /// <summary>ステータス</summary>
        [SerializeField] Status m_stats = new Status();
        /// <summary>敵キャラのアニメーター</summary>
        [SerializeField] Animator m_animator;
        /// <summary>Skill animator</summary>
        [SerializeField] Animator skillAnimator;
        /// <summary>Required time for destruction</summary>
        [SerializeField] float m_destroyingTime = 3f;

        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;

        /// <summary>バフの上昇値を保存しておく変数</summary>
        int buffBuffer;

        private void Start()
        {
            m_battleManager = BattleManager.Instance;
            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                if (state != BattleManager.StateMachine.State.EnemyAttack || this != m_battleManager.CurrentEnemy)
                {
                    return;
                }
            });
        }

        /// <summary>
        ///  Ons the attack.
        /// </summary>
        /// <returns>time of animation clip</returns>
        public float PlayAnimationOfAttack()
        {
            m_animator.CrossFadeInFixedTime("Attack", 0);
            var clips = m_animator.GetNextAnimatorClipInfo(0).ToList();
            return clips.First().clip.length;
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public void Destroy()
        {
            Debug.Log(gameObject.name + "は破壊されたよ");
            gameObject.SetActive(false);
            Destroy(gameObject, m_destroyingTime);
        }

        /// <summary>
        /// エフェクトアニメーションを再生する
        /// Animationからこのイベントを登録する
        /// </summary>
        public void PlayEffect()
        {
            skillAnimator.CrossFadeInFixedTime("Effect", 0);
        }

        /// <summary>
        /// 攻撃力を引数の整数分上昇させる
        /// 
        /// </summary>
        /// <param name="increase"></param>
        public void AttackBuffActivate(int increase)
        {
            buffBuffer = increase;
            Stats.Attack += increase;
        }

        /// <summary>
        /// 上昇した攻撃力を元に戻る
        /// </summary>
        public void AttackBuffDeactivate()
        {
            Stats.Attack -= buffBuffer;
        }
    }
}