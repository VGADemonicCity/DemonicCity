using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
            /// <summary>ゲーム開始時</summary>
            InitGame,
            /// <summary>プレイヤー選択時</summary>
            PlayerChoice,
            /// <summary>プレイヤー攻撃時</summary>
            PlayerAttack,
            /// <summary>敵攻撃時</summary>
            EnemyChoice,
            /// <summary>一時停止時</summary>
            Pause,
            /// <summary>勝利時</summary>
            Win,
            /// <summary>失敗時</summary>
            Lose,
        }

        /// <summary>ステートマシンの状態 : State of State Machine.</summary>
        public States m_states;
        /// <summary>ステート毎に呼び出すメソッドを変える : Change method calling each state.</summary>
        public StateMachineEvent m_behaviourByState = new StateMachineEvent();

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_states = States.PlayerChoice; // ゲーム開始時
        }

        /// <summary>
        /// Start this instance.
        /// ここにステート毎の処理を書く
        /// </summary>
        void Start()
        {
            m_behaviourByState.AddListener((states) => 
            {
                switch (states)
                {
                    case States.InitGame:
                        break;
                    case States.PlayerChoice:
                        break;
                    case States.PlayerAttack:
                        break;
                    case States.EnemyChoice:
                        break;
                    case States.Pause:
                        break;
                    case States.Win:
                        break;
                    case States.Lose:
                        break;
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        void Update()
        {
            //m_behaviourByState.Invoke(m_states); // イベント呼び出し
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
