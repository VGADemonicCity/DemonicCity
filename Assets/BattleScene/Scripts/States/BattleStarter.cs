using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle
{
    /// <summary>
    /// ボタン等のイベントに応じてバトルをスタートさせる
    /// </summary>
    public class BattleStarter : StatesBehaviour
    {
        /// <summary>
        /// バトルをスタートさせる
        /// </summary>
        public void BattleStart()
        {
            // ==============================
            // イベント呼び出し : StateMachine.PlayerChoice
            // ==============================
            m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
        }
    }
}
