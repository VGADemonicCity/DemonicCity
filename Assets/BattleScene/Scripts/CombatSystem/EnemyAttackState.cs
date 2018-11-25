using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Enemy attack state.
    /// </summary>
    public class EnemyAttackState : StateMachineBehaviour
    {
        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.EnemyAttack) // StateがEnemyAttack以外の時は処理終了
                {
                    return;
                }

                Debug.Log("EnemyAttack state called.");
                if (m_magia.m_stats.m_hitPoint <= 0) // PlayerのHPが0以下になったらLoseステートに遷移する
                {
                    // ==============================
                    // イベント呼び出し : StateMachine.Lose
                    // ==============================
                    SetStateMachine(BattleManager.StateMachine.Lose);
                }
                else
                {
                    // ==============================
                    // イベント呼び出し : StateMachine.PlayerChoice
                    // ==============================
                    SetStateMachine(BattleManager.StateMachine.PlayerChoice);
                }
            });
        }
    }
}
