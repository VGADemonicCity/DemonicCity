using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle.Skill
{

    /// <summary>
    /// Infernos fist.
    /// １００レベル：豪炎爆砕掌
    /// 破壊数１１以上で発動　破壊数×攻撃力の１％を加算 
    /// </summary>
    public class InfernosFist : PassiveSkill
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="passiveSkill">Passive skill.</param>
        /// <param name="timing"></param>
        /// <param name="cityDestructionCount">City destruction count.</param>
        protected override void TryProcess(Magia.PassiveSkill passiveSkill, SkillManager.Timing timing, int cityDestructionCount)
        {

            // パッシブスキルフラグが建っている && 天照ー爆炎ーの条件以下の破壊数 && 街破壊カウントが条件を満たしていたら && スキルを呼び出していない時　SkillActivateを呼ぶ
            if ((passiveSkill & m_passiveSkill) == m_passiveSkill
                && cityDestructionCount >= CountCondition
                && timing == m_timing)
            {
                if (cityDestructionCount >= GetComponent<AmaterasuInferno>().CountCondition && (Magia.PassiveSkill.AmaterasuInferno) == (m_magia.MyPassiveSkill & Magia.PassiveSkill.AmaterasuInferno))
                {
                    return;
                }
                m_skillActivated = true; // フラグを立てる
                IsActivatable = true;
                SkillActivate();
            }
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
            base.SkillDeactivate();
            Debug.Log("Deactivated the 豪炎爆砕掌");
            m_battleManager.m_MagiaStats.Attack -= (int)m_attackBuffer; // intに変換
        }
    }
}

