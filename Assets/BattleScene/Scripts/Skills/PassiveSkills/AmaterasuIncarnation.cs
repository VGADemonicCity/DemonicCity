using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{


    /// <summary>
    /// Amaterasu incanation.
    /// １３６レベル：天照権現　
    /// 街破壊数２６以上で発動　街破壊数×１％攻撃力　防御力を上昇
    /// </summary>
    public class AmaterasuIncarnation : PassiveSkill
    {
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkillName = Magia.PassiveSkill.AmaterasuIncanation; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 天照権現");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.Attack * m_incease; // 街破壊数 * (攻撃力 * 任意の%)
            m_defenseBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.Defense * m_incease; // 街破壊数 * (防御力 * 任意の%)
            m_battleManager.m_MagiaStats.Attack += (int)m_attackBuffer; // intに変換
            m_battleManager.m_MagiaStats.Defense += (int)m_defenseBuffer; // intに変換
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
            Debug.Log("Deactivated the 天照権現");
            m_battleManager.m_MagiaStats.Attack -= (int)m_attackBuffer; // 変動値を元に戻す
            m_battleManager.m_MagiaStats.Defense -= (int)m_defenseBuffer; // 変動値を元に戻す
        }
    }
}