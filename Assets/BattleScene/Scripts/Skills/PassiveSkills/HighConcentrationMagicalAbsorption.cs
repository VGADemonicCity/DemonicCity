﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// High concentration magical absorption.
    /// １１レベル：高濃度魔力吸収
    /// 　街破壊数７以上で発動　街破壊数×０．５％攻撃力　防御力を上昇
    /// </summary>
    public class HighConcentrationMagicalAbsorption : PassiveSkill
    {

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 高濃度魔力吸収");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.Attack * m_incease; // 街破壊数 * (攻撃力 * 任意の%)
            m_defenseBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.Defense * m_incease; // 街破壊数 * (防御力 * 任意の%)
            m_battleManager.m_MagiaStats.Attack += (int)m_attackBuffer; // intに変換
            m_battleManager.m_MagiaStats.Defense += (int)m_defenseBuffer; // intに変換
            buffText = string.Format("攻{0}\n防{1}", (int)m_attackBuffer, (int)m_defenseBuffer);
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
            Debug.Log("Deactivated the 高濃度魔力吸収");
            m_battleManager.m_MagiaStats.Attack -= (int)m_attackBuffer; // 変動値を元に戻す
            m_battleManager.m_MagiaStats.Defense -= (int)m_defenseBuffer; // 変動値を元に戻す
        }
    }
}
