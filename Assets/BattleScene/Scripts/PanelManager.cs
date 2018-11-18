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
    public class PanelManager : MonoBehaviour
    {
        [Flags]
        enum Flag
        {
            IsPanelProcessing = 1,
            dummy1 = 2,
            dummy2 = 4,
            dummy3 = 8
        }

        /// <summary>パネル枠。全てのパネルの親にする。</summary>
        [SerializeField] GameObject m_panelFrame;

        /// <summary>TouchGestureDetectorの参照</summary>
        TouchGestureDetector m_touchGestureDetector;
        /// <summary>パネル</summary>
        List<GameObject> m_panel;
        /// <summary>オープン前のパネル</summary>
        List<GameObject> m_panelsBforeOpening;
        /// <summary>オープン後のパネル</summary>
        List<GameObject> m_panelsAfterOpened;
        /// <summary>各パネルの生成座標</summary>
        List<Vector3> m_panelPositions;
        /// <summary>パネルのゲームオブジェクト</summary>
        GameObject m_panelPrefab;
        /// <summary>パネル座標の行列</summary>
        float[][] m_panelPosMatlix;
        /// <summary>パネルが処理中かどうか表すフラグ</summary>
        bool m_isPanelProcessing;

        void Awake()
        {
            Initialize();
            // shingleton,TouchGestureDetectorインスタンスの取得
            m_touchGestureDetector = TouchGestureDetector.Instance;
        }

        void Start()
        {
            SetPanels(); // パネルをセットする

            // タッチによる任意の処理をイベントに登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (m_isPanelProcessing) //パネルが処理中なら処理終了
                {
                    return;
                }
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.Click: // クリックジェスチャをした時

                        GameObject hitResult;
                        touchInfo.HitDetection(out hitResult);
                        if (hitResult != null && hitResult.tag == "Panel") // タッチしたオブジェクトのタグがパネルなら
                        {
                            var panel = hitResult.GetComponent<Panel>(); // タッチされたパネルのPanelクラスの参照
                            panel.Open(); // panelを開く
                            m_panelsAfterOpened.Add(hitResult); // 開いたオブジェクトを登録
                            //m_panelsBforeOpening.RemoveAll((obj) => obj == hitResult); //生成時に登録したリストに開けたパネルがあればリストから削除(現状使わないけどひとまずコメントアウトで退避
                        }
                        break;
                }
            });
        }

        void Initialize()
        {
            m_panelPrefab = Resources.Load<GameObject>("Battle_Panel"); //Battle_PanelをResourcesフォルダに入れてシーン外から取得
            m_panelPosMatlix = new float[2][]; // パネル座標のジャグ配列
            m_panelPosMatlix[0] = new[] { -2.43f, -3.63f, -4.83f }; //列
            m_panelPosMatlix[1] = new[] { -5f, -3.8f, -2.6f, -1.2f, 0f, 1.2f, 2.6f, 3.8f, 5f }; //行
            m_panelPositions = new List<Vector3>();

            for (int i = 0; i < m_panelPosMatlix[0].Length; i++) //列のfor文。行×列=27個のパネル座標を追加する
            {
                for (int j = 0; j < m_panelPosMatlix[1].Length; j++) // 行のfor文
                {
                    m_panelPositions.Add(new Vector3(m_panelPosMatlix[1][j], m_panelPosMatlix[0][i], 0f)); //リストに追加
                }
            }
        }

        public List<GameObject> Shuffle()
        {
            if (m_panel == null) // パネルリストが生成されていなかったら
            {
                PanelType[] panelAllocations = GetRandomPanels(); // パネルの数分PanelTypeのenum値を適切にランダム配分させたリスト
                m_panel = new List<GameObject>();
                //パネルを生成後PanelTypeを適切に割り振る
                //m_panelAllocationsとm_panelPositionsの要素数は一緒になっていなければおかしいので同時に条件分岐をとる
                for (int i = 0; i < panelAllocations.Length && i < m_panelPositions.Count; i++)
                {
                    //PanelPrefabのインスタンスを生成して、そのゲームオブジェクトの参照を代入する
                    GameObject panelObject = Instantiate(m_panelPrefab, m_panelPositions[i], Quaternion.identity, m_panelFrame.transform);
                    Panel panel = panelObject.GetComponent<Panel>(); // ゲームオブジェクトにアタッチされているパネルコンポーネントの参照を代入
                    panel.m_panelType = panelAllocations[i]; // パネルの種類をここで決めてもらう
                    m_panel.Add(panelObject); // パネルをリストに入れる
                }
            }
            return m_panel; // PanelTypeをランダム分配したリストを返す
        }

        /// <summary>バトルシーンの全てのパネル種類の配列</summary>
        public PanelType[] GetRandomPanels()
        {
            PanelType[] panelTypes = new PanelType[27];

            //Debug.Log("elements.ToArray().Length is : " + m_panelTypes.Length);
            for (int i = 0; i < panelTypes.ToArray().Length; i++)
            {
                if (i <= 1)
                {
                    panelTypes[i] = PanelType.Enemy;
                }
                else if (i <= 4)
                {
                    panelTypes[i] = PanelType.TripleCity;
                }
                else if (i <= 10)
                {
                    panelTypes[i] = PanelType.DoubleCity;
                }
                else
                {
                    panelTypes[i] = PanelType.City;
                }
            }
            PanelType[] array = panelTypes;
            PanelType[] resultArray = array.OrderBy(i => Guid.NewGuid()).ToArray();//ラムダ式。配列を変換して？シャッフルして配列に戻し代入
            panelTypes = resultArray;
            return panelTypes;
        }

        /// <summary>
        /// Sets the panels.
        /// </summary>
        public void SetPanels()
        {
            m_panelsBforeOpening = Shuffle(); // パネル生成処理
        }

        /// <summary>
        /// Resets the opened panels.
        /// </summary>
        public void ResetOpenedPanels()
        {
            m_panelsAfterOpened.ForEach((obj) => // 開いたパネルをシャッフルして開く前に戻す
            {
                Panel panel = obj.GetComponent<Panel>(); // パネルコンポーネント参照取得
                panel.ResetFlag(); // パネルをリセット
            });
        }

    }
}
