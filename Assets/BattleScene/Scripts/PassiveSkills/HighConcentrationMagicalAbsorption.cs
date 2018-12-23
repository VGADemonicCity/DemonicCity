using System.Collections;
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
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.HighConcentrationMagicalAbsorption; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 高濃度魔力吸収");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_magiaStats.Temp.m_attack * m_incease; // 街破壊数 * (攻撃力 * 任意の%)
            m_defenseBuffer = m_panelCounter.DestructionCount * m_battleManager.m_magiaStats.Temp.m_defense * m_incease; // 街破壊数 * (防御力 * 任意の%)
            m_battleManager.m_magiaStats.m_attack += (int)m_attackBuffer; // intに変換
            m_battleManager.m_magiaStats.m_defense += (int)m_defenseBuffer; // intに変換
        }

        protected override void SkillDeactivate()
        {
            Debug.Log("Deactivated the 高濃度魔力吸収");
            m_battleManager.m_magiaStats.m_attack -= (int)m_attackBuffer; // 変動値を元に戻す
            m_battleManager.m_magiaStats.m_defense -= (int)m_defenseBuffer; // 変動値を元に戻す
        }
    }
}
