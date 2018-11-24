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
        /// State machine : ステートマシン.
        /// 状態遷移を管理する
        /// </summary>
        public enum StateMachine
        {
            /// <summary>ゲーム開始時</summary>
            Init,
            /// <summary>プレイヤー選択時</summary>
            PlayerChoice,
            /// <summary>プレイヤー攻撃時</summary>
            PlayerAttack,
            /// <summary>敵攻撃時</summary>
            EnemyAttack,
            /// <summary>生存確認等の判定時</summary>
            Judge,
            /// <summary>次のターンに遷移する時</summary>
            NextTurn,
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
        public enum Wave
        {
            /// <summary>第1ウェーブ.</summary>
            FirstWave,
            /// <summary>第2ウェーブ</summary>
            SecondWave,
            /// <summary>第3ウェーブ</summary>
            ThirdWave
        }



        /// <summary>ステートマシンの状態 : State of State Machine.</summary>
        public StateMachine m_state;
        /// <summary>Wave.バトルシーンのウェーブフラグ</summary>
        public Wave m_wave;
        /// <summary>ステート毎に呼び出すメソッドを変える : Change method calling each state.</summary>
        public StateMachineEvent m_behaviourByState = new StateMachineEvent();

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_state = StateMachine.PlayerChoice; // ゲーム開始時, 
            // ===========
            // Debug用。本当はInitから始める
            // ===========
        }

        /// <summary>
        /// State machine event.
        /// </summary>
        public class StateMachineEvent : UnityEvent<StateMachine>
        {
            public StateMachineEvent() { }
        }
    }
}
