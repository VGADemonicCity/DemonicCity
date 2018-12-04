using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{

    /// <summary>
    /// Amaterasu inferno.
    /// １６０レベル：天照ー爆炎ー　
    /// 条件を満たすと豪炎爆砕掌の代わりに呼ばれる
    /// 街破壊数２８以上で発動　攻撃力の５０％を豪炎爆砕掌のスキル効果に上乗せする
    /// </summary>
    public class AmaterasuInferno : PassiveSkill
    {
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.01f;

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.AmaterasuInferno; // フラグを設定
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// 魔拳
        /// 街破壊数1以上で発動.
        /// 街破壊数 * 攻撃力の1% を加算して攻撃
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 魔拳");
            var count = m_panelCounter.GetCityDestructionCount(); // 街破壊数

        }
    }
}

    