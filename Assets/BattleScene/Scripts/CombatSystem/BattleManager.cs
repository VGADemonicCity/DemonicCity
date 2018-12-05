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
    [Serializable]
    public class BattleManager : MonoSingleton<BattleManager>
    {

        public StateMachine m_stateMachine;
        [SerializeField] Magia m_magia;

        ///// <summary>ステートマシンの状態 : State of State Machine.</summary>
        //public State m_state = State.Init;
        ///// <summary>Wave.バトルシーンのウェーブフラグ</summary>
        //public Wave m_wave;

        /// <summary>ステート毎に呼び出すメソッドを変える : Change method calling each state.</summary>
        public StateMachineEvent m_behaviourByState = new StateMachineEvent();

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_stateMachine = StateMachine.Instance;
            m_magia = Magia.Instance;
        }

        /// <summary>
        /// State machine event.
        /// </summary>
        public class StateMachineEvent : UnityEvent<StateMachine.State>
        {
            public StateMachineEvent() { }
        }

        [Serializable]
        public class StateMachine : SSB<StateMachine>
        {
            /// <summary>
            /// State machine : ステートマシン.
            /// 状態遷移を管理する
            /// </summary>
            [Serializable]
            public enum State
            {
                /// <summary>ゲーム開始時</summary>
                Init,
                /// <summary>プレイヤー選択時</summary>
                PlayerChoice,
                /// <summary>プレイヤー攻撃時</summary>
                PlayerAttack,
                /// <summary>敵攻撃時</summary>
                EnemyAttack,
                /// <summary>一時停止時</summary>
                Pause,
                /// <summary>勝利時</summary>
                Win,
                /// <summary>敗北時</summary>
                Lose,
            }

            /// <summary>
            /// Wave.
            /// </summary>
            [Serializable]
            public enum Wave
            {
                /// <summary>第1ウェーブ.</summary>
                FirstWave,
                /// <summary>第2ウェーブ</summary>
                SecondWave,
                /// <summary>第3ウェーブ</summary>
                ThirdWave
            }

            public State m_state;
            public Wave m_wave;
        }
    }
}
