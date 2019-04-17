using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 
    /// </summary>
    public class TutorialInPauseMenu : MonoBehaviour
    {
        #region Propaties
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
        #endregion

        #region Variables

        /// <summary>表示させるチュートリアルの素材群</summary>
        [SerializeField] TutorialItems tutorialObject;
        /// <summary>現在対象となっている素材</summary>
        TutorialItems.TutorialItem currentItem;
        /// <summary>チュートリアル画像を生成するオブジェクトの親</summary>
        GameObject tutorialImagesParent;
        /// <summary>現在タッチしている対象</summary>
        TouchGestureDetector.TouchInfo currentTouchInfo;

        const float width = 424f;
        const float height = 837.4f;
        #endregion

        #region Methods

        private void Start()
        {
            var imageSize = new Vector2(width, height);
            var xPos = 0f;
            tutorialObject.Items.ForEach(item =>
            {
                var go = new GameObject();
                go.transform.parent = tutorialImagesParent.transform.GetChild(0);
                var image = go.AddComponent<Image>();
                image.sprite = item.Sprite;
                image.rectTransform.sizeDelta = imageSize;
                image.rectTransform.position = new Vector2(xPos + (width * 0.5f), 0);
                xPos += width;
                image.rectTransform.localScale = Vector2.one;
            });

            TouchGestureDetector.Instance.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (BattleManager.Instance.m_StateMachine.m_State != BattleManager.StateMachine.State.Pause)
                {
                    return;
                }

                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        currentTouchInfo = touchInfo;
                        break;
                    case TouchGestureDetector.Gesture.Click:
                        break;
                    case TouchGestureDetector.Gesture.FlickTopToBottom:
                        break;
                    case TouchGestureDetector.Gesture.FlickBottomToTop:
                        break;
                    case TouchGestureDetector.Gesture.FlickLeftToRight:
                        break;
                    case TouchGestureDetector.Gesture.FlickRightToLeft:
                        break;
                    default:
                        break;
                }

            });
        }

        void OnFlick(TouchGestureDetector.Gesture gesture, TouchGestureDetector.TouchInfo touchInfo)
        {
            GameObject hitResult;
            if(touchInfo.HitDetection(out hitResult))
            {
                hitResult.
            }
        }


        #endregion
    }
}