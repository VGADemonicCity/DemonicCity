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
        [SerializeField] Animator skillAnimator;


        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;

        [SerializeField] float m_destroyingTime = 3f;

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
        public float Attack()
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

        public void PlayEffect()
        {
            skillAnimator.CrossFadeInFixedTime("Effect", 0);
        }
    }
}