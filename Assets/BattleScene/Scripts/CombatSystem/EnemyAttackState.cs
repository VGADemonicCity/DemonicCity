﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Enemy attack state.
    /// </summary>
    public class EnemyAttackState : StatesBehaviour
    {

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.EnemyAttack) // StateがEnemyAttack以外の時は処理終了
                {
                    return;
                }

                Debug.Log("EnemyAttack state called.");
                StartCoroutine(AttackProcess());
            });
        }

        IEnumerator AttackProcess()
        {
            // ==============================
            // ここに攻撃の演出処理を入れる予定
            // ==============================

            Debug.Log("敵から攻撃される前の[" + m_magia + "]の体力 : " + m_battleManager.m_MagiaStats.HitPoint);

            // Enemyの攻撃アニメーションを再生する
            m_battleManager.CurrentEnemy.AnimCtrl.CrossFadeInFixedTime("Attack", 0);
            var clips = m_battleManager.CurrentEnemy.AnimCtrl.GetCurrentAnimatorClipInfo(0);
            while(clips.Length == 0)
            {
                yield return null;
                clips = m_battleManager.CurrentEnemy.AnimCtrl.GetCurrentAnimatorClipInfo(0);
            }
            yield return new WaitForSeconds(clips[0].clip.length); // アニメーションの長さ分遅延させる

            if (m_enemySkillGauge.m_flag == true)
            {
                m_enemySkillGauge.SkillActivate();
            }

            var damage = m_battleManager.CurrentEnemy.Stats.Attack - m_battleManager.m_MagiaStats.Defense; // 敵の攻撃力からプレイヤーの防御力を引いた値
            var attack = m_battleManager.CurrentEnemy.Stats.Temp.Attack;
            Debug.Log("敵の攻撃力     " + attack);
            if (damage > 0)
            {
                m_battleManager.m_MagiaStats.HitPoint -= damage; // ダメージ
                m_magiaHPGauge.Sync(m_battleManager.m_MagiaStats.HitPoint); // HPGaugeと同期
            }
            Debug.Log("敵から攻撃された後の[" + m_magia + "]の体力 : " + m_battleManager.m_MagiaStats.HitPoint);

            OnProcessEnded();

            // ゲージが現状してる間の待つ
            yield return new WaitForSeconds(1f);


            if (m_battleManager.m_MagiaStats.HitPoint > 0) // プレイヤーの体力が1以上だったら次のターンへ遷移する
            {
                // ==============================
                // イベント呼び出し : StateMachine.PlayerChoice
                // ==============================
                m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
            }
            else // PlayerのHPが0以下になったらLoseステートに遷移する
            {
                // ==============================
                // イベント呼び出し : StateMachine.Lose
                // ==============================
                m_battleManager.SetStateMachine(BattleManager.StateMachine.State.Lose);
            }
        }

        /// <summary>
        /// スキルの発動が済んだら
        /// </summary>
        private void OnProcessEnded()
        {

            if (m_enemySkillGauge.m_flag == true)
            {
                m_enemySkillGauge.SkillDeactivate();
            }
            m_enemySkillGauge.Sync();
        }
    }
}
