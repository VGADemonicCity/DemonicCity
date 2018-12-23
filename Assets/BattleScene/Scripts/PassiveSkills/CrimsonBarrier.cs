using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{

    /// <summary>
    /// Crimson barrier.
    /// スキル : 紅蓮障壁
    /// 街破壊数１８以上で発動　次の敵の攻撃を１０％軽減
    /// </summary>
    public class CrimsonBarrier : PassiveSkill
    {
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.CrimsonBarrier; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 紅蓮障壁");
            m_defenseBuffer = m_battleManager.m_enemy.Stats.m_attack * m_incease; // enemyの攻撃力 * 乗算値
            m_battleManager.m_magia.m_defense += (int)m_defenseBuffer;
        }

        protected override void SkillDeactivate()
        {
            Debug.Log("Deactivated the 紅蓮障壁");
            m_battleManager.m_magia.m_defense -= (int)m_defenseBuffer; // 変動値を元に戻す
        }
    }
}

