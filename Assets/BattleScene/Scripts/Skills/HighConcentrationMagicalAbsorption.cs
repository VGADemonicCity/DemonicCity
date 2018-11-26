using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// High concentration magical absorption.
    /// スキル : 高濃度魔力吸収
    /// </summary>
    public class HighConcentrationMagicalAbsorption : PassiveSkill
    {
                /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.005f;

        protected override void Awake()
        {
            base.Awake();
            if (m_passiveSkill == 0) // 
            {
                m_passiveSkill = SaveData.Statistics.PassiveSkill.HighConcentrationMagicalAbsorption; // フラグを設定
            }
        }


        /// <summary>
        /// Start this instance.
        /// </summary>
        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// 高濃度魔力吸収
        /// 街は回数7以上で発動
        /// 街破壊数*攻撃力or防御力の0.5%をそれぞれ加算
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 高濃度魔力吸収");
            m_magia.m_stats.m_attack += m_panelCounter.GetCityDestructionCount() * m_magia.m_stats.m_attack * m_increase; // 攻撃力 += 街破壊数 * (攻撃力 * 任意の%)
            m_magia.m_stats.m_defense += m_panelCounter.GetCityDestructionCount() * m_magia.m_stats.m_defense * m_increase; // 防御力 += 街破壊数 * (防御力 * 任意の%)
        }
    }
}
