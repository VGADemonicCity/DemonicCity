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


        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 天照ー爆炎ー");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.Attack * m_incease; // 攻撃力の任意の%分加算
            m_battleManager.m_MagiaStats.Attack += (int)m_attackBuffer; // intに変換
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
            Debug.Log("Deactivated the 天照ー爆炎ー");
            m_battleManager.m_MagiaStats.Attack -= (int)m_attackBuffer; // intに変換
        }
    }
}

    