using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// パネルを生成してPanelTypeの割り当てを行うクラス
    /// </summary>
    public class InstantiatePanels : MonoBehaviour
    {
        /// <summary>パネル枠。全てのパネルの親に値する。</summary>
        [SerializeField] GameObject m_panelFrame;

        /// <summary>各パネルの生成座標</summary>
        List<Vector3> m_panelPositions;
        /// <summary>パネルのゲームオブジェクト</summary>
        GameObject m_panelPrefab;
        /// <summary>パネル座標の行列</summary>
        float[][] m_panelPosMatlix;

        void Initialize()
        {
            m_panelPrefab = Resources.Load<GameObject>("Battle_Panel"); //Battle_PanelをResourcesフォルダに入れてシーン外から取得
            m_panelPosMatlix = new float[2][]; // パネル座標のジャグ配列
            m_panelPosMatlix[0] = new[] { -2.43f, -3.63f, -4.83f }; //列
            m_panelPosMatlix[1] = new[] { -5f, -3.8f, -2.6f, -1.2f, 0f, 1.2f, 2.6f, 3.8f, 5f }; //行
            m_panelPositions = new List<Vector3>(); 
            m_panelFrame = GameObject.Find("PanelFrame"); // パネルの枠

            for (int i = 0; i < m_panelPosMatlix[0].Length; i++) //列のfor文。行×列=27個のパネル座標を追加する
            {
                for (int j = 0; j < m_panelPosMatlix[1].Length; j++) // 行のfor文
                {
                    m_panelPositions.Add(new Vector3(m_panelPosMatlix[1][j], m_panelPosMatlix[0][i], 0f)); //リストに追加
                }
            }
        }

        void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// パネル生成メソッド。
        /// ここでPanelTypeの決定を行う。
        /// PanelTypeの要素数に応じたランダムなPanelTypeを各パネルに渡す。
        /// </summary>
        public void GeneratePanels()
        {
            PanelType[] panelAllocations = GetRandomPanels(); // パネルの数分PanelTypeのenum値を適切にランダム配分させたリスト

            //パネルを生成後PanelTypeを適切に割り振る
            //m_panelAllocationsとm_panelPositionsの要素数は一緒になっていなければおかしいので同時に条件分岐をとる
            for (int i = 0; i < panelAllocations.Length && i < m_panelPositions.Count; i++)
            {
                //PanelPrefabのインスタンスを生成して、そのゲームオブジェクトの参照を代入する
                GameObject panelObject = Instantiate(m_panelPrefab, m_panelPositions[i], Quaternion.identity,m_panelFrame.transform); 
                Panel panel = panelObject.GetComponent<Panel>(); // ゲームオブジェクトにアタッチされているパネルコンポーネントの参照を代入
                panel.m_panelType = panelAllocations[i]; // パネルの種類をここで決めてもらう
            }
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
                    panelTypes[i] = PanelType.CityTriple;
                }
                else if (i <= 10)
                {
                    panelTypes[i] = PanelType.CityDouble;
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
    }
}
