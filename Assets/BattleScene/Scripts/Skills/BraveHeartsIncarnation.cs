using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// Brave hearts incarnation.
    /// ７０レベル：心焔権現　
    /// 街破壊数２０以上で発動　街破壊数×０．５％攻撃力　防御力を上昇
    /// </summary>
    public class BraveHeartsIncarnation : PassiveSkill
    {
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.01f;

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.BraveHeartsIncarnation; // フラグを設定
        }
        
        /// <summary>
        /// 魔拳
        /// 街破壊数1以上で発動.
        /// 街破壊数 * 攻撃力の1% を加算して攻撃
        /// </summary>
        protected override void SkillActivate()
        {


        }
    }
}

