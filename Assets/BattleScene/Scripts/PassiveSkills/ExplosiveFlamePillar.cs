using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{

    /// <summary>
    /// Explosive flame pillar.
    /// スキル : 爆炎熱風柱
    /// １６以上で発動　自分の攻撃力２分の1を敵に防御力を無視してダメージを与える
    /// </summary>
    public class ExplosiveFlamePillar : PassiveSkill
    {
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.ExplosiveFlamePillar; // フラグを設定
            m_timing = SkillManager.Timing.SpecialAttack; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 爆炎熱風柱");
            m_attackBuffer = m_battleManager.m_MagiaStats.Temp.m_attack * m_incease; // 攻撃力の1/2
            m_battleManager.CurrentEnemy.Stats.m_hitPoint -= (int)m_attackBuffer; // hpに直接ダメージを与える
        }

        protected override void SkillDeactivate()
        {
        }
    }
}

