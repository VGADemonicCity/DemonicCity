using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// ターンベースのステート管理をするクラス
    /// Singleton pattern
    /// </summary>
    public class BattleManager : MonoSingleton<BattleManager>
    {
        #region Property
        /// <summary>そのバトルに登場する敵オブジェクトのリスト</summary>
        public List<GameObject> EnemyObjects
        {
            get
            {
                return m_enemyObjects;
            }
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
            get
            {
                return m_enemies;
            }
            set
            {
                m_enemies = value;
            }
        }

        /// <summary>ステートマシン</summary>
        public StateMachine m_StateMachine { get; set; }
        #endregion

        #region Field
        /// <summary>敵の出現座標</summary>
        [SerializeField] public Transform m_CoordinateForSpawn;
        /// <summary>StateMacineのUnityEvent</summary>
        public StateMachineEvent m_BehaviourByState = new StateMachineEvent();
        /// <summary>バトル用のマギアのステータス</summary>
        public Status m_MagiaStats { get; set; }
        /// <summary>そのバトルに出てくる敵のオブジェクトのリスト</summary>
        List<GameObject> m_enemyObjects;
        /// <summary>そのバトルに出てくる敵のクラスのリスト</summary>
        List<Enemy> m_enemies = new List<Enemy>();
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

        private void Start()
        {
            Time.timeScale = 1f;
            // call event
            SetStateMachine(StateMachine.State.Init);
            SoundManager.Instance.StopWithFade(SoundManager.SoundTag.BGM);
        }


        /// <summary>
        /// 指定したステートに遷移させてBattleManagerのイベントを呼び出す
        /// </summary>
        /// <param name="state">State machine.</param>
        public void SetStateMachine(StateMachine.State state)
        {
            Debug.Log($"{state}がよばれたお！！！");
            // ステート遷移前のステートを保存 
            m_StateMachine.m_PreviousState = m_StateMachine.m_State;
            if (state == StateMachine.State.Pause || state == StateMachine.State.Debugging || state == StateMachine.State.Tutorial) // 遷移先がPauseステートの時保存
            {
                m_StateMachine.m_StateBeforeWithoutSpecialState = m_StateMachine.m_State;
            }
            m_StateMachine.
                m_State = state; // stateをセット

            Debug.Log(state);
            // ==================================
            // イベント呼び出し
            // ==================================
            m_BehaviourByState.Invoke(state);
        }

        /// <summary>
        /// Awake this instance.
        /// </summary>
        private void Awake()
        {
            m_StateMachine = StateMachine.Instance; // StateMachineの参照取得
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
                /// <summary>デバッグ画面表示時</summary>
                Debugging,
                /// <summary>Tutorials時</summary>
                Tutorial,
                /// <summary>コンテニュー選択中</summary>
                Continue,
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

            /// <summary>Pauseに遷移する前のステート</summary>
            public State m_StateBeforeWithoutSpecialState;
            /// <summary>遷移前のステート</summary>
            public State m_PreviousState;
            /// <summary>ステート</summary>
            public State m_State;
            /// <summary>ウェーブ</summary>
            public Wave m_Wave;

            /// <summary>現在のステートよりひとつ前のステートを返す.前のステートがPause,Debuggingの場合はPause,Debuggingステートに遷移する前のステートを返す</summary>
            public State PreviousStateWithoutSpecialStates
            {
                get
                {
                    if (m_PreviousState == State.Pause || m_PreviousState == State.Debugging || m_PreviousState == State.Tutorial)
                    {
                        return m_StateBeforeWithoutSpecialState;
                    }
                    return m_PreviousState;
                }
            }

            /// <summary>現在のステートよりひとつ前のステートがPauseステートならtrue,そうでない場合false</summary>
            public bool PreviousStateIsPause
            {
                get
                {
                    return m_PreviousState == State.Pause;
                }
            }
            /// <summary>現在のステートよりひとつ前のステートがDebuggingステートならtrue,そうでない場合false</summary>
            public bool PreviousStateIsDebugging
            {
                get
                {
                    return m_PreviousState == State.Debugging;
                }
            }
            /// <summary>現在のステートよりひとつ前のステートがPauseステートならtrue,そうでない場合false</summary>
            public bool PreviousStateIsTutorial
            {
                get
                {
                    return m_PreviousState == State.Tutorial;
                }
            }

            public bool PreviousStateIsSpecialStates =>
                m_PreviousState == State.Tutorial
                || m_PreviousState == State.Debugging
                || m_PreviousState == State.Pause;

            #endregion
        }
    }
}
