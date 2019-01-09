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
        /// <summary>敵のID</summary>
        public EnemiesFactory.EnemiesId EnemyId { get; set; }
        /// <summary>ステートマシン</summary>
        public StateMachine m_stateMachine { get; set; }
        /// <summary>敵キャラのデータベース</summary>
        public EnemiesFactory m_enemiesData { get; set; }
        /// <summary>バトルシーンで使用する敵オブジェクト</summary>
        [SerializeField] public List<GameObject> m_enemyObjects;
        /// <summary>バトルシーンで使用する敵オブジェクト</summary>
        [SerializeField] public GameObject m_enemyObject;
        /// <summary>そのバトルに出てくる敵のリスト</summary>
        [SerializeField] public List<Enemy> m_enemies;
        /// <summary>敵オブジェクトのEnemyクラス</summary>
        [SerializeField] public Enemy m_currentEnemy;
        /// <summary>バトル用のマギアのステータス</summary>
        [SerializeField] public Statistics m_magiaStats;

        ///// <summary>ステートマシンの状態 : State of State Machine.</summary>
        //public State m_state = State.Init;
        ///// <summary>Wave.バトルシーンのウェーブフラグ</summary>
        //public Wave m_wave;

        /// <summary>StateMacineのイベントシステム</summary>
        public StateMachineEvent m_behaviourByState = new StateMachineEvent();

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {

            EnemyId = EnemiesFactory.EnemiesId.Nafla; // =========実際はこのenumをステージに応じて登場するキャラクターに変える==========
            m_stateMachine = StateMachine.Instance; // StateMachineの参照取得
            m_enemiesData = EnemiesFactory.Instance; // EnemiesDataBaseの参照取得


        }


        /// <summary>
        /// State machine event.
        /// </summary>
        public class StateMachineEvent : UnityEvent<StateMachine.State>
        {
            public StateMachineEvent() { }
        }

        /// <summary>
        /// State machine : ステートマシン.
        /// 状態遷移を管理する
        /// </summary>
        [Serializable]
        public class StateMachine : SavableSingletonBase<StateMachine>
        {
            /// <summary>
            /// State.
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
                /// <summary>次のWaveに遷移する時</summary>
                NextWave,
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

            /// <summary>ステート</summary>
            public State m_state;
            /// <summary>ウェーブ</summary>
            public Wave m_wave;
        }
    }
}
