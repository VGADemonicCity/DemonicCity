using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// Passive skill.
    /// </summary>
    public abstract class PassiveSkill : MonoBehaviour
    {
        /// <summary>count conditions</summary>
        [SerializeField] protected int m_CountCondition = 1;
        /// <summary>パッシブスキルフラグ用変数</summary>
        [SerializeField] protected Magia.PassiveSkill m_passiveSkill;
        /// <summary>BattleManagerの参照</summary>
        protected BattleManager m_battleManager;
        /// <summary>SkillManagerの参照</summary>
        protected SkillManager m_skillManager;
        /// <summary>PanelCounterの参照</summary>
        protected PanelCounter m_panelCounter;
        /// <summary>Magiaの参照</summary>
        protected Magia m_magia;


        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected virtual void Awake()
        {
            m_battleManager = BattleManager.Instance; // PanelCounterの参照取得
            m_skillManager = SkillManager.Instance; // SkillManagerシングルトンインスタンス取得
            m_panelCounter = PanelCounter.Instance; // PanelCounterの参照取得
            m_magia = Magia.Instance; // Magiaの参照取得
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        protected virtual void Start()
        {
            m_skillManager.m_skillJudger.AddListener(TryProcess); // キャラのレベルと街破壊数を引数に渡して条件を満たせばスキルフラグを建てて効果を反映させる
        }

        /// <summary>
        /// Tries the process.
        /// </summary>
        /// <param name="passiveSkill">Passive skill.</param>
        /// <param name="CityDestructionCount">City destruction count.</param>
        public virtual void TryProcess(Magia.PassiveSkill passiveSkill, int CityDestructionCount)
        {
            // パッシブスキルフラグが建っている & 街破壊カウントが条件を満たしていたらSkillActivateを呼ぶ
            if ((passiveSkill & m_passiveSkill) == m_passiveSkill && CityDestructionCount >= m_CountCondition)
            {
                SkillActivate();
            }
        }

        /// <summary>
        /// 此処に各スキルの中身を実装する
        /// </summary>
        protected abstract void SkillActivate();
    }
}
