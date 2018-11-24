using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// Skill.
    /// </summary>
    public abstract class Skill : MonoBehaviour
    {
        /// <summary>level conditions</summary>
        [SerializeField] protected int m_levelCondition = 1;
        /// <summary>count conditions</summary>
        [SerializeField] protected int m_CountCondition = 1;
        /// <summary>レベルとパネルカウントが上限に達していたらtrueが入る</summary>
        [SerializeField] protected bool m_trialResult = false;
        /// <summary>Magia</summary>
        protected Magia m_magia;
        /// <summary>SkillManagerの参照</summary>
        protected SkillManager m_skillManager;
        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;

        void Awake()
        {
            m_magia = Magia.Instance; //  Magiaのシングルトンインスタンス取得
            m_skillManager = SkillManager.Instance; // SkillManagerシングルトンインスタンス取得
            m_panelCounter = GetComponent<PanelCounter>(); // PanelCounterの参照取得
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        public virtual void Start()
        {
            m_skillManager.m_skillJudger.AddListener(TryProcess); // キャラのレベルと街破壊数を引数に渡して条件を満たせばスキルフラグを建てて効果を反映させる
        }

        /// <summary>
        /// Tries the process.
        /// </summary>
        /// <returns><c>true</c>, if process was tryed, <c>false</c> otherwise.</returns>
        /// <param name="level">Level.</param>
        /// <param name="CityDestructionCount">City destruction count.</param>
        public virtual void TryProcess(int level, int CityDestructionCount)
        {
            // レベルと街破壊カウントが条件を満たしていたらtrue満たなかったらfalseにする
            m_trialResult = true; // 初期値はtrue

            if (level < m_levelCondition || CityDestructionCount < m_CountCondition) // 条件を満たしていなかったらfalseに変える
            {
                m_trialResult = false;
            }
        }

        /// <summary>
        /// 此処に各スキルの中身を実装する
        /// </summary>
        protected abstract void SkillActivate();
    }
}
