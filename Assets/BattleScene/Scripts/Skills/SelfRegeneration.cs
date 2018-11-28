using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// SelfRegeneration
    ///２３レベル：自己再生
    /// 街破壊数１２以上で発動　街破壊数×最大 HPの１％回復
    /// </summary>
    public class SelfRegeneration : PassiveSkill
    {
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.01f;

        protected override void Awake()
        {
            base.Awake();
            if (m_passiveSkill == 0) // 
            {
                m_passiveSkill = Statistics.PassiveSkill.DevilsFist; // フラグを設定
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
        /// 魔拳
        /// 街破壊数1以上で発動.
        /// 街破壊数 * 攻撃力の1% を加算して攻撃
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 魔拳");
            var count = m_panelCounter.GetCityDestructionCount(); // 街破壊数
            m_magia.Stats.m_attack += count * m_magia.Stats.m_attack * m_increase; // 攻撃力の任意の%分加算

        }
    }
}

