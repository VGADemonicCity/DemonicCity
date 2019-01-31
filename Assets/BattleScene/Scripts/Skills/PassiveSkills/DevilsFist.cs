using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// Devils fist.
    /// １レベル：魔拳　
    /// 街破壊数1以上で発動　街破壊数×攻撃力の1％を加算して攻撃
    /// </summary>
    public class DevilsFist : PassiveSkill
    {
        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.DevilsFist; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動条件の審査を行う
        /// 魔拳烈火ノ型の発動条件が正の場合発動させない
        /// </summary>
        /// <param name="passiveSkill">Passive skill.</param>
        /// <param name="cityDestructionCount">City destruction count.</param>
        protected override void TryProcess(Magia.PassiveSkill passiveSkill, SkillManager.Timing timing, int cityDestructionCount)
        {

            // パッシブスキルフラグが建っている && パッシブスキルフラグに魔拳烈火ノ型 && 街破壊カウントが条件を満たしていたら && スキルを呼び出していない && 呼び出しタイミングがAttack時　SkillActivateを呼ぶ
            if ((passiveSkill & m_passiveSkill) == m_passiveSkill
                && cityDestructionCount < GetComponent<DevilsFistInfernoType>().CountCondition
                && cityDestructionCount >= CountCondition
                && timing == m_timing)
            {
                m_skillActivated = true; // フラグを立てる
                SkillActivate();
            }
        }

        /// <summary>
        /// スキルアクティブ
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 魔拳");
            m_attackBuffer = m_panelCounter.DestructionCount * m_battleManager.m_MagiaStats.Temp.m_attack * m_incease; // 攻撃力の任意の%分加算
            m_battleManager.m_MagiaStats.m_attack += (int)m_attackBuffer; // intに変換
        }

        /// <summary>
        /// スキル非アクティブ
        /// </summary>
        protected override void SkillDeactivate()
        {
            Debug.Log("Deactivated the 魔拳");
            m_battleManager.m_MagiaStats.m_attack -= (int)m_attackBuffer; // intに変換
        } 
    }
}

