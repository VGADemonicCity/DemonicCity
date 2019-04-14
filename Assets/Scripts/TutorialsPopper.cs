using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity
{
    public class TutorialsPopper : MonoBehaviour
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



        /// <summary>表示させるチュートリアルの素材群</summary>
        [SerializeField] TutorialItems tutorialObject;
        /// <summary>popup system</summary>
        [SerializeField] PopupSystem popupSystem;
        /// <summary>次へボタン</summary>
        [SerializeField] Button toNextButton;
        /// <summary>前へボタン</summary>
        [SerializeField] Button toPreviousButton;
        /// <summary>閉じるボタン</summary>
        [SerializeField] Button closeButton;

        /// <summary>現在対象となっている素材</summary>
        TutorialItems.TutorialItem currentItem;
        /// <summary>一つ前に対象になっていた素材</summary>
        TutorialItems.TutorialItem previousItem;
        /// <summary>popup materials</summary>
        List<PopupSystemMaterial> popupMaterials;

        const int width = 1080;
        const int height = 1920;


        private void Start()
        {
            popupMaterials = new List<PopupSystemMaterial>
            {
                new PopupSystemMaterial(OnPushNextButton,toNextButton.gameObject.name,false),
                new PopupSystemMaterial(OnPushPreviousButton,toPreviousButton.gameObject.name,false),
            };
        }

        public void Popup()
        {
            popupSystem.Popup();
            popupMaterials.ForEach(material => popupSystem.SubscribeButton(material));
            var imageSize = new Vector2(width, height);
            var xPos = 0;
            tutorialObject.Items.ForEach(item =>
            {
                var go = new GameObject();
                go = Instantiate(go, popupSystem.popupedObject.transform);
                var image = go.AddComponent<Image>();
                image.sprite = item.Sprite;
                image.rectTransform.sizeDelta = imageSize;
                image.rectTransform.position = new Vector2(xPos, 0);
                xPos += width;
            });
        }

        void FadingImage()
        {
            //iTween.MoveTo()
        }

        void Close()
        {

        }

        void OnPushNextButton()
        {
            ChangeItem(Index.Next);
            OnChangeItem();
        }

        void OnPushPreviousButton()
        {
            ChangeItem(Index.Previous);
            OnChangeItem();
        }

        /// <summary>
        /// Itemを変更した時
        /// </summary>
        void OnChangeItem()
        {
            // ボタンが表示可能かどうか判断し,ボタンを表示するかしないか決定する
            CheckButtonVibible(toNextButton, Index.Next);
            CheckButtonVibible(toPreviousButton, Index.Previous);
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
                    if (previousItem != null)
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

        ///// <summary>
        ///// 指定した要素が存在するかどうか
        ///// </summary>
        ///// <param name="direction"></param>
        ///// <returns></returns>
        //bool HasElement(Direction direction)
        //{
        //    var index = tutorialObject.Items.IndexOf(currentItem);
        //    switch (direction)
        //    {
        //        case Direction.Next:
        //            return ++index < tutorialObject.Items.Count-1;
        //        case Direction.Previous:
        //            return --index >= 0;
        //        default:
        //            throw new System.ArgumentException("Directionが適切に指定されていません.");
        //    }
        //}
    }
    enum Index
    {
        Next,
        Previous,
        Last,
    }
}
