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
        /// <summary>animator of wave title</summary>
        [SerializeField] WaveTitle waveTitle;

        /// <summary>章情報に基づき敵のインスタンスを生成する工場</summary>
        EnemiesFactory m_enemiesFactory;
        /// <summary>最初の遅延時間</summary>
        [SerializeField] float firstWaitTime = 1f;

        /// <summary>
        /// 敵オブジェクト群の生成間隔
        /// </summary>
        [Header("Parameters")]
        const float m_spawnSpacing = -5f;

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
                StartCoroutine(FirstWaveAnimation());
            });
        }

        IEnumerator FirstWaveAnimation()
        {
            yield return new WaitForSeconds(firstWaitTime);
            // 敵を動かす
            var waitTime = m_enemiesMover.Moving();
            //yield return new WaitForSeconds(waitTime);

            // Waveタイトルのアニメーションを再生した後,ステートを遷移させる
            waitTime =waveTitle.Play();
            yield return new WaitForSeconds(waitTime);
            // ==============================
            // イベント呼び出し : StateMachine.PlayerChoice
            // ==============================
            m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);

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
        void Initialize()
        {
            // その章のChapterを取得
            //m_chapter = ChapterManager.Instance.GetChapter();
            m_chapter = ChapterManager.Instance.GetChapter(Progress.StoryProgress.Nafla);


            SpawnEnemies();



            m_battleManager.InitWave(); // wave初期化
            m_panelCounter.InitializeCounter(); // カウント初期化
            m_panelManager.InitPanels(); // パネル初期化
            m_battleManager.m_MagiaStats = m_magia.GetStats(); // バトル用のStatisticsインスタンスにmagiaのStatsの各値を登録する
            m_battleManager.m_MagiaStats.Init(m_battleManager.m_MagiaStats); //バトル開始直前の Magiaのステータスの初期値を保存
            m_battleManager.CurrentEnemy.Stats.Init(m_battleManager.CurrentEnemy.Stats); // 敵のステータスを初期化
            m_magiaHPGauge.Initialize(m_battleManager.m_MagiaStats.HitPoint); // マギアのHP最大値を引数に初期化する
            m_enemyHPGauge.Initialize(m_battleManager.CurrentEnemy.Stats.HitPoint); // 敵のHP最大値を引数に初期化する
        }



        /// <summary>
        /// Waveに出現する敵群を生成する
        /// </summary>
        void SpawnEnemies()
        {
            m_enemiesFactory = EnemiesFactory.Instance;
            m_battleManager.EnemyObjects = m_enemiesFactory.Create(m_chapter); // ======DEBUG======完成時は引数なしにする　

            // 敵を1体生成する度にm_spawnSpacingValue分座標をずらして生成する
            // 生成した敵オブジェクトにアタッチされているEnemyコンポーネントをBattleManagerのリストに格納する
            float spacings;
            Vector3 spawnPosition = m_battleManager.m_CoordinateForSpawn.position;
            GameObject enemyObject;

            for (int i = 0; i < m_battleManager.EnemyObjects.Count; i++)
            {
                spacings = m_battleManager.m_CoordinateForSpawn.position.x + (m_spawnSpacing * i);
                spawnPosition.x = spacings;
                enemyObject = Instantiate(m_battleManager.EnemyObjects[i], spawnPosition, Quaternion.identity, m_battleManager.m_CoordinateForSpawn);
                m_battleManager.Enemies.Add(enemyObject.GetComponent<Enemy>());
            }
        }
    }
}

