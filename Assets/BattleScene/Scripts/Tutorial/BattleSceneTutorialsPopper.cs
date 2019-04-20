using System;
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
    public class BattleSceneTutorialsPopper : MonoSingleton<BattleSceneTutorialsPopper>
    {
        public TutorialItems.TutorialItem NextItem
        {
            get
            {
                var index = targetItems.IndexOf(currentItem);
                if (index < targetItems.Count - 1)
                {
                    return targetItems[++index];
                }
                return null;
            }
        }

        public TutorialItems.TutorialItem PreviousItem
        {
            get
            {
                var index = targetItems.IndexOf(currentItem);
                if (index > 0)
                {
                    return targetItems[--index];
                }
                return null;
            }
        }

        /// <summary>表示させるチュートリアルの素材群</summary>
        [SerializeField] TutorialItems tutorialObject;
        /// <summary>次へボタン</summary>
        [SerializeField] Button toNextButton;
        /// <summary>閉じるボタン</summary>
        [SerializeField] Button closeButton;
        /// <summary>itween animationに使う時間</summary>
        [SerializeField] float fadingTime = .5f;
        [SerializeField] AudioSource audioSource;

        /// <summary>popup system</summary>
        PopupSystem popupSystem;
        /// <summary>現在対象となっている素材</summary>
        TutorialItems.TutorialItem currentItem;
        /// <summary>そのタイミングに表示するチュートリアル素材の一覧</summary>
        List<TutorialItems.TutorialItem> targetItems;
        /// <summary>popup materials</summary>
        List<PopupSystemMaterial> popupMaterials;
        /// <summary>チュートリアル画像を生成するオブジェクトの親</summary>
        GameObject tutorialImagesParent;
        /// <summary>次へボタン</summary>
        Button popupedToNextButton;
        /// <summary>閉じるボタン</summary>
        Button popupedCloseButton;


        const int width = 1080;
        const int height = 1920;

        public void TestPopup()
        {
            if(BattleManager.Instance.m_StateMachine.m_State != BattleManager.StateMachine.State.Init )
            Popup(Subject.AboutAttack | Subject.CompletePanels);
        }

        private void Start()
        {
            popupSystem = GetComponent<PopupSystem>();

            popupMaterials = new List<PopupSystemMaterial>()
            {
                new PopupSystemMaterial(OnPushNextButton,toNextButton.gameObject.name,false),
                new PopupSystemMaterial(OnPushOk, closeButton.gameObject.name, true),
            };
        }

        /// <summary>
        /// subject
        /// </summary>
        /// <param name="subject"></param>
        public void Popup(Subject subject)
        {
            BattleManager.Instance.SetStateMachine(BattleManager.StateMachine.State.Pause);
            popupSystem.Popup();
            popupMaterials.ForEach(material => popupSystem.SubscribeButton(material));
            targetItems = tutorialObject.Items.FindAll(( item) => item.subject == (item.subject & subject));
            //targetItems = targetItems.OrderBy(item => item.subject).ToList();
            var imageSize = new Vector2(width, height);
            float xPos = 0;
            targetItems.ForEach(item =>
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
            OnPopup();
        }

        void OnPopup()
        {
            // on pupuped.
            if (targetItems[0] != null)
            {
            currentItem = targetItems[0];
            }
            popupedToNextButton = GameObject.Find(toNextButton.gameObject.name).GetComponent<Button>();
            popupedCloseButton = GameObject.Find(closeButton.gameObject.name).GetComponent<Button>();
            tutorialImagesParent = popupSystem.popupedObject.transform.GetChild(0).gameObject;
            OnChangeItem();
        }

        /// <summary>
        /// Itemを変更した時
        /// </summary>
        void OnChangeItem()
        {
            // ボタンが表示可能かどうか判断し,ボタンを表示するかしないか決定する
            CheckButtonVibible(popupedToNextButton, Index.Next);
            CheckButtonVibible(popupedCloseButton, Index.Last);

            var soundManaegr = SoundManager.Instance;

            // 前の画面で再生されていた音声を停止して次の音声が存在する場合音声を再生させる
            soundManaegr.PlayWithFade(SoundManager.SoundTag.Voice, null);
            if (currentItem.useVoice)
            {
                soundManaegr.PlayWithFade(SoundManager.SoundTag.Voice, currentItem.VoiceClip);
                //audioSource.PlayOneShot(currentItem.VoiceClip);
            }
        }

        /// <summary>
        /// fading image
        /// </summary>
        void FadingImage(Index index)
        {
            switch (index)
            {
                case Index.Next:
                    iTween.MoveBy(tutorialImagesParent, iTween.Hash("amount", new Vector3(-width, 0), "time", fadingTime,"ignoretimescale",true));
                    break;
                case Index.Previous:
                    iTween.MoveBy(tutorialImagesParent, iTween.Hash("amount", new Vector3(width, 0), "time", fadingTime, "ignoretimescale", true));
                    break;
                case Index.Last:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Itemを適切な要素へ変更する
        /// </summary>
        /// <param name="direction"></param>
        void ChangeItem(Index direction)
        {
            var index = targetItems.IndexOf(currentItem);
            switch (direction)
            {
                case Index.Next:
                    currentItem = targetItems[++index];
                    break;
                case Index.Previous:
                    currentItem = targetItems[--index];
                    break;
                default:
                    throw new System.ArgumentException("Directionが適切に指定されていません.");
            }
        }

        /// <summary>
        /// Okボタンへのイベントハンドラ
        /// </summary>
        void OnPushOk()
        {
            var battleManager = BattleManager.Instance;
            battleManager.SetStateMachine(battleManager.m_StateMachine.PreviousStateWithoutPause);
        }

        /// <summary>
        /// 次へボタンのイベントハンドラ
        /// </summary>
        void OnPushNextButton()
        {
            ChangeItem(Index.Next);
            FadingImage(Index.Next);
            OnChangeItem();
        }

        /// <summary>
        /// ボタンが表示可能かどうかチェックし,trueならコンポーネントを有効に,falseなら無効にする
        /// /// </summary>
        /// <param name="button"></param>
        /// <param name="direction"></param>
        void CheckButtonVibible(Button button, Index direction)
        {
            switch (direction)
            {
                case Index.Next:
                    if (NextItem != null)
                    {
                        button.gameObject.SetActive(true);
                    }
                    else
                    {
                        button.gameObject.SetActive(false);
                    }
                    break;
                case Index.Last:
                    var index = targetItems.IndexOf(currentItem);
                    if (index == targetItems.Count - 1)
                    {
                        button.gameObject.SetActive(true);
                    }
                    else
                    {
                        button.gameObject.SetActive(false);
                    }
                    break;
                default:
                    throw new System.ArgumentException("Directionが適切に指定されていません.");
            }
        }
    }

    [Flags]
    public enum Subject
    {
        AboutPanels = 1,
        AboutAttack = 2,
        AboutPause = 4,
        CompletePanels = 8,
        FirstPanelOpen = 16,
        UniqueSkillAccumulated = 32,
        UsedUniqueSkill = 64,
        AboutUniqueSkills = 128,
        AboutTeleportSkill = 256,
        AboutTeleportSkill_2 = 512,
        AboutTeleportSkill_3 = 1024,
        FirstPanelOpen_2 = 2048,
        FirstPanelOpen_3 = 4096,
        FirstPanelOpen_4 = 8192,
        AllFlag = 16383,
    }
}
