using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// バトル中のスキル関係の制御を行うクラス
    /// </summary>
    public class SkillManager : MonoSingleton<SkillManager>
    {
        /// <summary>SkillProcessor</summary>
        public SkillJudger m_skillJudger = new SkillJudger();
        public Timing m_timing;


        /// <summary>
        /// 条件に沿ってパッシブスキルを発動させるかどうかを判断して、条件を満たすスキルの効果を反映させるイベント
        /// Skill judger.
        /// int arg0 = Level.
        /// int arg1 = City destruction count.
        /// </summary>
        public class SkillJudger : UnityEvent<Magia.PassiveSkill,Timing,int>
        {
            public SkillJudger() { }
        }

        /// <summary>
        /// スキル発動のタイミング
        /// </summary>
        public enum Timing
        {
            /// <summary>強化</summary>
            Enhancement,
            /// <summary>特殊攻撃</summary>
            SpecialAttack,
        }
    }
}