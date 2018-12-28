using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class BattleStarter : MonoBehaviour
    {
        /// <summary>BattleManagerの参照</summary>
        protected BattleManager m_battleManager;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected virtual void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
        }


        /// <summary>
        /// バトルをスタートさせる
        /// </summary>
        public void BattleStart()
        {
            m_battleManager.m_behaviourByState.Invoke(BattleManager.StateMachine.State.Init);
         

             //StartCoroutine(StartWait());
        }

        /// <summary>
        /// Starts the wait.
        /// </summary>
        /// <returns>The wait.</returns>
        IEnumerator StartWait()
        {
            yield return new WaitForSeconds(1f);
            // ==============================
            // イベント呼び出し : StateMachine.Init
            // ==============================
            m_battleManager.m_behaviourByState.Invoke(BattleManager.StateMachine.State.Init);
        }
    }
}
