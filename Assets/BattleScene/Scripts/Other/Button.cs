using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonicCity.BattleScene;
using DemonicCity.Debugger;

public class Button : MonoBehaviour
{
    BattleDebugger m_battleDebugger;


    private void Awake()
    {
        m_battleDebugger = BattleDebugger.Instance;
        
    }

    public void Onclick()
    {
        //if (m_battleDebugger.m_debugFlag == true)
        //{
        //    m_battleDebugger.m_debugFlag = false;
        //    return;
        //}
        //if (m_battleDebugger.m_debugFlag == false)
        //{
        //    m_battleDebugger.m_debugFlag = true;
        //    return;
        //}
    }               
}
