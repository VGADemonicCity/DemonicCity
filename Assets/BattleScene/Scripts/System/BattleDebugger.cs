using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonicCity.BattleScene;

namespace DemonicCity.Debugger
{
    public class BattleDebugger : MonoSingleton<BattleDebugger>
    {
        /// <summary></summary>
        PanelManager m_panelManager;
        /// <summary></summary>
        BattleManager m_battleManager;
        /// <summary></summary>
        [SerializeField] bool m_debugFlag;


        private void Awake()
        {
            /// <summary></summary>
            m_panelManager = PanelManager.Instance;
            /// <summary></summary>
            m_battleManager = BattleManager.Instance;
        }


    }
}
