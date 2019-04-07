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
        /// <summary>封印された敵パネル</summary>
        [SerializeField] Sprite sealdEnemyPanel;

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
            var enemyPanel = m_panelManager.PanelsInTheScene.Find((panel) => panel.MyPanelType == PanelType.Enemy); // パネル枠の中から敵パネルを取得
            enemyPanel.Open(m_processingTime, sealdEnemyPanel);// スキル発動時専用の敵パネルのスプライトを渡してそれに変える
            m_panelFrameManager.StartCoroutine(m_panelFrameManager.MovingFrame(enemyPanel.MyFramePosition)); // 敵パネルの位置情報の場所にパネルフレームを移動させる
            enemyPanel.IsOpened = true;


            Debug.Log("Activated Devil eye.");
        }
    }
}
