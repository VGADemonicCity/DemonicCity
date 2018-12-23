using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Init.
    /// </summary>
    public class InitState : StatesBehaviour
    {
        /// <summary>MagiaのHPDrawの参照</summary>
        [SerializeField] HitPointGauge m_magiaHPGauge;
        /// <summary>EnemyのHPDrawの参照</summary>
        [SerializeField] HitPointGauge m_enemyHPGauge; 

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.Init) // StateがInit以外の時は処理終了
                {
                    return;
                }
                Debug.Log("Init state called.");
                Initialize();

                // ==============================
                // イベント呼び出し : StateMachine.PlayerChoice
                // ==============================
                SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
            });
        }

        private void Initialize()
        {
            m_battleManager.m_magiaStats = m_magia.GetStats(); // 現在のマギアのステータスをバトル用Statisticsのインスタンスを作る
            m_battleManager.m_magiaStats.Init(m_battleManager.m_magiaStats); // magiaのステータスを初期化
            m_panelCounter.InitCounts(); // カウント初期化
            m_panelManager.InitPanels(); // パネル初期化
            m_magiaHPGauge.Initialize(m_battleManager.m_magiaStats.m_hitPoint); // マギアのHP最大値を引数に初期化する
            m_enemyHPGauge.Initialize(m_battleManager.m_enemy.Stats.m_hitPoint); // 敵のHP最大値を引数に初期化する
        }
    }
}

