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

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 心焔権現");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.Attack * m_incease; // 街破壊数 * (攻撃力 * 任意の%)
            m_battleManager.m_MagiaStats.Attack += (int)m_attackBuffer; // intに変換
            SetBuffText(EnhanceType.AttackBuff, (int)m_attackBuffer);
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
            Debug.Log("Deactivated the 心焔権現");
            m_battleManager.m_MagiaStats.Attack -= (int)m_attackBuffer; // 変動値を元に戻す
        }
    }
}

