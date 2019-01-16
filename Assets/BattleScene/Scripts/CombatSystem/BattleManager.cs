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
        #region Property
        /// <summary>そのバトルに登場する敵オブジェクトのリスト</summary>
        public List<GameObject> EnemyObjects
        {
            get { return m_enemyObjects; }
            set
            {
                m_enemyObjects = value;
            }
        }

        /// <summary>現在の敵オブジェクト</summary>
        public GameObject CurrentEnemyObject
        {
            get
            {
                if (m_enemyObjects == null)
                {
                    Debug.Log("m_enemyObjectsが設定されていません");
                }

                switch (m_StateMachine.m_Wave)
                {
                    case StateMachine.Wave.FirstWave:
                        return m_enemyObjects[0];
                    case StateMachine.Wave.SecondWave:
                        return m_enemyObjects[1];
                    case StateMachine.Wave.LastWave:
                        return m_enemyObjects[2];
                    default:
                        Debug.Log("CurrentEnemyObject取得に失敗しました。");
                        return null;
                }
            }
        }

        /// <summary>現在の敵オブジェクトのEnemyクラス</summary>
        public Enemy CurrentEnemy
        {
            get
            {
                if (Enemies == null)
                {
                    Debug.Log("m_enemiesが設定されていません");
                }

                switch (m_StateMachine.m_Wave)
                {
                    case StateMachine.Wave.FirstWave:
                        return Enemies[0];
                    case StateMachine.Wave.SecondWave:
                        return Enemies[1];
                    case StateMachine.Wave.LastWave:
                        return Enemies[2];
                    default:
                        Debug.Log("CurrentEnemy取得に失敗しました。");
                        return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the m_enemies.
        /// </summary>
        /// <value>The enemies.</value>
        public List<Enemy> Enemies
        {
            get { return m_enemies; }
            set { m_enemies = value; }
        }

        #endregion

        #region Field
        /// <summary>StateMacineのUnityEvent</summary>
        public StateMachineEvent m_BehaviourByState = new StateMachineEvent();
        /// <summary>バトル用のマギアのステータス</summary>
        [SerializeField] public Statistics m_MagiaStats;
        /// <summary>ステートマシン</summary>
        public StateMachine m_StateMachine { get; set; }
        /// <summary>敵キャラのデータベース</summary>
        public EnemiesFactory m_EnemiesFactory { get; set; }

        /// <summary>そのバトルに出てくる敵のオブジェクトのリスト</summary>
        [SerializeField] private List<GameObject> m_enemyObjects;
        /// <summary>そのバトルに出てくる敵のクラスのリスト</summary>
        [SerializeField] private List<Enemy> m_enemies;
        /// <summary>敵の出現座標</summary>
        [SerializeField] public Transform m_CoordinateForSpawn;
        #endregion

        #region Method
        /// <summary>
        /// Setting wave to StateMachine
        /// ステートマシンに<param name="wave">wave</param>を設定する
        /// </summary>
        public void InitWave()
        {
            m_StateMachine.m_Wave = StateMachine.Wave.FirstWave;
        }

        /// <summary>
        /// 次のウェーブがあるか判断し、あれば次のウェーブへ。なければバトル終了へ。
        /// </summary>
        public void TryNextWave()
        {
            if (m_StateMachine.m_Wave != StateMachine.Wave.LastWave)
            {
                m_StateMachine.m_Wave++;
                Debug.Log(m_StateMachine.m_Wave);
                // ==============================
                // イベント呼び出し : StateMachine.NextWave
                // ==============================
                m_BehaviourByState.Invoke(StateMachine.State.NextWave);
                return;
            }
            // ==============================
            // イベント呼び出し : StateMachine.End
            // ==============================
            m_BehaviourByState.Invoke(StateMachine.State.Win);
        }

        /// <summary>
        /// Awake this instance.
        /// </summary>
        private void Awake()
        {
            m_StateMachine = StateMachine.Instance; // StateMachineの参照取得
            m_EnemiesFactory = EnemiesFactory.Instance; // EnemiesDataBaseの参照取得
        }
        #endregion

        #region StateMachineAndItUnityEvent

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
                /// <summary>ラストウェーブ</summary>
                LastWave,
            }

            /// <summary>ステート</summary>
            public State m_State;
            /// <summary>ウェーブ</summary>
            public Wave m_Wave;
        }
        #endregion
    }
}
