using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity.BattleScene.Debugger
{
    public class DebuggerButton : MonoBehaviour
    {
        BattleDebugger m_battleDebugger;
        [SerializeField] BattleDebugger.DebuggingFlag flag;

        private void Awake()
        {
            m_battleDebugger = BattleDebugger.Instance;
        }

        public void OnPush()
        {
            m_battleDebugger.Flag ^= flag;
        }
    }
}