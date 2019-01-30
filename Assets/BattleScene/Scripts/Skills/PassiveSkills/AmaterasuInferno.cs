using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{

    /// <summary>
    /// Amaterasu inferno.
    /// １６０レベル：天照ー爆炎ー　
    /// 条件を満たすと豪炎爆砕掌の代わりに呼ばれる
    /// 街破壊数２８以上で発動　攻撃力の５０％を豪炎爆砕掌のスキル効果に上乗せする
    /// </summary>
    public class AmaterasuInferno : PassiveSkill
    {
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.AmaterasuInferno; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 天照ー爆炎ー");
        }

        protected override void SkillDeactivate()
        {
            Debug.Log("Deactivated the 天照ー爆炎ー");
        }
    }
}

    