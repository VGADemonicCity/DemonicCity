using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class DevilEye :  MonoBehaviour, IUniqueSkillActivatable
{
        /// <summary>panelを開けるのに掛ける時間</summary>
        [SerializeField] float m_processingTime = 3f;

        PanelManager m_panelManager;
        

        private void Awake()
        {
            m_panelManager = PanelManager.Instance;
        }

        /// <summary>
        /// 魔眼発動
        /// </summary>
        public void Activate()
        {
            var enemyPanel = m_panelManager.m_panelsBforeOpen.Find((panel) => panel.m_panelType == PanelType.Enemy); // パネル枠の中から敵パネルを取得
            enemyPanel.Open(m_processingTime);
            Debug.Log("Activated Devil eye.");
        }
    }
}
