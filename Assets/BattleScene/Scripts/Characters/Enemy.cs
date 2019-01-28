using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Enemy character.
    /// </summary>
    [Serializable]
    public class Enemy : MonoBehaviour , IAttackHandler
    {
        /// <summary>m_statsのプロパティ</summary>
        public Statistics Stats
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

        /// <summary>敵キャラのID</summary>
        [SerializeField] private EnemiesFactory.EnemiesId m_id;
        /// <summary>ステータス</summary>
        [SerializeField] Statistics m_stats = new Statistics();
        /// <summary>敵キャラのアニメーター</summary>
        [SerializeField] Animator m_animator;
        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;

        [SerializeField] float m_destroyingTime = 3f;

        private void Start()
        {
            m_animator = GetComponent<Animator>();

            m_battleManager = BattleManager.Instance;
            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                if(state != BattleManager.StateMachine.State.EnemyAttack || this != m_battleManager.CurrentEnemy)
                {
                    return;
                }

                OnAttack();
            });
        }

        /// <summary>
        /// Ons the attack.
        /// </summary>
        public void OnAttack()
        {
            m_animator.SetTrigger("Attack");
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
    }
}