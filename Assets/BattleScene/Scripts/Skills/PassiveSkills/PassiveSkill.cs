﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// Passive skill.
    /// パッシブスキルの親クラス
    /// フラグが立ってる時はスキルをアクティブ化してステータス等に効果を付与し、フラグが立っていない時は非アクティブ化して効果を消す
    /// </summary>
    public abstract class PassiveSkill : MonoBehaviour
    {
        public int CountCondition { get { return m_countCondition; } }
        public bool IsActivatable { get; set; }
        public Magia.PassiveSkill GetPassiveSkill { get { return m_passiveSkillName; } }
        public int LevelCondition { get { return m_levelCondition; } }

        /// <summary>count conditions</summary>
        [SerializeField] protected int m_countCondition = 1;
        /// <summary>level conditions</summary>
        [SerializeField] protected int m_levelCondition = 1;
        /// <summary>パッシブスキルフラグ用変数</summary>
        [SerializeField] protected Magia.PassiveSkill m_passiveSkillName;
        /// <summary>PassivesSkillの発動タイミング</summary>
        [SerializeField] protected SkillManager.Timing m_timing;
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] protected float m_incease;
        /// <summary>BattleManagerの参照</summary>
        protected BattleManager m_battleManager;
        /// <summary>SkillManagerの参照</summary>
        protected SkillManager m_skillManager;
        /// <summary>PanelCounterの参照</summary>
        protected PanelCounter m_panelCounter;
        /// <summary>Magiaの参照</summary>
        protected Magia m_magia;
        /// <summary>skillが既に適応されているかどうか.<see langword="true"/> is activated. <see langword="false"/> is deactivated</summary>
        protected bool m_skillActivated;
        /// <summary>hit pointの変動値を一時保存</summary>
        protected float m_hitPointBuffer;
        /// <summary>attackの変動値を一時保存</summary>
        protected float m_attackBuffer;
        /// <summary>defenseの変動値を一時保存</summary>
        protected float m_defenseBuffer;




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
            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                if (state == BattleManager.StateMachine.State.PlayerChoice && m_skillActivated) // playerChoice時 && スキルが発動されていた場合
                {
                    // フラグを降ろす
                    m_skillActivated = false;
                    IsActivatable = false;
                    SkillDeactivate();
                }
            });
        }

        /// <summary>
        /// Tries the process.
        /// </summary>
        /// <param name="passiveSkill">Passive skill.</param>
        /// <param name="cityDestructionCount">City destruction count.</param>
        protected virtual void TryProcess(Magia.PassiveSkill passiveSkill, SkillManager.Timing timing, int cityDestructionCount)
        {
            // パッシブスキルフラグが建っている && 街破壊カウントが条件を満たしていたら && スキルを呼び出していない && 呼び出しタイミングがAttack時　SkillActivateを呼ぶ
            if ((passiveSkill & m_passiveSkillName) == m_passiveSkillName
                && cityDestructionCount >= m_countCondition
                && timing == m_timing)
            {
                SkillActivate();
                // フラグを立てる
                IsActivatable = true;
                m_skillActivated = true;
            }
        }

        /// <summary>
        /// スキルが発動された時のコールバック
        /// </summary>
        public void OnSkillActivated()
        {
            IsActivatable = false;
        }

        /// <summary>
        /// 此処に各スキルのアクティブ時の効果を実装する
        /// </summary>
        protected abstract void SkillActivate();

        /// <summary>
        /// 此処に各スキルの非アクティブ時の効果をを実装する
        /// </summary>
        protected virtual void SkillDeactivate()
        {
            IsActivatable = false;
        }
    }
}
