using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace DemonicCity.BattleScene.Debugger
{
    public class BattleDebugger : MonoSingleton<BattleDebugger>
    {
        /// <summary></summary>
        PanelManager m_panelManager;
        /// <summary></summary>
        BattleManager m_battleManager;
        /// <summary></summary>
        [Header("エフェクトをスキップする")]
        [SerializeField] bool effectSkip;
        /// <summary></summary>
        [Header("任意の枚数自動でパネルを選択させる")]
        [SerializeField] bool autoPlay;
        /// <summary>パネルを表示させる</summary>
        [Header("開かれていないパネルを全て可視化する(開かれてはいない)")]
        [SerializeField] bool displayPanels;
        /// <summary>パネルを開く枚数</summary>
        [Header("ボタンを押した時に開くパネルの枚数(ランダム)")]
        [Range(0, 25)]
        [SerializeField] int openPanelQuantity;
        /// <summary>バトル中のマギアのステータス</summary>
        [SerializeField] Status magiaStatus;
        /// <summary>バトル中の敵のステータス</summary>
        [SerializeField] Status currentEnemyStatus;
        /// <summary>Debug用フラグのバッキングフィールド</summary>
        [SerializeField] DebuggingFlag flag;

        /// <summary>Debug用フラグ</summary>
        public DebuggingFlag Flag
        {
            get
            {
                //// 各々フラグが立っていたらflagに論理和で追加,立っていない場合排斥
                //// effect skip
                //if (effectSkip)
                //{
                //    flag |= DebuggingFlag.SkipEffect;
                //}
                //else if (DebuggingFlag.SkipEffect == (flag & DebuggingFlag.SkipEffect))
                //{
                //    flag ^= DebuggingFlag.SkipEffect;
                //}
                //// auto play
                //if (autoPlay)
                //{
                //    flag |= DebuggingFlag.AutoPlay;
                //}
                //else if (DebuggingFlag.AutoPlay == (flag & DebuggingFlag.AutoPlay))
                //{
                //    flag ^= DebuggingFlag.AutoPlay;
                //}

                //// display panels
                //if (displayPanels)
                //{
                //    flag |= DebuggingFlag.DisplayPanels;
                //}
                //else if (DebuggingFlag.DisplayPanels == (flag & DebuggingFlag.DisplayPanels))
                //{
                //    flag ^= DebuggingFlag.DisplayPanels;
                //}
                return flag;
            }
            set
            {
                flag = value;
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
                if (state != BattleManager.StateMachine.State.PlayerChoice || DebuggingFlag.AutoPlay != (Flag & DebuggingFlag.AutoPlay)) // PlayerChoice以外,debugフラグがオフの時は何もしない
                {
                    return;
                }
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
        
        public enum UpdateStatusesTag
        {
            MagiaAtk,
            MagiaDef,
            MagiaHp,
            EnemyAtk,
            Reset,
        }

        [Flags]
        public enum DebuggingFlag
        {
            AutoPlay = 1,
            SkipEffect = 2,
            DisplayPanels = 4,
        }
    }
}
