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
                /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.005f;

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.HighConcentrationMagicalAbsorption; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 高濃度魔力吸収");
            var attackBuffer = m_panelCounter.GetCityDestructionCount() * m_magia.Stats.m_attack * m_increase; // 街破壊数 * (攻撃力 * 任意の%)
            var defenseBuffer = m_panelCounter.GetCityDestructionCount() * m_magia.Stats.m_defense * m_increase; // 街破壊数 * (防御力 * 任意の%)
            m_magia.Stats.m_attack += (int)attackBuffer; // intに変換
            m_magia.Stats.m_defense += (int)defenseBuffer; // intに変換
        }
    }
}
