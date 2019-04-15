using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleSceneTutorialsPopper : MonoBehaviour
    {
        public TutorialItems.TutorialItem NextItem
        {
            get
            {
                var index = tutorialObject.Items.IndexOf(currentItem);
                if (index < tutorialObject.Items.Count - 1)
                {
                    return tutorialObject.Items[++index];
                }
                return null;
            }
        }

        public TutorialItems.TutorialItem PreviousItem
        {
            get
            {
                var index = tutorialObject.Items.IndexOf(currentItem);
                if (index > 0)
                {
                    return tutorialObject.Items[--index];
                }
                return null;
            }
        }

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

        const int width = 1080;
        const int height = 1920;

        private void Start()
        {
            popupSystem = GetComponent<PopupSystem>();
            popupMaterial = new PopupSystemMaterial(OnPushOk, okButton.gameObject.name, true);
        }

        public void Popup(Subject subject)
        {
            popupSystem.Popup();
            popupSystem.SubscribeButton(popupMaterial);
            var items = tutorialObject.Items.FindAll(item => subject == item.subject);
            if (items.Count > 1)
            {
                var imageSize = new Vector2(width, height);
                float xPos = 0;
                tutorialObject.Items.ForEach(item =>
                {
                    var go = new GameObject();
                    go.transform.parent = popupSystem.popupedObject.transform.GetChild(0);
                    var image = go.AddComponent<Image>();
                    image.sprite = item.Sprite;
                    image.rectTransform.sizeDelta = imageSize;
                    image.rectTransform.position = new Vector2(xPos + (width * 0.5f), 0);
                    xPos += width;
                    image.rectTransform.localScale = Vector2.one;
                });
            }
            else
            {
            currentItem = items.First();

            }

            targetImage.sprite = currentItem.Sprite;
            if (currentItem.useVoice)
            {
                //SoundManager.Instance.PlayWithFade(SoundManager.SoundTag.SE, clip);
            }
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
