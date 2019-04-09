using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace DemonicCity.BattleScene.Debugger
{
    public class DebuggerButton : MonoBehaviour
    {
        BattleManager battleManager;
        BattleDebugger battleDebugger;
        PopupSystem popupSystem;
        [SerializeField] Button closeButton;
        [SerializeField] Toggle autoPlayToggle;
        [SerializeField] Toggle skipEffectToggle;
        [SerializeField] Toggle displayPanelsToggle;

        List<PopupSystemMaterial> toggleMaterials;
        PopupSystemMaterial buttonMaterial;

        private void Awake()
        {
            battleDebugger = BattleDebugger.Instance;
            battleManager = BattleManager.Instance;
            popupSystem = GetComponent<PopupSystem>();

            buttonMaterial = new PopupSystemMaterial(Close, closeButton.gameObject.name, true);
            toggleMaterials = new List<PopupSystemMaterial>
            {
                new PopupSystemMaterial(OnChangedValueOfAutoPlay,autoPlayToggle.gameObject.name),
                new PopupSystemMaterial(OnChangedValueOfSkillSkip,skipEffectToggle.gameObject.name),
                new PopupSystemMaterial(OnChangedValueOfDisplayPanels,displayPanelsToggle.gameObject.name),
            };
        }

        /// <summary>
        /// Close
        /// </summary>
        void Close()
        {
            battleManager.SetStateMachine(battleManager.m_StateMachine.m_PreviousState);
        }

        /// <summary>
        /// デバッグ画面を出す
        /// </summary>
        public void InningTheDebugWindow()
        {
            if (battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Pause
                && battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Init
                && battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Win)
            {
                battleManager.SetStateMachine(BattleManager.StateMachine.State.Pause);
                popupSystem.Popup();
                popupSystem.SubscribeButton(buttonMaterial);
                toggleMaterials.ForEach(item => popupSystem.SubscribeButton(item));
            }
        }

        /// <summary>
        /// AutoPlayがトグルされた時Debuggerのフラグを切り替える
        /// </summary>
        /// <param name="changedValue"></param>
        void OnChangedValueOfAutoPlay(bool changedValue)
        {
            if (changedValue)
            {
                battleDebugger.Flag |= BattleDebugger.DebuggingFlag.AutoPlay;
            }
            else if (BattleDebugger.DebuggingFlag.AutoPlay == (battleDebugger.Flag & BattleDebugger.DebuggingFlag.AutoPlay))
            {
                battleDebugger.Flag ^= BattleDebugger.DebuggingFlag.AutoPlay;
            }
        }

        /// <summary>
        /// SkillSkipがトグルされた時Debuggerのフラグを切り替える
        /// </summary>
        /// <param name="changedValue"></param>
        void OnChangedValueOfSkillSkip(bool changedValue)
        {
            if (changedValue)
            {
                battleDebugger.Flag |= BattleDebugger.DebuggingFlag.SkipEffect;
            }
            else if (BattleDebugger.DebuggingFlag.SkipEffect == (battleDebugger.Flag & BattleDebugger.DebuggingFlag.SkipEffect))
            {
                battleDebugger.Flag ^= BattleDebugger.DebuggingFlag.SkipEffect;
            }
        }

        /// <summary>
        /// DisplayPanelsがトグルされた時Debuggerのフラグを切り替える
        /// </summary>
        /// <param name="changedValue"></param>
        void OnChangedValueOfDisplayPanels(bool changedValue)
        {
            if (changedValue)
            {
                battleDebugger.Flag |= BattleDebugger.DebuggingFlag.DisplayPanels;
            }
            else if (BattleDebugger.DebuggingFlag.DisplayPanels == (battleDebugger.Flag & BattleDebugger.DebuggingFlag.DisplayPanels))
            {
                battleDebugger.Flag ^= BattleDebugger.DebuggingFlag.DisplayPanels;
            }
        }

    }
}