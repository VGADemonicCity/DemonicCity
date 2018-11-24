using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// High concentration magical absorption.
    /// スキル : 高濃度魔力吸収
    /// </summary>
    public class HighConcentrationMagicalAbsorption : Skill
    {

        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.005f;

        /// <summary>
        /// Start this instance.
        /// </summary>
        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Tries the process.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <param name="CityDestructionCount">City destruction count.</param>
        public override void TryProcess(int level, int CityDestructionCount)
        {
            base.TryProcess(level, CityDestructionCount); // 親クラスのメソッドを呼ぶ

            if (m_trialResult) // TryProcessがtrueだった場合
            {
                m_magia.m_stats.m_passiveSkill = m_magia.m_stats.m_passiveSkill | SaveData.Statistics.PassiveSkill.HighConcentrationMagicalAbsorption; // 論理和でフラグを加える
                SkillActivate(); // スキル呼び出し
            }
        }

        /// <summary>
        /// 高濃度魔力吸収
        /// 街は回数7以上で発動
        /// 街破壊数*攻撃力or防御力の0.5%をそれぞれ加算
        /// </summary>
        protected override void SkillActivate()
        {
            m_magia.m_stats.m_attack += m_panelCounter.GetCityDestructionCount() * m_magia.m_stats.m_attack * m_increase; // 攻撃力 += 街破壊数 * (攻撃力 * 任意の%)
            m_magia.m_stats.m_defense += m_panelCounter.GetCityDestructionCount() * m_magia.m_stats.m_defense * m_increase; // 防御力 += 街破壊数 * (防御力 * 任意の%)
        }
    }
}
