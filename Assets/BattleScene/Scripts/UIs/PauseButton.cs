using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] PopupWindowSystem pws;

        public void InningThePause()
        {
            BattleManager.Instance.SetStateMachine(BattleManager.StateMachine.State.Pause);
            pws.Popup();
        }
    }
}