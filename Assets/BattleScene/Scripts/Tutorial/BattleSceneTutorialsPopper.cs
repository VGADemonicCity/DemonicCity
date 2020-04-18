using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.Battle
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
        public bool isPlayingTutorialAnimation { get; set; }


        /// <summary>表示させるチュートリアルの素材群</summary>
        [SerializeField] TutorialItems tutorialObject;
        /// <summary>次へボタン</summary>
        [SerializeField] Button toNextButton;
        /// <summary>閉じるボタン</summary>
        [SerializeField] Button closeButton;
        /// <summary>itween animationに使う時間</summary>
        [SerializeField] float fadingTime = .1f;
        [SerializeField] iTween.EaseType easeType = iTween.EaseType.easeInOutBack;

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
        TutorialInPauseMenu tutorialInPauseMenu;
        /// <summary>一時的にVector3を保存しておく箱</summary>
        Vector3 vectorBuffer = new Vector3();
        Subject targetSubject = new Subject();

        const int width = 1080;
        const int height = 1920;

        private void Start()
        {
            popupSystem = GetComponent<PopupSystem>();

            popupMaterials = new List<PopupSystemMaterial>()
            {
                new PopupSystemMaterial(OnPushNextButton,toNextButton.gameObject.name,false),
                new PopupSystemMaterial(OnPushClose, closeButton.gameObject.name, false),
            };
        }

        /// <summary>
        /// subject
        /// </summary>
        /// <param name="subject"></param>
        public void Popup(Subject subject)
        {
            targetSubject = subject;
            BattleManager.Instance.SetStateMachine(BattleManager.StateMachine.State.Tutorial);
            popupSystem.Popup();
            popupMaterials.ForEach(material => popupSystem.SubscribeButton(material));
            targetItems = tutorialObject.Items.FindAll((item) => item.subject == (item.subject & subject));
            targetItems = targetItems.OrderBy(item => item.subject).ToList();
            var imageSize = new Vector2(width, height);
            float xPos = 0;
            targetItems.ForEach(item =>
            {
                var go = new GameObject();
                go.transform.parent = popupSystem.popupedObject.transform.GetChild(0);
                var image = go.AddComponent<Image>();
                image.sprite = item.Sprite;
                Debug.Log(item.subject);
                image.rectTransform.sizeDelta = imageSize;
                image.rectTransform.localPosition = new Vector2(xPos, 0);
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
            tutorialInPauseMenu = popupSystem.popupedObject.GetComponent<TutorialInPauseMenu>();
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
            var fromPosition = tutorialImagesParent.transform.localPosition;
            Vector3 toPosition;
            switch (index)
            {
                case Index.Next:
                    toPosition = tutorialImagesParent.transform.localPosition + new Vector3(-width, 0, 0);
                    iTween.ValueTo(gameObject, iTween.Hash(
                        "from", fromPosition,
                        "to", toPosition,
                        "time", fadingTime,
                        "onstart", "OnStartTutorialWindowAnimation",
                        "onstarttarget", gameObject,
                        "onupdate", "SyncPosition",
                        "onupdatetarget", gameObject,
                        "oncomplete", "OnCompleteTutorialWindowAnimation",
                        "oncompletetarget", gameObject,
                        "easetype", easeType,
                        "ignoretimescale", true));
                    //tutorialImagesParent.transform.localPosition += new Vector3(-width, 0, 0);
                    break;
                case Index.Previous:
                    toPosition = tutorialImagesParent.transform.localPosition + new Vector3(+width, 0, 0);
                    iTween.ValueTo(gameObject, iTween.Hash(
                    "from", fromPosition,
                    "to", toPosition,
                    "time", fadingTime,
                    "onupdate", "SyncPosition",
                    "onupdatetarget", gameObject,
                    "onstart", "OnStartTutorialWindowAnimation",
                    "onstarttarget", gameObject,
                    "onupdate", "SyncPosition",
                    "onupdatetarget", gameObject,
                    "oncomplete", "OnCompleteTutorialWindowAnimation",
                    "oncompletetarget", gameObject,
                    "easetype", easeType,
                    "ignoretimescale", true));
                    //tutorialImagesParent.transform.localPosition += new Vector3(width, 0, 0);

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
        /// Closeボタンへのイベントハンドラ
        /// </summary>
        void OnPushClose()
        {
            if (tutorialInPauseMenu != null)
            {
                if (tutorialInPauseMenu.isPlayingTutorialAnimation)
                {
                    return;
                }
            }
            if (isPlayingTutorialAnimation)
            {
                return;
            }
            var battleManager = BattleManager.Instance;
            if (targetSubject == Subject.FirstPanelOpen_4)
            {
                battleManager.SetStateMachine(BattleManager.StateMachine.State.EnemyAttack);
            }
            else
            {
                battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
            }
            Destroy(popupSystem.popupedObject.transform.parent.gameObject);
        }

        /// <summary>
        /// 次へボタンのイベントハンドラ
        /// </summary>
        void OnPushNextButton()
        {
            if (isPlayingTutorialAnimation)
            {
                return;
            }
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

        #region iTweenAnimateMethods
        void OnStartTutorialWindowAnimation()
        {
            isPlayingTutorialAnimation = true;
        }

        void OnCompleteTutorialWindowAnimation()
        {
            isPlayingTutorialAnimation = false;
        }


        void SyncPosition(Vector3 nextPosition)
        {
            tutorialImagesParent.transform.localPosition = nextPosition;
        }
        #endregion
    }

    [Flags]
    public enum Subject
    {
        AboutPanels = 1,
        AboutPanelFrame = 2,
        AboutAttack = 4,
        AboutPause = 8,
        CompletePanels = 16,
        FirstPanelOpen = 32,
        UniqueSkillAccumulated = 64,
        UsedUniqueSkill = 128,
        AboutUniqueSkills = 256,
        AboutTeleportSkill = 512,
        AboutTeleportSkill_2 = 1024,
        AboutTeleportSkill_3 = 2048,
        FirstPanelOpen_2 = 4096,
        FirstPanelOpen_3 = 8192,
        FirstPanelOpen_4 = 16384,
        AllFlag = 32767,
    }
}
