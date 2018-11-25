using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonicCity.BattleScene
{
    public class LoseState : StateMachineBehaviour
    {
        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.Lose) // StateがLose以外の時は処理終了
                {
                    return;
                }
                Debug.Log("Lose state called.");
            });
        }
    }

}
