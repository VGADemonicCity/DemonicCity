using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{

    /// <summary>
    /// State machine behaviour.
    /// </summary>
    public class StatesBehaviour : MonoSingleton<StatesBehaviour>
    {
        /// <summary>PanelFrameManagerの参照</summary>
        protected PanelFrameManager m_panelFrameManager;
        /// <summary>BattleManagerの参照</summary>
        protected BattleManager m_battleManager;
        /// <summary>PanelManagerの参照</summary>
        protected PanelManager m_panelManager;
        /// <summary>PanelCounterの参照</summary>
        protected PanelCounter m_panelCounter;
        /// <summary>SkillManagerの参照</summary>
        protected SkillManager m_skillManager;
        /// <summary>Magiaの参照</summary>
        protected Magia m_magia;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected virtual void Awake()
        {
            m_panelFrameManager = PanelFrameManager.Instance; // PanelFrameManagerの参照取得
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
            m_panelManager = PanelManager.Instance; // PanelManagerの参照取得
            m_panelCounter = PanelCounter.Instance; // PanelCounterの参照取得
            m_skillManager = SkillManager.Instance; // SkillManagerの参照取得
            m_magia = Magia.Instance; // Magiaの参照取得

        }

        /// <summary>
        /// 指定したステートに遷移させてBattleManagerのイベントを呼び出す
        /// </summary>
        /// <param name="state">State machine.</param>
        protected void SetStateMachine(BattleManager.StateMachine.State state)
        {
            m_battleManager.m_stateMachine.m_state = state; // stateをセット
            // ==================================
            // イベント呼び出し
            // ==================================
            m_battleManager.m_behaviourByState.Invoke(state);
        }
    }
}
