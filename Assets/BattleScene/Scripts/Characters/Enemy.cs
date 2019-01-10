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

        private void Start()
        {
            m_animator = GetComponent<Animator>();

            m_battleManager = BattleManager.Instance;
            m_battleManager.m_behaviourByState.AddListener((state) =>
            {
                if(state != BattleManager.StateMachine.State.EnemyAttack)
                {
                    return;
                }

                OnAttack();
            });
        }

        public void OnAttack()
        {
            Debug.Log("OnAttackが呼ばれたよ");
            m_animator.SetTrigger("Attack");
        }
    }
}