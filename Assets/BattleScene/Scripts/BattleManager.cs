using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Battle manager.
    /// Singleton pattern
    /// </summary>
    public class BattleManager : MonoSingleton<BattleManager>
    {
        /// <summary>
        /// State machine.
        /// </summary>
        public enum States
        {
            /// <summary></summary>
            Init,
            /// <summary></summary>
            CountDown,
            /// <summary></summary>
            PlayerTurn,
            /// <summary></summary>
            EnemyTurn,
            /// <summary></summary>
            Pause,
            /// <summary></summary>
            Win,
            /// <summary></summary>
            Lose,
            /// <summary></summary>
            Calculatate
        }

        /// <summary>ステートマシンの状態 : State of State Machine.</summary>
        public States m_states { get; private set; }
        /// <summary>ステート毎に呼び出すメソッドを変える : Change method calling each state.</summary>
        public StateMachineEvent m_behaviourByState = new StateMachineEvent();


        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            //m_states = States.Init; // 
            m_states = States.PlayerTurn; // debug用
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <param name="state">Request state.</param>
        public void ChangeState(States state)
        {
            m_states = state; // 引数のstateに状態遷移
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        void Update()
        {
            m_behaviourByState.Invoke(m_states); // イベント呼び出し
        }

        /// <summary>
        /// State machine event.
        /// </summary>
        public class StateMachineEvent : UnityEvent<States>
        {
            public StateMachineEvent() { }
        }
    }
}
