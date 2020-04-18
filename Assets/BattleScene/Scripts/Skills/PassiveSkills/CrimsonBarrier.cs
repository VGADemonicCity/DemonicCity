using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle.Skill
{

    /// <summary>
    /// Crimson barrier.
    /// スキル : 紅蓮障壁
    /// 街破壊数１８以上で発動　次の敵の攻撃を１０％軽減
    /// </summary>
    public class CrimsonBarrier : PassiveSkill
    {
        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 紅蓮障壁");
            m_defenseBuffer = m_battleManager.CurrentEnemy.Stats.Attack * m_incease; // enemyの攻撃力 * 乗算値
            m_battleManager.m_MagiaStats.Defense += (int)m_defenseBuffer;
            SetBuffText(EnhanceType.DefenseBuff, (int)m_defenseBuffer);
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
            Debug.Log("Deactivated the 紅蓮障壁");
            m_battleManager.m_MagiaStats.Defense -= (int)m_defenseBuffer; // 変動値を元に戻す
        }
    }
}

