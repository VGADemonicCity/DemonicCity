using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Nafura.
    /// ナフラのキャラクター情報
    /// </summary>
    public class Nafura : EnemyObject
    {
        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_statistics.m_hitPoint = 700;
            m_statistics.m_attack = 130;
            m_statistics.m_defense = 40;
        }
    }
}
