using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonicCity.BattleScene;
using System.Linq;
using System;

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
        /// <summary></summary>
        public bool DebugFlag
        {
            get
            {
                return m_debugFlag;
            }
            private set
            {
                m_debugFlag = value;
            }
        }


        private void Awake()
        {
            m_panelManager = PanelManager.Instance; // PanelManagerの参照取得
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
        }

        private void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if(state != BattleManager.StateMachine.State.PlayerChoice || !m_debugFlag) // PlayerChoice以外,debugフラグがオフの時は何もしない
                {
                    return;
                }
                StartCoroutine(AutoPanelSelector());
            });
        }

        /// <summary>
        /// パネルをランダムに自動選択する
        /// </summary>
        /// <returns></returns>
        IEnumerator AutoPanelSelector()
        {
            var panels = m_panelManager.m_panelsBforeOpen.OrderBy(i => Guid.NewGuid()); // パネルをランダムにソートする

            foreach (var panel in panels)
            {
                m_panelManager.PanelProcessing(panel);
                yield return new WaitUntil(() => panel.m_alreadyProcessed); // パネルが処理中の間は此方の処理を一時停止させる
                if (panel.m_panelType == PanelType.Enemy) // 敵パネルを引いたら処理終了
                {
                    break;
                }
            }
        }
    }
}
