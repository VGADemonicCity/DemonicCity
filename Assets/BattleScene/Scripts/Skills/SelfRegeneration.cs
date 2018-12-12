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
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.01f;

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.SelfRegeneration; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 自己再生");
            var buffer = m_panelCounter.GetCityDestructionCount() * m_magia.MaxHP * m_increase; // 丸め込み対策の為に一度変数に保存
            m_magia.Stats.m_hitPoint += (int)buffer; // ここでintに変換
        }
    }
}

