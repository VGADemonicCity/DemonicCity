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
        /// <summary>MagiaのHPDrawの参照</summary>
        [SerializeField] HitPointGauge m_magiaHPGauge;

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkillName = Magia.PassiveSkill.SatansCell; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 魔王ノ細胞");
            m_hitPointBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.MaxHP * m_incease; // 街破壊数 * 最大HP * 割合
            m_battleManager.m_MagiaStats.HitPoint += (int)m_hitPointBuffer;

            if (m_battleManager.m_MagiaStats.HitPoint > m_battleManager.m_MagiaStats.MaxHP) // もしMaxHPを越したら
            {
                m_battleManager.m_MagiaStats.HitPoint = m_battleManager.m_MagiaStats.MaxHP; // hpをmaxに戻す
            }
            m_magiaHPGauge.Sync(m_battleManager.m_MagiaStats.HitPoint); // HPGaugeと同期
        }

        protected override void SkillDeactivate()
        {
        }
    }
}

