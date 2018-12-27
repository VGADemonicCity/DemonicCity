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
                m_panelCounter.InitCounts();
                m_panelManager.InitPanels();
                m_panelFrameManager.MovingCenter();

                // ==============================
                // ここにプレイヤーターンが始まった時の処理を書く
                // PlayerCoiceStateから遷移する処理はPanelCounterが敵パネルを認識してState遷移処理をさせている
                // PlayerChoiceの時のinvokeはPanelCounter.PanelJudgerが行っている
                // ==============================
            });
        }
    }
}