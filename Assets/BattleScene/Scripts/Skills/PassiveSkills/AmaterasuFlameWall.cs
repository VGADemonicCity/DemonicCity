using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity.BattleScene.Skill
{

    /// <summary>
    /// Amaterasu flame wall.
    /// １８１レベル：天照ー焔壁ー　
    /// 街破壊数３０以上で発動　次の敵の攻撃を無効化
    /// </summary>
    public class AmaterasuFlameWall : PassiveSkill
    {


        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 天照ー焔壁ー");
            m_defenseBuffer = m_battleManager.CurrentEnemy.Stats.Attack; // 敵の攻撃力をそのまま自分の防御力に加算
            m_battleManager.m_MagiaStats.Defense += (int)m_defenseBuffer;
            SetBuffText(EnhanceType.DefenseBuff, (int)m_defenseBuffer);
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
            Debug.Log("Deactivated the 天照ー焔壁ー");
            m_battleManager.m_MagiaStats.Defense -= (int)m_defenseBuffer;
        }
    }
}