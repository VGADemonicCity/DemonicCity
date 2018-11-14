using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Turn Based Battle System
    /// ターン制バトルシステム
    /// </summary>
    public class TurnSystemManager : MonoBehaviour
    {
        /// <summary>The players turn.</summary>
        [HideInInspector] public bool PlayersTurn;
        /// <summary>The m enemy remaining count.</summary>
        [SerializeField] int m_enemyRemainingCount;

        IEnumerator EnemyTurn()
        {
            yield return null;
        }

        IEnumerator PlayerTurn()
        {
            yield return null;
        }
    }

}

