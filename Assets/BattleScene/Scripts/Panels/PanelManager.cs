using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DemonicCity.BattleScene.Skill;
using DemonicCity.Debugger;
using UnityEngine.EventSystems;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Panel manager.
    /// </summary>
    public class PanelManager : MonoSingleton<PanelManager>
    {

        /// <summary>Flag</summary>
        public bool m_isPanelProcessing { get; private set; }
        /// <summary>Scene上に存在する全てのパネルのリスト</summary>
        public List<Panel> PanelsInTheScene { get; set; }
        /// <summary>Scene上に存在する敵以外のパネル</summary>
        public List<Panel> AllPanelsExceptEnemyPanels { get { return PanelsInTheScene.FindAll(panel => panel.MyPanelType != PanelType.Enemy); } }
        public float ProcesssingTimeOfOpenPanel { get { return m_waitTime; } }
        /// <summary>範囲指定をする最小値のvector</summary>
        public Vector3 EnableMinimumPosition { get { return enableMinimumPosition; } }
        /// <summary>範囲指定をする最大値のvector</summary>
        public Vector3 EnableMaximumPosition { get { return enableMaximumPosition; } }
        /// <summary>Scene上に存在する敵以外のパネルが全てオープンされていたらTrueを返す</summary>
        public bool IsOpenedAllPanelsExceptEnemyPanels
        {
            get
            {
                var allPanelsExceptEnemyPanels = PanelsInTheScene.Count(panel => panel.MyPanelType != PanelType.Enemy);
                var opendPanels = PanelsInTheScene.Count(panel => panel.IsOpened && panel.MyPanelType != PanelType.Enemy);
                return opendPanels == allPanelsExceptEnemyPanels;
            }
        }
        public float PanelAnimTime
        {
            get
            {
                return m_waitTime;
            }
            set
            {
                m_waitTime = value;
            }
        }

        /// <summary>パネル枠。全てのパネルの親にする。</summary>
        [SerializeField] GameObject m_panelFrame;
        /// <summary>パネルを回転させる時間</summary>
        [SerializeField] float m_waitTime;
        /// <summary>ShufflePanelsの参照</summary>
        [SerializeField] ShufflePanels m_shufflePanels;


        /// <summary>TouchGestureDetectorの参照</summary>
        TouchGestureDetector m_touchGestureDetector;
        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;
        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;
        /// <summary>オープン後のパネル</summary>
        List<Panel> m_panelsAfterOpened;
        /// <summary>各パネルの生成座標</summary>
        List<Vector3> m_panelPositions;
        /// <summary>パネルのゲームオブジェクト</summary>
        GameObject m_panelPrefab;
        /// <summary>パネル座標の行列</summary>
        float[][] m_panelPosMatlix;

        /// <summary>１パネル枠に存在するパネル数(3*3)</summary>
        const int MatrixValueInGroup = 3;
        /// <summary>Panel grounpの総数</summary>
        const int groupMembers = 3;
        /// <summary>each Panels Spacing on group position</summary>
        const float eachPanelsSpacingOnGroup = 230f;
        /// <summary>x axis of local position of each spacing on group</summary>
        const float eachSpacingOnGroupPosition = 733.3f;
        /// <summary>local position of first instantiate panel</summary>
        readonly Vector2 initialPanelLocalPosition = new Vector2(-963f, 232.5f);
        /// <summary>Colliderを有効にする座標の最小値</summary>
        readonly Vector3 enableMinimumPosition = new Vector3(-0.85f, -5.5f, 0.0f);
        /// <summary>Colliderを有効にする座標の最大値</summary>
        readonly Vector3 enableMaximumPosition = new Vector3(2.6f, -2.4f, 0.0f);

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_touchGestureDetector = TouchGestureDetector.Instance; // shingleton,TouchGestureDetectorインスタンスの取得
            m_battleManager = BattleManager.Instance; // shingleton,BattleManagerインスタンスの取得
            m_panelCounter = PanelCounter.Instance; // PanelCounterの参照取得
            m_panelPrefab = Resources.Load<GameObject>("Panel"); //PanelをResourcesフォルダに入れてシーン外から取得
            m_panelPositions = new List<Vector3>();
            m_panelsAfterOpened = new List<Panel>();
        }


        /// <summary>
        /// パネルのメインの処理を登録する
        /// </summary>
        void Start()
        {
            InitPanels(); // パネルをセットする

            // タッチによる任意の処理をイベントに登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (m_battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.PlayerChoice || m_isPanelProcessing) // プレイヤーのターンじゃない or パネルが処理中なら処理終了  || m_battleDebugger.DebugFlag
                {
                    return;
                }

                if (gesture == TouchGestureDetector.Gesture.Click) // タップ時
                {
                    GameObject hitResult; // Raycastの結果を入れる変数

                    if (touchInfo.HitDetection(out hitResult)) // タッチしたオブジェクトのタグがnullじゃないなら
                    {
                        ProcessingFactory(hitResult); // 結果内容を判別し結果に応じて処理を自動的に行わせる
                    }
                }
            });
        }

        /// <summary>
        /// タッチしたゲームオブジェクトのタグに応じて処理を行う
        /// </summary>
        /// <param name="hitResult"></param>
        public void ProcessingFactory(GameObject hitResult)
        {
            switch (hitResult.tag)
            {
                case "Panel":
                    var panel = hitResult.GetComponent<Panel>();
                    if (panel.IsOpened == true || !panel.CheckActivatablePanel() || IsOpenedAllPanelsExceptEnemyPanels) // 既に開かれているパネルなら終了
                    {
                        return;
                    }
                    PanelProcessing(panel);
                    break;
            }
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            Debug.Log("on initialize");
        }

        /// <summary>
        /// Panelをタッチした時の処理(処理に掛ける時間を返す)
        /// </summary>
        /// <param name="panel"></param>
        public float PanelProcessing(Panel panel)
        {
            m_isPanelProcessing = true; // フラグを建てる
            panel.Open(m_waitTime); // panelを開く
            m_panelsAfterOpened.Add(panel); // 開いたオブジェクトを登録
            StartCoroutine(PanelWait(panel)); // パネル処理時止める。PanelCounterにパネルを渡す為に引数に入れる
            return m_waitTime;
        }

        /// <summary>
        /// パネルを初期化する
        /// </summary>
        public void InitPanels()
        {
            if (PanelsInTheScene != null) // パネルリストが既に存在していたら
            {
                PanelType[] panelAllocations = GetRandomPanels(); // 新たにGuidインスタンスを使ってランダム配列生成。
                int count = 0; // panelTypeの要素を順に充てていく為のカウント
                PanelsInTheScene.ForEach((obj) => // パネルを全てリセット。
                {
                    Panel panel = obj.GetComponent<Panel>(); // 各パネルのPanelコンポーネントの参照
                    panel.ResetPanel(); // パネルをオープン前の状態に戻す
                    panel.MyPanelType = panelAllocations[count]; // ランダムにソートしたpanelTypeを充てていく。
                    count++; // count up.
                });
            }
            else // パネルリストが生成されていなかったら
            {
                PanelsInTheScene = new List<Panel>(); // パネルを開く前のリスト
                PanelType[] panelAllocations = GetRandomPanels(); // パネルの数分PanelTypeのenum値を適切にランダム配分させたリスト
                Vector3 pos;
                int totalIndex = 0;

                for (int groupIndex = 0; groupIndex < groupMembers; groupIndex++)
                {
                    for (int indexXAxis = 0; indexXAxis < MatrixValueInGroup; indexXAxis++)
                    {
                        for (int indexYAxis = 0; indexYAxis < MatrixValueInGroup; indexYAxis++)
                        {
                            pos = new Vector3(
                            initialPanelLocalPosition.x + (eachPanelsSpacingOnGroup * indexXAxis) + (eachSpacingOnGroupPosition * groupIndex),
                            initialPanelLocalPosition.y - (eachPanelsSpacingOnGroup * indexYAxis),
                            0f);
                            var panelObject = Instantiate(m_panelPrefab, m_panelFrame.transform);
                            panelObject.transform.localPosition = pos;
                            Panel panel = panelObject.GetComponent<Panel>(); // ゲームオブジェクトにアタッチされているパネルコンポーネントの参照を代入
                            panel.MyPanelType = panelAllocations[totalIndex]; // パネルタイプを割り当てる
                            panel.MyFramePosition = DetectFramePosition(panel); // パネルの位置を特定して代入
                            PanelsInTheScene.Add(panel);
                            totalIndex++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 配列のindex値からパネルの位置を特定して位置に応じたenumを返す
        /// </summary>
        /// <param name="index">パネルの配列要素位置</param>
        /// <returns>パネルの位置</returns>
        private FramePosition DetectFramePosition(Panel panel)
        {
            var isLessThan = panel.transform.position.x < EnableMinimumPosition.x;
            var isGreaterThan = panel.transform.position.x > EnableMaximumPosition.x;

            if (isLessThan)
            {
                return FramePosition.Left;
            }
            else if(!isLessThan && !isGreaterThan)
            {
                return FramePosition.Center;
            }
            else 
            {
                return FramePosition.Right;
            }
        }

        /// <summary>
        /// Gets the random panels.
        /// </summary>
        /// <returns>The random panels.</returns>
        public PanelType[] GetRandomPanels()
        {
            PanelType[] panelType = new PanelType[27];

            //Debug.Log("elements.ToArray().Length is : " + m_panelTypes.Length);
            for (int i = 0; i < panelType.ToArray().Length; i++)
            {
                if (i <= 1) // 2個type Enemyにする
                {
                    panelType[i] = PanelType.Enemy;
                }
                else if (i <= 4) // 3個type TripleCityにする
                {
                    panelType[i] = PanelType.TripleCity;
                }
                else if (i <= 11) // 7個type Dounbleにする
                {
                    panelType[i] = PanelType.DoubleCity;
                }
                else // 残り全てtype Cityにする
                {
                    panelType[i] = PanelType.City;
                }
            }
            PanelType[] resultArray = panelType.OrderBy(i => Guid.NewGuid()).ToArray(); // Guid配列に変換、OrderByでアルファベット順に並び替える
            return resultArray; // ソート結果を返す
        }

        /// <summary>
        /// Panels the wait.
        /// </summary>
        /// <returns>The wait.</returns>
        IEnumerator PanelWait(Panel panel)
        {
            yield return new WaitForSeconds(m_waitTime); // 指定時間遅延させる。
            m_panelCounter.PanelJudger(panel); // パネルタイプを判定して対応した処理に導く
            m_isPanelProcessing = false; // フラグを消す
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetOpenedPanels(Collider2D[] panels)
        {
            var panelList = new List<Panel>(); // colliderに検出されたオブジェクトのPanelの参照リスト
            var panelTypes = new List<PanelType>(); // colliderに検出されたパネルのPanelTypeをリストで格納

            foreach (var panel in panels) // colliderに検出されたパネルとそのPanelTypeを全てリストに格納する
            {
                var panelObject = panel.gameObject.GetComponent<Panel>(); // Panelの参照取得
                panelList.Add(panelObject); // Panelをリストに追加
                panelTypes.Add(panelObject.MyPanelType); // PanelTypeをリストに追加
            }
            var result = panelTypes.OrderBy((arg1) => Guid.NewGuid()).ToArray(); // Guid配列に変換、OrderByでアルファベット順に並び替える
            var count = 0; // ForEachに使うresult配列の要素指定用のカウンター

            panelList.ForEach((panel) => // リストに格納した各パネルにGuidでランダム化したPanelTypeを順番に代入の後パネルを引いていない状態に戻す
            {
                panel.MyPanelType = result[count]; // PanelTypeの代入
                panel.ResetPanel(); // パネルを引いていない状態に戻す
                count++; // カウントアップ
            });
        }

        /// <summary>
        /// <param name="targetVec">Target vec.</param>
        /// が指定した範囲内にいる時<returns><c>true</c>
        /// そうでない時<c>false</c></returns>
        /// </summary>
        /// <param name="min">最小値のvector</param>
        /// <param name="max">最大値のvector</param>
        private bool IsWithinRange(Vector2 targetVec, Vector2 min, Vector2 max)
        {
            if (targetVec.x >= min.x && targetVec.x < max.x
            && targetVec.y >= min.y && targetVec.y < max.y)
            {
                return true;
            }
            return false;
        }
    }
}
