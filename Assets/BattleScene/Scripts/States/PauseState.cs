using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace DemonicCity.BattleScene
{
    public class PauseState : StatesBehaviour
    {
        private void Start()
        {


            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                if (state == BattleManager.StateMachine.State.Pause
                || state == BattleManager.StateMachine.State.Debugging 
                || state == BattleManager.StateMachine.State.Tutorial)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            });
        }
    }
}