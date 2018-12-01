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
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.01f;

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.ExplosiveFlamePillar; // フラグを設定
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
            m_magia.Stats.m_attack += count * m_magia.Stats.m_attack * m_increase; // 攻撃力の任意の%分加算

        }
    }
}

