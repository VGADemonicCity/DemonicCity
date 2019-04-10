using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonicCity.BattleScene
{
    public class LoseState : StatesBehaviour
    {
        [SerializeField] float fadingTime = 3f;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.Lose || m_battleManager.m_StateMachine.m_PreviousState == BattleManager.StateMachine.State.Pause) // StateがLose以外の時は処理終了
                {
                    return;
                }
                Debug.Log("Lose state called.");
                SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Home, fadingTime);
            });
        }
    }

}
