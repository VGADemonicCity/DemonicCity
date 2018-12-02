using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonicCity.BattleScene.Skill;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Player choice.
    /// </summary>
    public class PlayerChoiceState : StatesBehaviour
    {
        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.PlayerChoice) // StateがPlayerChoice以外の時は処理終了
                {
                    return;
                }
                Debug.Log("PlayerChoice state called.");

                // 毎ターン前ターンのスキル情報をさせる為、スキル適応前のStats情報をbufferに保存してプレイヤーターン開始時毎に初期化する様にする
                if (m_magia.StatsBuffer != null) { m_magia.Stats = m_magia.StatsBuffer; }
                m_panelCounter.InitCounts(); // カウント初期化
                m_panelManager.InitPanels(); // パネル初期化
                // ==============================
                // ここにプレイヤーターンが始まった時の処理を書く
                // PlayerCoiceStateから遷移する処理はPanelCounterが敵パネルを認識してState遷移処理をさせている
                // ==============================
            });
        }
    }
}