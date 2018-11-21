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
        BattleManager m_battleManager = BattleManager.Instance;


        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <returns>The total count.</returns>
        public int GetTotalCount()
        {
            return m_CityCount + m_doubleCount + m_tripleCount;
        }

        /// <summary>
        /// Gets the passive count.
        /// </summary>
        /// <returns>The passive count.</returns>
        public int GetPassiveCount()
        {
            return m_CityCount + (m_doubleCount * 2) + (m_tripleCount * 3);
        }

        /// <summary>
        /// パネルのタイプで判別して対応した処理をする
        /// </summary>
        /// <param name="panel">Panel.</param>
        public void PanelJudger(Panel panel)
        {
                switch (panel.m_panelType)
            {
                case PanelType.City:
                    m_CityCount++; // シティパネルカウントアップ
                    break;
                case PanelType.DoubleCity:
                    m_doubleCount++;// ダブルパネルカウントアップ
                    break;
                case PanelType.TripleCity:
                    m_tripleCount++;// トリプルパネルカウントアップ
                    break;
                case PanelType.Enemy:
                    m_battleManager.m_states = BattleManager.States.PlayerAttack; // ステートマシンをPlayerAttackへ
                    break;
            }
        }
    }
}