using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Init.
    /// </summary>
    public class InitState : StatesBehaviour
    {
        /// <summary>MagiaのHPDrawの参照</summary>
        [SerializeField] HitPointGauge m_magiaHPGauge;
        /// <summary>EnemyのHPDrawの参照</summary>
        [SerializeField] HitPointGauge m_enemyHPGauge;
        /// <summary>章の情報を管理するクラス</summary>
        ChapterManager m_chapterManager;
        /// <summary>章情報に基づき敵のインスタンスを生成する工場</summary>
        EnemiesFactory m_enemiesFactory;

        protected override void Awake()
        {
            base.Awake();

            SceneManager.sceneLoaded += ((scene, mode) => // シーンがロードされたとき
            {
                if (scene.name != "Battle")
                {
                    return;
                }

                m_chapterManager = ChapterManager.Instance;
                m_enemiesFactory = EnemiesFactory.Instance;

                // =================
                // Debug用
                // =================
                m_gameManager.CurrentChapter = m_chapterManager.GetChapter(Progress.StoryProgress.Nafla);
                m_battleManager.EnemyObjects = m_enemiesFactory.Create(m_gameManager.CurrentChapter);
            });
        }


        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            // ==============================
            // イベント呼び出し : StateMachine.Init
            // ==============================
            m_battleManager.m_BehaviourByState.Invoke(BattleManager.StateMachine.State.Init);


            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.Init) // StateがInit以外の時は処理終了
            {
                    return;
                }
                Debug.Log("Init state called.");
                Initialize();
            });
        }

        private void Initialize()
        {
            m_battleManager.InitWave(); // wave初期化
            m_battleManager.m_magiaStats = m_magia.GetStats(); // 現在のマギアのステータスをバトル用Statisticsのインスタンスを作る
            m_battleManager.m_magiaStats.Init(m_battleManager.m_magiaStats); // magiaのステータスを初期化
            m_panelCounter.InitCounts(); // カウント初期化
            m_panelManager.InitPanels(); // パネル初期化
            m_magiaHPGauge.Initialize(m_battleManager.m_magiaStats.m_hitPoint); // マギアのHP最大値を引数に初期化する
            m_enemyHPGauge.Initialize(m_battleManager.CurrentEnemy.Stats.m_hitPoint); // 敵のHP最大値を引数に初期化する


        }
    }
}

