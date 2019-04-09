﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace DemonicCity.BattleScene.Debugger
{
    public class BattleDebugger : MonoSingleton<BattleDebugger>
    {
        /// <summary></summary>
        public bool AutoPlay { get { return DebuggingFlag.AutoPlay == (Flag & DebuggingFlag.AutoPlay); } }
        /// <summary></summary>
        public bool DisplayPanels { get { return DebuggingFlag.DisplayPanels == (Flag & DebuggingFlag.DisplayPanels ); } }
        /// <summary></summary>
        public bool EffectSkip { get { return DebuggingFlag.SkipEffect == (Flag & DebuggingFlag.SkipEffect); } }


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
        /// <summary>マギアのステータスをEditorから読み込むかどうか</summary>
        [Header("これがTrueの時はTargetMagiaLevelから適切なマギアのステータスを読み込む")]
        [SerializeField] public bool LoadStatusFromInspector;
        /// <summary>Editor上からマギアのステータスを設定する時のクラス</summary>
        [Range(0, 200)]
        [SerializeField] public int TargetMagiaLevel;
        /// <summary>基礎ステータスに追加したいステータスがある場合true</summary>
        [Header("このフラグがTrueの時にここのStatusを追加し,固有ステータスも基礎ステータスに反映させる")]
        [SerializeField] bool addStatus;
        /// <summary>追加したいStatus</summary>
        [SerializeField] Status additionalStatus;

        /// <summary>固有ステータスから基礎ステータスに変換する際のHPの係数</summary>
        [Header("固有ステータスから基礎ステータスに変換する際のHPの係数")]
        [SerializeField] int hpFactor = 50;
        /// <summary>固有ステータスから基礎ステータスに変換する際のAttackの係数</summary>
        [Header("固有ステータスから基礎ステータスに変換する際のAttackの係数")]
        [SerializeField] int attackFactor = 5;
        /// <summary>固有ステータスから基礎ステータスに変換する際のDefenseの係数</summary>
        [Header("固有ステータスから基礎ステータスに変換する際のDefenseの係数")]
        [SerializeField] int defenseFactor = 5;

        /// <summary></summary>
        PanelManager m_panelManager;
        /// <summary></summary>
        BattleManager m_battleManager;

        /// <summary>Debug用フラグ</summary>
        public DebuggingFlag Flag
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;
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
            if (DisplayPanels && m_battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Init)
            {
                var panelManager = PanelManager.Instance;
                if (DisplayPanels)
                {
                    panelManager.PanelsInTheScene.ForEach(panel =>
                    {
                        panel.ChangingTexture();
                    });
                }
                else
                {
                    panelManager.AllPanelsExceptEnemyPanels.ForEach(panel =>
                    {
                        panel.ResetPanel();
                    });
                }
            }
        }
#endif

        private void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.PlayerChoice)
                {
                    return;
                }

                magiaStatus = m_battleManager.m_MagiaStats;
                currentEnemyStatus = m_battleManager.CurrentEnemy.Stats;

                if (AutoPlay) // PlayerChoice以外,debugフラグがオフの時は何もしない
                {
                    // 設定された枚数パネルを引いた後敵パネルを引く
                    OpenAllPanelsExceptEnemyPanels();
                    var enemyPanel = m_panelManager.PanelsInTheScene.Find(panel => panel.MyPanelType == PanelType.Enemy);
                    m_panelManager.PanelProcessing(enemyPanel);
                }




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

        /// <summary>
        /// Debug用のステータス取得
        /// </summary>
        /// <returns></returns>
        public Status GetStats()
        {
            {
                Status stats = new Status()
                {
                    Level = 1,
                    HitPoint = 1000,
                    Attack = 100,
                    Defense = 100,
                    Charm = 0,
                    Sense = 0,
                    Dignity = 0,
                    Knowledge = 0,
                    Durability = 0,
                    MuscularStrength = 0,
                };

                for (int level = 1; level < TargetMagiaLevel; level++)
                {
                    LevelUp(stats);
                }
                if (addStatus)
                {
                    stats += additionalStatus;
                }
                Sync(stats);
                return stats;
            }
        }

        /// <summary>
        /// Debug用レベルアップ
        /// </summary>
        /// <param name="stats"></param>
        void LevelUp(Status stats)
        {
            // レベルアップする直前のレベルに合わせてステータスを上昇させる
            if (stats.Level < 50) // レベル50以下なら
            {
                stats.HitPoint += 50;
                stats.Attack += 15;
                stats.Defense += 15;
            }
            else if (stats.Level >= 50 && stats.Level < 100) // レベル50~99なら
            {
                stats.HitPoint += 25;
                stats.Attack += 10;
                stats.Defense += 10;
            }
            else if (stats.Level >= 100 && stats.Level < 150) // レベル100~149なら
            {
                stats.HitPoint += 10;
                stats.Attack += 5;
                stats.Defense += 5;
            }
            else if (stats.Level >= 150 && stats.Level < 200) // レベル150~199なら
            {
                stats.HitPoint += 5;
                stats.Attack += 1;
                stats.Defense += 1;
            }
            stats.Level++; // levelを1上げる
            return;
        }

        /// <summary>
        /// 強化画面で編集したStatsをmagiaにセットし、固有ステータスを基礎ステータスに反映させる
        /// </summary>
        void Sync(Status stats)
        {
            stats.Attack = stats.Attack + (stats.Sense * attackFactor); // センスを攻撃力に変換
            stats.Attack = stats.Attack + (stats.MuscularStrength * attackFactor); // 筋力を攻撃力に変換
            stats.Defense = stats.Defense + (stats.Durability * defenseFactor); // 耐久力を防御力に変換
            stats.Defense = stats.Defense + (stats.Knowledge * defenseFactor); // 知識を防御力に変換
            stats.HitPoint = stats.HitPoint + (stats.Charm * hpFactor); // 魅力をHPに変換
            stats.HitPoint = stats.HitPoint + (stats.Dignity * hpFactor); // 威厳をHPに変換
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
