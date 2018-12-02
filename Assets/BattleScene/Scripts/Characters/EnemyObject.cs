using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Enemy character.
    /// </summary>
    public class EnemyObject : MonoBehaviour
    {
        /// <summary>ステータス</summary>
        [SerializeField] protected Statistics m_statistics = new Statistics();
    }
}
