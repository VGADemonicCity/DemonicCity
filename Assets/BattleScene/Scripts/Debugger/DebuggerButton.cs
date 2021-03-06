﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

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
        [SerializeField] Slider openPanelQuantitySlider;
        [SerializeField] Slider panelAnimTimeSlider;

        List<PopupSystemMaterial> toggleMaterials;
        PopupSystemMaterial buttonMaterial;
        List<PopupSystemMaterial> sliderMaterials;

        private void Awake()
        {
            battleDebugger = BattleDebugger.Instance;
            battleManager = BattleManager.Instance;
            popupSystem = GetComponent<PopupSystem>();

            buttonMaterial = new PopupSystemMaterial(Close, closeButton.gameObject.name, true);
            sliderMaterials = new List<PopupSystemMaterial> {
                new PopupSystemMaterial(OnChangedValueOfOpenPanels, openPanelQuantitySlider.gameObject.name),
                new PopupSystemMaterial(OnChangedValueOfPanelAnimTime, panelAnimTimeSlider.gameObject.name),
            };
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
            battleManager.SetStateMachine(battleManager.m_StateMachine.PreviousStateWithoutSpecialStates);
        }

        /// <summary>
        /// デバッグ画面を出す
        /// </summary>
        public void InningTheDebugWindow()
        {
            if (battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Pause
                && battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Init
                && battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Win
                && battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Debugging)
            {
                // pauseステートに遷移し,各要素をポップアップ後イベント登録
                battleManager.SetStateMachine(BattleManager.StateMachine.State.Debugging);
                popupSystem.Popup();
                popupSystem.SubscribeButton(buttonMaterial);
                toggleMaterials.ForEach(material => popupSystem.SubscribeToggle(material));
                sliderMaterials.ForEach(material => popupSystem.SubscribeSlider(material));

                // 各要素を適切に初期化
                var toggles = popupSystem.popupedObject.GetComponentsInChildren<Toggle>().ToList();
                toggles.ForEach(toggle =>
                {
                    if(toggle.gameObject.name == displayPanelsToggle.gameObject.name)
                    {
                       if(battleDebugger.DisplayPanels)
                        {
                            toggle.isOn = true;
                        }
                    }
                    if (toggle.gameObject.name == autoPlayToggle.gameObject.name)
                    {
                        if (battleDebugger.AutoPlay)
                        {
                            toggle.isOn = true;
                        }
                    }
                    if (toggle.gameObject.name == skipEffectToggle.gameObject.name)
                    {
                        if (battleDebugger.EffectSkip)
                        {
                            toggle.isOn = true;
                        }
                    }

                    var slider = GameObject.Find("OpenPanelsSlider").GetComponentInChildren<Slider>();
                    slider.value = battleDebugger.OpenPanelQuantity;

                    slider = GameObject.Find("PanelAnimTimeSlider").GetComponentInChildren<Slider>();
                    slider.value = PanelManager.Instance.PanelAnimTime;
                });
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
                battleDebugger.IsExecutable = true;
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

        /// <summary>
        /// Openボタンを押した時開くパネルの枚数を設定
        /// </summary>
        /// <param name="changedValue"></param>
        void OnChangedValueOfOpenPanels(float changedValue)
        {
            battleDebugger.OpenPanelQuantity = (int)changedValue;
            var textBox = GameObject.Find("Counter").GetComponent<Text>();
            textBox.text = changedValue.ToString();
        }

        void OnChangedValueOfPanelAnimTime(float changedValue)
        {
            PanelManager.Instance.PanelAnimTime = changedValue;
            var textBox = GameObject.Find("AnimSpeed").GetComponent<Text>();
            textBox.text = changedValue.ToString();
        }
    }
}