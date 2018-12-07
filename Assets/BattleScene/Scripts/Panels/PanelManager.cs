﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DemonicCity.BattleScene.Skill;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Panel manager.
    /// </summary>
    public class PanelManager : MonoSingleton<PanelManager>
    {
        /// <summary>
        /// Flag.
        /// </summary>
        [Flags]
        enum Flag
        {
            IsPanelProcessing = 1,
            TimeCounter = 2, // never used
            dummy2 = 4,
            dummy3 = 8
        }

        /// <summary>パネル枠。全てのパネルの親にする。</summary>
        [SerializeField] GameObject m_panelFrame;
        /// <summary>パネルを回転させる時間</summary>
        [SerializeField] float m_waitTime = 3f;


        /// <summary>TouchGestureDetectorの参照</summary>
        TouchGestureDetector m_touchGestureDetector;
        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;
        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;
        /// <summary>shufflePanelsの参照</summary>
        ShufflePanels m_shufflePanels;
        /// <summary>オープン前のパネル</summary>
        List<GameObject> m_panelsBforeOpen;
        /// <summary>オープン後のパネル</summary>
        List<GameObject> m_panelsAfterOpened;
        /// <summary>各パネルの生成座標</summary>
        List<Vector3> m_panelPositions;
        /// <summary>パネルのゲームオブジェクト</summary>
        GameObject m_panelPrefab;
        /// <summary>パネル座標の行列</summary>
        float[][] m_panelPosMatlix;
        /// <summary>Flag</summary>
        bool m_isPanelProcessing;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_touchGestureDetector = TouchGestureDetector.Instance; // shingleton,TouchGestureDetectorインスタンスの取得
            m_panelCounter = PanelCounter.Instance; // PanelCounterの参照取得
            m_shufflePanels = GetComponent<ShufflePanels>(); // ShufflePanelsの参照取得
            m_panelPrefab = Resources.Load<GameObject>("Battle_Panel"); //Battle_PanelをResourcesフォルダに入れてシーン外から取得
            m_panelPosMatlix = new float[2][]; // パネル座標のジャグ配列
            m_panelPosMatlix[0] = new[] { -2.43f, -3.63f, -4.83f }; //列
            m_panelPosMatlix[1] = new[] { -4.155f, -2.955f, -1.755f, -0.355f, .845f, 2.045f, 3.445f, 4.645f, 5.845f }; //行
            m_panelPositions = new List<Vector3>();
            m_panelsAfterOpened = new List<GameObject>();

            for (int i = 0; i < m_panelPosMatlix[0].Length; i++) // 列のfor文。行×列=27個のパネル座標を追加する
            {
                for (int j = 0; j < m_panelPosMatlix[1].Length; j++) // 行のfor文
                {
                    m_panelPositions.Add(new Vector3(m_panelPosMatlix[1][j], m_panelPosMatlix[0][i], 0f)); // リストに追加
                }
            }
        }



        /// <summary>
        /// --------------------------
        /// パネルのメインの処理を登録する
        /// --------------------------
        /// </summary>
        void Start()
        {
            m_battleManager = BattleManager.Instance; // shingleton,BattleManagerインスタンスの取得
            InitPanels(); // パネルをセットする

            // タッチによる任意の処理をイベントに登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (m_battleManager.m_stateMachine.m_state != BattleManager.StateMachine.State.PlayerChoice || (m_isPanelProcessing)) // プレイヤーのターンじゃない or パネルが処理中なら処理終了
                {
                    return;
                }

                if (gesture == TouchGestureDetector.Gesture.Click) // タップ時
                {
                    GameObject hitResult; // Raycastの結果を入れる変数
                    touchInfo.HitDetection(out hitResult); // レイキャストしてゲームオブジェクトをとってくる
                    if (hitResult != null && hitResult.tag == "Panel") // タッチしたオブジェクトのタグがパネルなら
                    {
                        m_isPanelProcessing = true; // フラグを建てる
                        var panel = hitResult.GetComponent<Panel>(); // タッチされたパネルのPanelクラスの参照
                        panel.Open(m_waitTime); // panelを開く
                        m_panelsAfterOpened.Add(hitResult); // 開いたオブジェクトを登録
                        StartCoroutine(PanelWait(panel)); // パネル処理時止める。PanelCounterにパネルを渡す為に引数に入れる
                    }
                }
                if (gesture == TouchGestureDetector.Gesture.FlickBottomToTop) // Debug用
                {
                    InitPanels(); // Debug用
                }
                if (gesture == TouchGestureDetector.Gesture.FlickTopToBottom) // Debug用
                {
                    var a = Magia.Instance;
                    a.LevelUp();
                }
            });


        }

        /// <summary>
        /// パネルを初期化する
        /// </summary>
        public void InitPanels()
        {
            if (m_panelsBforeOpen != null) // パネルリストが既に存在していたら
            {
                PanelType[] panelAllocations = GetRandomPanels(); // 新たにGuidインスタンスを使ってランダム配列生成。
                int count = 0; // panelTypeの要素を順に充てていく為のカウント
                m_panelsBforeOpen.ForEach((obj) => // パネルを全てリセット。
                {
                    Panel panel = obj.GetComponent<Panel>(); // 各パネルのPanelコンポーネントの参照
                    panel.ResetPanel(); // パネルをオープン前の状態に戻す
                    panel.m_panelType = panelAllocations[count]; // ランダムにソートしたpanelTypeを充てていく。
                    count++; // count up.
                });
            }
            else // パネルリストが生成されていなかったら
            {
                m_panelsBforeOpen = new List<GameObject>(); // パネルを開く前のリスト
                PanelType[] panelAllocations = GetRandomPanels(); // パネルの数分PanelTypeのenum値を適切にランダム配分させたリスト
                // パネルを生成後PanelTypeを適切に割り振る. m_panelAllocationsとm_panelPositionsの要素数は一緒になっていなければおかしいので同時に条件分岐をとる
                for (int i = 0; i < panelAllocations.Length && i < m_panelPositions.Count; i++)
                {
                    //PanelPrefabのインスタンスを生成して、そのゲームオブジェクトの参照を代入する
                    GameObject panelObject = Instantiate(m_panelPrefab, m_panelPositions[i], Quaternion.identity, m_panelFrame.transform);
                    Panel panel = panelObject.GetComponent<Panel>(); // ゲームオブジェクトにアタッチされているパネルコンポーネントの参照を代入
                    panel.m_panelType = panelAllocations[i]; // パネルの種類をここで決めてもらう
                    m_panelsBforeOpen.Add(panelObject); // パネルをリストに入れる
                }
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
            PanelType[] array = panelType; // 変換用配列を定義
            PanelType[] resultArray = array.OrderBy(i => Guid.NewGuid()).ToArray(); // Guid配列に変換、OrderByでアルファベット順に並び替える
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
                panelTypes.Add(panelObject.m_panelType); // PanelTypeをリストに追加
            }
            var result = panelTypes.OrderBy((arg1) => Guid.NewGuid()).ToArray(); // Guid配列に変換、OrderByでアルファベット順に並び替える
            var count = 0; // ForEachに使うresult配列の要素指定用のカウンター

            panelList.ForEach((panel) => // リストに格納した各パネルにGuidでランダム化したPanelTypeを順番に代入の後パネルを引いていない状態に戻す
            {
                panel.m_panelType = result[count]; // PanelTypeの代入
                panel.ResetPanel(); // パネルを引いていない状態に戻す
                count++; // カウントアップ
            });
        }
    }
}
