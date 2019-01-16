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
        [Header("Components")]
        /// <summary>敵のオブジェクト群を動かすクラス</summary>
        [SerializeField] EnemiesMover m_enemiesMover;
        /// <summary>章の情報を管理するクラス</summary>
        ChapterManager m_chapterManager;
        /// <summary>章情報に基づき敵のインスタンスを生成する工場</summary>
        EnemiesFactory m_enemiesFactory;


        [Header("Parameters")]
        [SerializeField] float m_spawnSpacingValue = -5f;

        protected override void Awake()
        {
            base.Awake();
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


        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            // ==============================
            // イベント呼び出し : StateMachine.Init
            // ==============================
            m_battleManager.m_BehaviourByState.Invoke(BattleManager.StateMachine.State.Init);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        private void Initialize()
        {
            SpawnEnemies();
            m_enemiesMover.Moving();

            m_battleManager.InitWave(); // wave初期化
            m_battleManager.m_magiaStats = m_magia.GetStats(); // 現在のマギアのステータスをバトル用Statisticsのインスタンスを作る
            m_battleManager.m_magiaStats.Init(m_battleManager.m_magiaStats); // magiaのステータスを初期化
            m_panelCounter.InitCounts(); // カウント初期化
            m_panelManager.InitPanels(); // パネル初期化
            m_magiaHPGauge.Initialize(m_battleManager.m_magiaStats.m_hitPoint); // マギアのHP最大値を引数に初期化する
            m_enemyHPGauge.Initialize(m_battleManager.CurrentEnemy.Stats.m_hitPoint); // 敵のHP最大値を引数に初期化する
        }

        private void SpawnEnemies()
        {
            m_chapterManager = ChapterManager.Instance;
            m_enemiesFactory = EnemiesFactory.Instance;
            m_gameManager.CurrentChapter = m_chapterManager.GetChapter(Progress.StoryProgress.Nafla); // ======DEBUG======プログレスを代入する処理をいれる予定
            m_battleManager.EnemyObjects = m_enemiesFactory.Create(m_gameManager.CurrentChapter);

            // 敵を1体生成する度にm_spawnSpacingValue分座標をずらして生成する
            // 生成した敵オブジェクトにアタッチされているEnemyコンポーネントをBattleManagerのリストに格納する
            float spacings;
            Vector3 spawnPosition = m_battleManager.m_CoordinateForSpawn.position;
            GameObject enemyObject;

            for (int i = 0; i < m_battleManager.EnemyObjects.Count; i++)
            {
                spacings = m_battleManager.m_CoordinateForSpawn.position.x + (m_spawnSpacingValue * i);
                spawnPosition.x = spacings;
                enemyObject = Instantiate(m_battleManager.EnemyObjects[i], spawnPosition, Quaternion.identity, m_battleManager.m_CoordinateForSpawn);
                m_battleManager.Enemies.Add(enemyObject.GetComponent<Enemy>());
            }
        }
    }
}

