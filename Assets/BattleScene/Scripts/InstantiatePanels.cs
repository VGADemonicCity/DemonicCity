using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity.BattleScene
{
    /// <summary>パネルを生成してPanelTypeの割り当てを行うクラス</summary>
    public class InstantiatePanels : MonoBehaviour
    {
        /// <summary>各パネルの生成座標</summary>
        private List<Vector3> m_panelPositions;
        /// <summary>パネルのゲームオブジェクト</summary>
        private GameObject m_panelPrefab;
        /// <summary>PanelTypeの要素数 : Element count of PanelType</summary>
        private int m_elementCount;
        private Panel m_panel;
        /// <summary>インスタンス生成したパネルの参照</summary>
        private GameObject m_panelObject;
        /// <summary>パネル座標の行列</summary>
        private float[][] m_panelPosMatlix;
        /// <summary>パネルの数分PanelTypeのenum値を適切にランダム配分させたリスト</summary>
        private PanelType[] m_panelAllocations;
        private InitializePanels m_initializePanels;
        /// <summary>パネル全体の枠であり親。</summary>
        private GameObject m_panelFrame;

        private void Initialize()
        {

            m_panelPosMatlix = new float[2][];
            m_panelPosMatlix[0] = new[] { -2.43f, -3.63f, -4.83f }; //列
            m_panelPosMatlix[1] = new[] { -5f, -3.8f, -2.6f, -1.2f, 0f, 1.2f, 2.6f, 3.8f, 5f };　//行
            m_panelPositions = new List<Vector3>();
            m_initializePanels = GetComponent<InitializePanels>();
            m_panelFrame = GameObject.Find("PanelFrame");
            



            for (int i = 0; i < m_panelPosMatlix[0].Length; i++) //行×列=27個のパネル座標を追加する
            {
                for (int j = 0; j < m_panelPosMatlix[1].Length; j++)
                {
                    m_panelPositions.Add(new Vector3(m_panelPosMatlix[1][j], m_panelPosMatlix[0][i], 0f));
                }
            }

            m_panelPrefab = Resources.Load<GameObject>("Battle_Panel"); //Battle_PanelをResourcesフォルダに入れてシーン外から取得

        }

        private void Awake()
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
            m_panelAllocations = m_initializePanels.GetRandomPanels();

            //パネルを生成後PanelTypeを適切に割り振る
            //m_panelAllocationsとm_panelPositionsの要素数は一緒になっていなければおかしいので同時に条件分岐をとる
            for (int i = 0; i < m_panelAllocations.Length && i < m_panelPositions.Count; i++)
            {
                m_panelObject = Instantiate(m_panelPrefab, m_panelPositions[i], Quaternion.identity,m_panelFrame.transform);
                m_panel = m_panelObject.GetComponent<Panel>();
                m_panel.m_panelType = m_panelAllocations[i];
            }
        }
    }
}
