using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Next wave state.
    /// </summary>
    public class NextWaveState : StatesBehaviour
    {
        /// <summary>敵のオブジェクト群を動かすクラス</summary>
        [SerializeField] EnemiesMover m_enemiesMover;
        /// <summary>移動間隔</summary>
        readonly Vector2 m_movementSpacing = new Vector2(5f, 0f);

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.NextWave) // StateがWin以外の時は処理終了
                {
                    return;
                }
                Debug.Log("NextWave state called.");
                m_battleManager.m_StateMachine.m_Wave++;

                m_battleManager.CurrentEnemy.Stats.Init(m_battleManager.CurrentEnemy.Stats); //バトル開始直前の 敵のステータスの初期値を保存
                m_enemyHPGauge.Initialize(m_battleManager.CurrentEnemy.Stats.m_hitPoint); // 敵のHP最大値をGaugeに登録する
                m_enemyHPGauge.Sync(m_battleManager.CurrentEnemy.Stats.m_hitPoint); // HPの値をゲージに同期
                m_enemiesMover.Moving(); // 敵を前進させる
                Debug.Log("current wave is : " + m_battleManager.m_StateMachine.m_Wave);
                SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
            });
        }
    }
}

