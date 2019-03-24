using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{

    /// <summary>
    /// Infernos fist.
    /// １００レベル：豪炎爆砕掌
    /// 破壊数１１以上で発動　破壊数×攻撃力の１％を加算 
    /// </summary>
    public class InfernosFist : PassiveSkill
    {
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkillName = Magia.PassiveSkill.InfernosFist; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキルアクティブ
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 豪炎爆砕掌");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.Attack * m_incease; // 攻撃力の任意の%分加算
            m_battleManager.m_MagiaStats.Attack += (int)m_attackBuffer; // intに変換
        }

        /// <summary>
        /// スキル非アクティブ
        /// </summary>
        protected override void SkillDeactivate()
        {
            Debug.Log("Deactivated the 豪炎爆砕掌");
            m_battleManager.m_MagiaStats.Attack -= (int)m_attackBuffer; // intに変換
        }
    }
}

