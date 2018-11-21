using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        Flag m_flag = 0;


        void Awake()
        {
            Initialize(); // 各要素の初期化
            m_touchGestureDetector = TouchGestureDetector.Instance; // shingleton,TouchGestureDetectorインスタンスの取得
        }



        /// <summary>
        /// --------------------------
        /// パネルのメインの処理を登録する
        /// --------------------------
        /// </summary>
        void Start()
        {
            m_battleManager = BattleManager.Instance; // shingleton,BattleManagerインスタンスの取得
            SetPanels(); // パネルをセットする

            // タッチによる任意の処理をイベントに登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (m_battleManager.m_states != BattleManager.States.PlayerChoice || (m_flag & Flag.IsPanelProcessing) == Flag.IsPanelProcessing) // プレイヤーのターンじゃない or パネルが処理中なら処理終了
                {
                    return;
                }

                if(gesture == TouchGestureDetector.Gesture.Click) // タップ時
                {
                    GameObject hitResult; // Raycastの結果を入れる変数
                    touchInfo.HitDetection(out hitResult); // レイキャストしてゲームオブジェクトをとってくる
                    if (hitResult != null && hitResult.tag == "Panel") // タッチしたオブジェクトのタグがパネルなら
                    {
                        m_flag = m_flag | Flag.IsPanelProcessing; // 論理和,XORしてフラグを消す
                        var panel = hitResult.GetComponent<Panel>(); // タッチされたパネルのPanelクラスの参照
                        panel.Open(m_waitTime); // panelを開く
                        m_panelsAfterOpened.Add(hitResult); // 開いたオブジェクトを登録
                        StartCoroutine(PanelWait(panel)); // パネル処理時止める。PanelCounterにパネルを渡す為に引数に入れる
                    }
                }
                if(gesture == TouchGestureDetector.Gesture.FlickBottomToTop) // Debug用
                {
                    SetPanels(); // Debug用
                }
            });
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        void Initialize()
        {
            m_panelPrefab = Resources.Load<GameObject>("Battle_Panel"); //Battle_PanelをResourcesフォルダに入れてシーン外から取得
            m_panelPosMatlix = new float[2][]; // パネル座標のジャグ配列
            m_panelPosMatlix[0] = new[] { -2.43f, -3.63f, -4.83f }; //列
            m_panelPosMatlix[1] = new[] { -5f, -3.8f, -2.6f, -1.2f, 0f, 1.2f, 2.6f, 3.8f, 5f }; //行
            m_panelPositions = new List<Vector3>();
            m_panelsAfterOpened = new List<GameObject>();
            m_panelCounter = GetComponent<PanelCounter>();


            for (int i = 0; i < m_panelPosMatlix[0].Length; i++) // 列のfor文。行×列=27個のパネル座標を追加する
            {
                for (int j = 0; j < m_panelPosMatlix[1].Length; j++) // 行のfor文
                {
                    m_panelPositions.Add(new Vector3(m_panelPosMatlix[1][j], m_panelPosMatlix[0][i], 0f)); // リストに追加
                }
            }
        }

        /// <summary>
        /// パネルをセットする
        /// </summary>
        public void SetPanels()
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
                //パネルを生成後PanelTypeを適切に割り振る
                //m_panelAllocationsとm_panelPositionsの要素数は一緒になっていなければおかしいので同時に条件分岐をとる
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
            PanelType[] panelsType = new PanelType[27];

            //Debug.Log("elements.ToArray().Length is : " + m_panelTypes.Length);
            for (int i = 0; i < panelsType.ToArray().Length; i++)
            {
                if (i <= 1) // 2個type Enemyにする
                {
                    panelsType[i] = PanelType.Enemy;
                }
                else if (i <= 4) // 3個type TripleCityにする
                {
                    panelsType[i] = PanelType.TripleCity;
                }
                else if(i<= 11) // 7個type Dounbleにする
                {
                    panelsType[i] = PanelType.DoubleCity;
                }
                else // 残り全てtype Cityにする
                {
                    panelsType[i] = PanelType.City;
                }

            }
            PanelType[] array = panelsType; // 変換用配列を定義
            PanelType[] resultArray = array.OrderBy(i => Guid.NewGuid()).ToArray();//ラムダ式でGuid配列に変換、OrderByでアルファベット順に並び替える
            panelsType = resultArray; // paneslTypeに代入
            return panelsType; // ソート結果を返す
        }

        /// <summary>
        /// Panels the wait.
        /// </summary>
        /// <returns>The wait.</returns>
        IEnumerator PanelWait(Panel panel)
        {
            yield return new WaitForSeconds(m_waitTime); // 指定時間遅延させる。
            m_panelCounter.PanelJudger(panel); // パネルタイプを判定して対応した処理に導く
            m_flag = m_flag ^ Flag.IsPanelProcessing; // 排他的論理和,XORしてフラグを消す
        }

        ///// <summary>
        ///// Resets the opened panels.
        ///// </summary>
        //public void ResetOpenedPanels()
        //{
        //    m_panelsAfterOpened.ForEach((obj) => // 開いたパネルをシャッフルして開く前に戻す
        //    {
        //        Panel panel = obj.GetComponent<Panel>(); // パネルコンポーネント参照取得
        //        panel.ResetPanel(); // パネルをリセット
        //    });
        //}
    }
}
