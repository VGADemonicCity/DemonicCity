using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Panel counter.
    /// </summary>
    public class PanelCounter : MonoBehaviour
    {
        /// <summary>パネルカウントのカウント変数</summary>
        [SerializeField] int m_CityCount;
        /// <summary>ダブルパネルのカウント変数</summary>
        [SerializeField] int m_doubleCount;
        /// <summary>トリプルパネルのカウント変数</summary>
        [SerializeField] int m_tripleCount;
        /// <summary>BattleManagerのシングルトンインスタンスの参照</summary>
        BattleManager m_battleManager;


        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
        }

        /// <summary>
        /// return the total count.
        /// </summary>
        /// <returns>The total count.</returns>
        public int GetTotalCount()
        {
            return m_CityCount + m_doubleCount + m_tripleCount; // パネルを引いた枚数を返す. 固有スキル用
        }

        /// <summary>
        /// Gets the passive count.
        /// </summary>
        /// <returns>The passive count.</returns>
        public int GetPassiveCount()
        {
            return m_CityCount + (m_doubleCount * 2) + (m_tripleCount * 3); // トータル街破壊数を返す. パッシブスキル判定用
        }

        /// <summary>
        /// Inits the counts.
        /// </summary>
        public void InitCounts()
        {
            m_CityCount = 0;
            m_doubleCount = 0;
            m_tripleCount = 0;
        }

        /// <summary>
        /// パネルのタイプで判別して対応した処理をする
        /// </summary>
        /// <param name="panel">Panel.</param>
        public void PanelJudger(Panel panel)
        {
                switch (panel.m_panelType)
            {
                case PanelType.City: // cityパネルを引いた時
                    m_CityCount++; // シティパネルカウントアップ
                    break;
                case PanelType.DoubleCity: // doubleパネルを引いた時
                    m_doubleCount++;// ダブルパネルカウントアップ
                    break;
                case PanelType.TripleCity: // tripleパネルを引いた時
                    m_tripleCount++;// トリプルパネルカウントアップ
                    break;
                case PanelType.Enemy: // enemyパネルを引いた時
                    m_battleManager.m_states = BattleManager.States.PlayerAttack; // ステートマシンをPlayerAttackへ
                    m_battleManager.m_behaviourByState.Invoke(BattleManager.States.PlayerAttack); // m_behaviourByStateイベントを起動する
                    break;
            }
        }
    }
}