using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Player choice.
    /// </summary>
    public class PlayerChoiceState : StateMachineBehaviour
    {
        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.PlayerChoice) // StateがPlayerChoice以外の時は処理終了
                {
                    return;
                }
                Debug.Log("PlayerChoice state called.");
                m_panelCounter.InitCounts(); // カウント初期化
                m_panelManager.InitPanels(); // パネル初期化
                // ==============================
                // ここにプレイヤーターンが始まった時の処理を書く
                // PlayerCoiceStateから遷移する処理はPanelCounterが敵パネルを認識して処理させている
                // ==============================
            });
        }
    }
}