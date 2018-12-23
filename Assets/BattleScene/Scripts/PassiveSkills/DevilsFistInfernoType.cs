using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// DevilsFistInfernoType
    /// ５８レベル：魔拳烈火ノ型　
    /// 条件を満たすと魔拳の代わりに発動
    /// 街破壊数１１以上で発動　攻撃力の１０％を魔拳のスキル効果に上乗せして発動する
    /// </summary>
    public class DevilsFistInfernoType : PassiveSkill
    {

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.DevilsFistInfernoType; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// 魔拳の代わりに発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 魔拳烈火ノ型");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_magiaStats.Temp.m_attack * m_incease; // 攻撃力の任意の%分加算
            m_battleManager.m_magiaStats.m_attack += (int)m_attackBuffer; // intに変換
        }

        protected override void SkillDeactivate()
        {
            Debug.Log("Deactivated the 魔拳烈火ノ型");
            m_battleManager.m_magiaStats.m_attack -= (int)m_attackBuffer; // 変動値を元に戻す
        }
    }
}

