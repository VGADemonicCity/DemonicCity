using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// SelfRegeneration
    ///２３レベル：自己再生
    /// 街破壊数１２以上で発動　街破壊数×最大 HPの１％回復
    /// </summary>
    public class SelfRegeneration : PassiveSkill
    {
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.SelfRegeneration; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 自己再生");
            m_hitPointBuffer = m_panelCounter.GetCityDestructionCount() * m_battleManager.BattleMagia.Temp.m_hitPoint * m_incease; // 丸め込み対策の為に一度変数に保存
            m_battleManager.BattleMagia.m_hitPoint += (int)m_hitPointBuffer; // ここでintに変換
        }

        protected override void SkillDeactivate()
        {
        }
    }
}

