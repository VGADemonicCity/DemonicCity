using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 固有スキル : 魔眼
    /// </summary>
    public class DevilEye :  MonoBehaviour, IUniqueSkillActivatable
{
        /// <summary>panelを開けるのに掛ける時間</summary>
        [SerializeField] float m_processingTime = 3f;

        /// <summary>PanelManagerの参照</summary>
        PanelManager m_panelManager;
        /// <summary>PanelFrameManagerの参照</summary>
        PanelFrameManager m_panelFrameManager;
        

        private void Awake()
        {
            m_panelManager = PanelManager.Instance; // 参照取得
            m_panelFrameManager = PanelFrameManager.Instance; // 参照取得
        }

        /// <summary>
        /// 魔眼発動
        /// </summary>
        public void Activate()
        {
            var enemyPanel = m_panelManager.m_panelsBforeOpen.Find((panel) => panel.MyPanelType == PanelType.Enemy); // パネル枠の中から敵パネルを取得
            var panelIndex = m_panelManager.m_panelsBforeOpen.FindIndex((panel) => panel.MyPanelType == PanelType.Enemy); // パネルのインデックスを取得
            enemyPanel.Open(m_processingTime);
            m_panelFrameManager.StartCoroutine(m_panelFrameManager.MovingFrame(enemyPanel.MyFramePosition)); // 敵パネルの位置情報の場所にパネルフレームを移動させる


            Debug.Log("Activated Devil eye.");
        }
    }
}
