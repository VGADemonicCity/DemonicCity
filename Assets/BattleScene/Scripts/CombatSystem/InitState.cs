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
                m_magia.StatsBuffer = m_magia.Stats;　// 一番最初のターンだけバッファ変数にStatsを代入
                Debug.Log(m_magia.StatsBuffer.m_attack + "ばっふぁわん");
                m_panelCounter.InitCounts(); // カウント初期化
                m_panelManager.InitPanels(); // パネル初期化
                // ==============================
                // イベント呼び出し : StateMachine.PlayerChoice
                // ==============================
                SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
            });

            StartCoroutine(StartWait());
        }

        IEnumerator StartWait()
        {
            yield return new WaitForSeconds(1f);
            // ==============================
            // イベント呼び出し : StateMachine.Init
            // ==============================
            Debug.Log(m_battleManager.m_stateMachine.m_state);
            m_battleManager.m_behaviourByState.Invoke(BattleManager.StateMachine.State.Init);
        }
    }
}

