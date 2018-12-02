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


        /// <summary>
        /// 条件に沿ってパッシブスキルを発動させるかどうかを判断して、条件を満たすスキルの効果を反映させるイベント
        /// Skill judger.
        /// int arg0 = Level.
        /// int arg1 = City destruction count.
        /// </summary>
        public class SkillJudger : UnityEvent<Magia.PassiveSkill, int>
        {
            public SkillJudger() { }
        }
    }
}