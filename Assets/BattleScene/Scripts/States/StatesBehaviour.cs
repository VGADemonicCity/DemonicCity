﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonicCity.BattleScene.Skill;
using System.Linq;

namespace DemonicCity.BattleScene
{

    /// <summary>
    /// State machine behaviour.
    /// </summary>
    public abstract class StatesBehaviour : MonoSingleton<StatesBehaviour>
    {
        /// <summary>PanelFrameManagerの参照</summary>
        protected PanelFrameManager m_panelFrameManager;
        /// <summary>GameManagerの参照</summary>
        protected GameManager m_gameManager;
        /// <summary>BattleManagerの参照</summary>
        protected BattleManager m_battleManager;
        /// <summary>PanelManagerの参照</summary>
        protected PanelManager m_panelManager;
        /// <summary>PanelCounterの参照</summary>
        protected PanelCounter m_panelCounter;
        /// <summary>SkillManagerの参照</summary>
        protected SkillManager m_skillManager;
        /// <summary>MagiaのHPDrawの参照</summary>
        protected HitPointGauge m_magiaHPGauge;
        /// <summary>EnemyのHPDrawの参照</summary>
        protected HitPointGauge m_enemyHPGauge;
        /// <summary>Magiaの参照</summary>
        protected Magia m_magia;
        /// <summary>Enemy skill gauge</summary>
        protected EnemySkillGauge m_enemySkillGauge;
        /// <summary>背景</summary>
        protected SpriteRenderer m_background;
        /// <summary>その章で使うChapterクラス</summary>
        protected Chapter m_chapter;
        /// <summary>sound manager</summary>
        protected SoundManager m_soundManager;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected virtual void Awake()
        {
            m_magia = Magia.Instance; // Magiaの参照取得
            m_skillManager = SkillManager.Instance; // SkillManagerの参照取得
            m_panelCounter = PanelCounter.Instance; // PanelCounterの参照取得
            m_panelManager = PanelManager.Instance; // PanelManagerの参照取得
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
            m_gameManager = GameManager.Instance; // Magiaの参照取得
            m_soundManager = SoundManager.Instance;
            m_panelFrameManager = PanelFrameManager.Instance; // PanelFrameManagerの参照取得
            m_magiaHPGauge = GameObject.Find("MagiaHPGauge").GetComponentInChildren<HitPointGauge>();
            m_enemyHPGauge = GameObject.Find("EnemyHPGauge").GetComponentInChildren<HitPointGauge>();
            m_enemySkillGauge = GameObject.Find("EnemyHPGauge").GetComponentInChildren<EnemySkillGauge>();
            m_background = GameObject.Find("Background").GetComponent<SpriteRenderer>();

            var debugger = Debugger.BattleDebugger.Instance;
            if (debugger.UseTargetStory)
            {
                m_chapter = ChapterManager.Instance.GetChapter(debugger.TargetStory);
            }
            else
            {
                // その章のChapterを取得
                m_chapter = ChapterManager.Instance.GetChapter();
            }
        }
    }
}
