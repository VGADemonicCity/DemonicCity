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
            m_passiveSkillName = Magia.PassiveSkill.ExplosiveFlamePillar; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 爆炎熱風柱");
            m_attackBuffer = m_battleManager.m_MagiaStats.Temp.Attack * m_incease; // 攻撃力の1/2
            m_battleManager.CurrentEnemy.Stats.HitPoint -= (int)m_attackBuffer; // hpに直接ダメージを与える
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
        }
    }
}

