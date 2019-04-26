using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace DemonicCity.BattleScene
{
    public class PauseState : StatesBehaviour
    {
        [SerializeField] AudioMixer mixer;
        [SerializeField] float volumeInThePause = -20f;

        private void Start()
        {


            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                if (state == BattleManager.StateMachine.State.Pause || state == BattleManager.StateMachine.State.Debugging)
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