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
        [Header("自動でパネルを選択させるか")]
        [SerializeField] bool m_debugFlag;
        /// <summary>パネルを表示させる</summary>
        [Header("開かれていないパネルを全て可視化するか")]
        [SerializeField] bool displayPanels;
        [Header("自動パネル選択設定時のパネルを引き始める直前の遅延")]
        /// <summary></summary>
        [SerializeField] float waitTime = 1f;

        /// <summary>パネルを開く枚数</summary>
        [Header("ボタンを押した時に開くパネルの枚数(ランダム)")]
        [Range(0, 25)]
        [SerializeField] int openPanelQuantity;
        /// <summary>バトル中のマギアのステータス</summary>
        [SerializeField] Status magiaStatus;
        /// <summary>バトル中の敵のステータス</summary>
        [SerializeField] Status currentEnemyStatus;


        /// <summary></summary>
        public bool DebugFlag
        {
            get
            {
                return m_debugFlag;
            }
            set
            {
                m_debugFlag = value;
            }
        }

        private void Update()
        {
            if (m_battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Init)
            {
                magiaStatus = m_battleManager.m_MagiaStats;
                currentEnemyStatus = m_battleManager.CurrentEnemy.Stats;
            }
        }

        private void Awake()
        {
            m_panelManager = PanelManager.Instance; // PanelManagerの参照取得
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
        }

#if UNITY_EDITOR
        /// <summary>
        /// Inspectorのチェックボックスでtrueに設定した時パネルの種類を全て可視化する
        /// </summary>
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            if (displayPanels && m_battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Init)
            {
                PanelManager.Instance.PanelsInTheScene.ForEach(panel =>
                {
                    panel.ChangingTexture();
                });
            }
        }
#endif

        private void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.PlayerChoice || !m_debugFlag) // PlayerChoice以外,debugフラグがオフの時は何もしない
                {
                    return;
                }
                //StartCoroutine(AutoPanelSelector());
                OpenAllPanelsExceptEnemyPanels();
                var enemyPanel = m_panelManager.PanelsInTheScene.Find(panel => panel.MyPanelType == PanelType.Enemy);
                m_panelManager.PanelProcessing(enemyPanel);
            });
        }

        /// <summary>
        /// 敵パネル以外の全てのパネルを開く
        /// </summary>
        public void OpenAllPanelsExceptEnemyPanels()
        {
            if (m_battleManager.m_StateMachine.m_State == BattleManager.StateMachine.State.PlayerChoice)
            {

                var openCount = 0;
                var panels = m_panelManager.PanelsInTheScene.FindAll(panel => panel.MyPanelType != PanelType.Enemy && !panel.IsOpened);
                var orderedPanels = panels.OrderBy(panel => Guid.NewGuid());
                foreach (var panel in orderedPanels)
                {
                    m_panelManager.PanelProcessing(panel);

                    openCount++;
                    if (openCount >= openPanelQuantity)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// パネルをランダムに自動選択する
        /// </summary>
        /// <returns></returns>
        IEnumerator AutoPanelSelector()
        {
            var panels = m_panelManager.PanelsInTheScene.OrderBy(i => Guid.NewGuid()); // パネルをランダムにソートする
            yield return new WaitForSeconds(waitTime);
            foreach (var panel in panels)
            {
                m_panelManager.PanelProcessing(panel);
                yield return new WaitUntil(() => panel.IsOpened); // パネルが処理中の間は此方の処理を一時停止させる
                if (panel.MyPanelType == PanelType.Enemy) // 敵パネルを引いたら処理終了
                {
                    break;
                }
            }
        }
    }
}
