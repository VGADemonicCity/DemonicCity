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
        /// <summary>itween animationに使う時間</summary>
        [SerializeField] float fadingTime = .5f;

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
        /// <summary>次へボタン</summary>
        Button popupedToNextButton;
        /// <summary>チュートリアル画像を生成するオブジェクトの親</summary>
        GameObject tutorialImagesParent;

        AudioSource audioSource;

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
                var go = new GameObject();
                go.transform.parent = popupSystem.popupedObject.transform.GetChild(0);
            }

            if (currentItem.useVoice)
            {
                //SoundManager.Instance.PlayWithFade(SoundManager.SoundTag.SE, clip);
            }
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
        /// Itemを変更した時
        /// </summary>
        void OnChangeItem()
        {



            var soundManaegr = SoundManager.Instance;

            // 前の画面で再生されていた音声を停止して次の音声が存在する場合音声を再生させる
            soundManaegr.PlayWithFade(SoundManager.SoundTag.Voice, null);
            if (currentItem.useVoice)
            {
                soundManaegr.PlayWithFade(SoundManager.SoundTag.Voice, currentItem.VoiceClip);
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
                    iTween.MoveBy(tutorialImagesParent, iTween.Hash("amount", new Vector3(-width, 0), "time", fadingTime));
                    break;
                case Index.Previous:
                    iTween.MoveBy(tutorialImagesParent, iTween.Hash("amount", new Vector3(width, 0), "time", fadingTime));
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
            var index = tutorialObject.Items.IndexOf(currentItem);
            switch (direction)
            {
                case Index.Next:
                    currentItem = tutorialObject.Items[++index];
                    break;
                case Index.Previous:
                    currentItem = tutorialObject.Items[--index];
                    break;
                default:
                    throw new System.ArgumentException("Directionが適切に指定されていません.");
            }
        }

        void OnPushOk()
        {
            var battleManager = BattleManager.Instance;
            battleManager.SetStateMachine(battleManager.m_StateMachine.m_PreviousState);
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
                case Index.Previous:
                    if (PreviousItem != null)
                    {
                        button.gameObject.SetActive(true);
                    }
                    else
                    {
                        button.gameObject.SetActive(false);
                    }
                    break;
                case Index.Last:
                    var index = tutorialObject.Items.IndexOf(currentItem);
                    if (index == tutorialObject.Items.Count - 1)
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
