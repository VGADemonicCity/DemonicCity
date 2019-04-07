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

        /// <summary>
        /// スキル発動条件の審査を行う
        /// 魔拳烈火ノ型の発動条件が正の場合発動させない
        /// </summary>
        /// <param name="passiveSkill">Passive skill.</param>
        /// <param name="cityDestructionCount">City destruction count.</param>
        protected override void TryProcess(Magia.PassiveSkill passiveSkill, SkillManager.Timing timing, int cityDestructionCount)
        {

            // パッシブスキルフラグが建っている && パッシブスキルフラグに魔拳烈火ノ型 && 街破壊カウントが条件を満たしていたら && スキルを呼び出していない時　SkillActivateを呼ぶ
            if ((passiveSkill & m_passiveSkillName) == m_passiveSkillName
                && cityDestructionCount < GetComponent<InfernosFist>().CountCondition
                && cityDestructionCount >= CountCondition
                && timing == m_timing)
            {
                m_skillActivated = true; // フラグを立てる
                IsActivatable = true;
                SkillActivate();
            }
        }

        /// <summary>
        /// スキル発動
        /// 魔拳の代わりに発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 魔拳烈火ノ型");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.Attack * m_incease; // 攻撃力の任意の%分加算
            m_battleManager.m_MagiaStats.Attack += (int)m_attackBuffer; // intに変換
        }

        protected override void SkillDeactivate()
        {
            base.SkillDeactivate();
            Debug.Log("Deactivated the 魔拳烈火ノ型");
            m_battleManager.m_MagiaStats.Attack -= (int)m_attackBuffer; // 変動値を元に戻す
        }
    }
}

