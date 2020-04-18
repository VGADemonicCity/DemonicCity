using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle.Skill
{
    /// <summary>
    /// SelfRegeneration
    ///２３レベル：自己再生
    /// 街破壊数１２以上で発動　街破壊数×最大 HPの１％回復
    /// </summary>
    public class SelfRegeneration : PassiveSkill
    {
        /// <summary>MagiaのHPDrawの参照</summary>
        [SerializeField] HitPointGauge m_magiaHPGauge;

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 自己再生");
            m_hitPointBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.HitPoint * m_incease; // 丸め込み対策の為に一度変数に保存
            m_battleManager.m_MagiaStats.HitPoint += (int)m_hitPointBuffer; // ここでintに変換

            if (m_battleManager.m_MagiaStats.HitPoint > m_battleManager.m_MagiaStats.MaxHP) // もしMaxHPを越したら
            {
                m_battleManager.m_MagiaStats.HitPoint = m_battleManager.m_MagiaStats.MaxHP; // hpをmaxに戻す
            }
            m_magiaHPGauge.Sync(m_battleManager.m_MagiaStats.HitPoint); // HPGaugeと同期
            SetBuffText(EnhanceType.HpBuff, (int)m_hitPointBuffer);
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
        }
    }
}

