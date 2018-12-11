using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Init.
    /// </summary>
    public class InitState : StatesBehaviour
    {
        /// <summary>MagiaのHPDrawの参照</summary>
        [SerializeField] ExampleHpDraw m_magiaHpDraw;
        /// <summary>EnemyのHPDrawの参照</summary>
        [SerializeField] ExampleHpDraw m_enemyHpDraw; 

        protected override void Awake()
        {
            base.Awake();
            m_magiaHpDraw = m_magiaHpDraw.GetComponent<ExampleHpDraw>(); // magiaのHPDrawコンポーネント取得
            m_enemyHpDraw = m_enemyHpDraw.GetComponent<ExampleHpDraw>(); // enemyのHPDrawコンポーネント取得

        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.Init) // StateがInit以外の時は処理終了
                {
                    return;
                }
                Debug.Log("Init state called.");
                m_magia.Stats.Init(m_magia.Stats); // 一番最初のターンだけバッファ変数にStatsを代入
                m_panelCounter.InitCounts(); // カウント初期化
                m_panelManager.InitPanels(); // パネル初期化
                m_magia.InitMaxHP(m_magia.Stats.m_hitPoint);
                m_magiaHpDraw.Initialize(m_magia.MaxHP); // マギアのHP最大値を引数に初期化する
                m_enemyHpDraw.Initialize(m_battleManager.m_enemy.Stats.m_hitPoint); // 敵のHP最大値を引数に初期化する

                // ==============================
                // イベント呼び出し : StateMachine.PlayerChoice
                // ==============================
                SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
            });

            StartCoroutine(StartWait());
        }

        /// <summary>
        /// Starts the wait.
        /// </summary>
        /// <returns>The wait.</returns>
        IEnumerator StartWait()
        {
            yield return new WaitForSeconds(1f);
            // ==============================
            // イベント呼び出し : StateMachine.Init
            // ==============================
            m_battleManager.m_behaviourByState.Invoke(BattleManager.StateMachine.State.Init);
        }
    }
}

