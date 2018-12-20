using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{

    /// <summary>
    /// Satans cell.
    /// １１１レベル：魔王ノ細胞　
    /// 街破壊数２１枚以上で発動 街破壊数×最大HPの２％回復
    /// </summary>
    public class SatansCell : PassiveSkill
    {
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.SatansCell; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 魔王ノ細胞");
            m_hitPointBuffer = m_panelCounter.GetCityDestructionCount() * m_battleManager.BattleMagia.MaxHP * m_incease; // 街破壊数 * 最大HP * 割合
            m_battleManager.BattleMagia.m_hitPoint += (int)m_hitPointBuffer;
        }

        protected override void SkillDeactivate()
        {
        }
    }
}

