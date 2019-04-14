using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleSceneTutorialsPopper : MonoBehaviour
    {
        [SerializeField] TutorialItems tutorialObject;
        /// <summary>閉じるボタン</summary>
        [SerializeField] Button okButton;

        /// <summary>popup system</summary>
        PopupSystem popupSystem;
        /// <summary>現在対象となっている素材</summary>
        TutorialItems.TutorialItem currentItem;
        /// <summary>popup materials</summary>
        PopupSystemMaterial popupMaterial;
        /// <summary>OKボタン</summary>
        Button popupedOkButton;
        /// <summary>対象の</summary>
        Image targetImage;


        private void Start()
        {
            popupSystem = GetComponent<PopupSystem>();
            popupMaterial = new PopupSystemMaterial(OnPushOk, okButton.gameObject.name, true);
        }

        public void Popup(Subject subject)
        {
            popupSystem.Popup();
            popupSystem.SubscribeButton(popupMaterial);
            currentItem = tutorialObject.Items.Find(i => subject == i.subject);
        }

        void OnPushOk()
        {
            var battleManager = BattleManager.Instance;
            battleManager.SetStateMachine(battleManager.m_StateMachine.m_PreviousState);
        }


    }
    public enum Subject
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven,
        Twelve,
        Thirteen,
        Fourteen,
    }
}
