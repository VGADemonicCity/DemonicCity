﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class PauseState : StatesBehaviour
    {
        private void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                //if (state == BattleManager.StateMachine.State.Pause)
                //{
                //    Time.timeScale = 0f;
                //}
                //else
                //{
                //    Time.timeScale = 1f;
                //}
            });
        }
    }
}