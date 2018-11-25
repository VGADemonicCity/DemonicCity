using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// Devils fist.
    /// スキル : 魔拳
    /// </summary>
    public class DevilsFist : PassiveSkill
    {
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.01f;

        /// <summary>
        /// Start this instance.
        /// </summary>
        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Tries the process.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <param name="CityDestructionCount">City destruction count.</param>
        public override void TryProcess(SaveData.Statistics.PassiveSkill passiveSkill, int CityDestructionCount)
        {
            base.TryProcess(m_magia.m_stats.m_passiveSkill, CityDestructionCount); // 親クラスのメソッドを呼ぶ

            if (m_trialResult) // スキル発動の条件を満たしていたら
            {
                SkillActivate();
            }
        }

        /// <summary>
        /// 魔拳
        /// 街破壊数1以上で発動.
        /// 街は回数*攻撃力の1%を加算して攻撃
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("DFよばれた");
            if ((m_magia.m_stats.m_passiveSkill & SaveData.Statistics.PassiveSkill.DevilsFist) == SaveData.Statistics.PassiveSkill.DevilsFist) // フラグが建っていたら
            {
                // スキルの中身
                var Count = m_panelCounter.GetCityDestructionCount(); // 街破壊数
                m_magia.m_stats.m_attack += m_magia.m_stats.m_attack * m_increase; // 攻撃力の任意の%分加算
            }
        }
    }
}

