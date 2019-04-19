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

        public Vector2 ImageSize
        {
            get
            {
                return new Vector2(width * multiplier, height * multiplier);
            }
        }

        public Vector2 FullScreenSize
        {
            get
            {
                return new Vector2(Screen.width, Screen.height);
            }
        }

        /// <summary>経過時間</summary>
        /// <value>The elapsed time.</value>
        public float ElapsedTime
        {
            get
            {
                return Time.realtimeSinceStartup - startTime;
            }
        }

        public Vector2 Spacing
        {
            get
            {
                if (isFullScreen)
                {
                    return FullScreenSize;
                }
                else
                {
                    return ImageSize;
                }
            }
        }
        #endregion

        #region Variables

        /// <summary>表示させるチュートリアルの素材群</summary>
        [SerializeField] TutorialItems tutorialObject;
        /// <summary>チュートリアル画像を生成するオブジェクト</summary>
        [SerializeField] GameObject tutorialImagesParent;
        /// <summary>game object of mask</summary>
        [SerializeField] RectTransform maskObject;
        /// <summary>itween animationに使う時間</summary>
        [SerializeField] float fadingTime = 10f;
        /// <summary>画像のサイズ倍率</summary>
        [SerializeField] float multiplier = 1f;
        /// <summary>ダブルタップ判定タイムリミット</summary>
        [SerializeField] float countTimeLimit;

        int myIndex = 0;
        float spacing = 0f;
        Vector3 vec = new Vector3();
        int targetIndex;
        Image currentImage;
        int doubleTapCounter;
        float startTime;
        bool isFullScreen;

        /// <summary>現在タッチしている対象</summary>
        TouchGestureDetector.TouchInfo touchInfoOnTouchBegin;
        /// <summary>現在対象となっている素材</summary>
        TutorialItems.TutorialItem currentItem;
        /// <summary>生成されたチュートリアル画像のリスト</summary>
        List<Image> popupedTutorialImages = new List<Image>();



        const float width = 424f;
        const float height = 837.4f;
        #endregion

        #region Methods

        private void Start()
        {
            // maskのイメージを指定サイズに変更
            maskObject.sizeDelta = ImageSize;

            // チュートリアル画像群生成
            var xPos = 0f;
            tutorialObject.Items.ForEach(item =>
            {
                var go = new GameObject();
                go.transform.parent = tutorialImagesParent.transform;
                go.tag = "Tips";
                var image = go.AddComponent<Image>();
                image.sprite = item.Sprite;
                image.rectTransform.sizeDelta = ImageSize;
                image.rectTransform.localPosition = new Vector3(xPos, 0, 0);
                xPos += ImageSize.x;
                image.rectTransform.localScale = Vector2.one;
                popupedTutorialImages.Add(image);
            });

            // 最初のチュートリアル素材に初期化
            currentItem = tutorialObject.Items.First();
            // 最初の画像を表示した時用のコールバック
            OnChangeItem();


            TouchGestureDetector.Instance.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        touchInfoOnTouchBegin = touchInfo;
                        if (doubleTapCounter == 0)
                        {
                            startTime = Time.realtimeSinceStartup;
                        }
                        doubleTapCounter++;

                        // 時間内に二回タップしたらコールバック
                        if (ElapsedTime < countTimeLimit)
                        {
                            if (doubleTapCounter == 2)
                            {
                                //callback
                                OnDoubleTap();
                                doubleTapCounter = 0;
                            }
                        }
                        else
                        {
                            doubleTapCounter = 0;
                        }
                        break;
                    case TouchGestureDetector.Gesture.FlickLeftToRight:
                    case TouchGestureDetector.Gesture.FlickRightToLeft:
                        OnFlick(gesture, touchInfoOnTouchBegin);
                        break;
                    default:
                        break;
                }


            });
        }

        /// <summary>
        /// ダブルタップを検知した時
        /// </summary>
        void OnDoubleTap()
        {
            // 対象のインデックス値を代入
            targetIndex = tutorialObject.Items.IndexOf(currentItem);

            if (isFullScreen)
            {
                isFullScreen = false;
                iTween.ValueTo(gameObject, iTween.Hash("from", FullScreenSize, "to", ImageSize, "time", fadingTime, "onupdatetarget", gameObject, "onupdate", "SyncImageSize", "ignoretimescale", true));
                iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 127f, "time", fadingTime, "onupdatetarget", gameObject, "onupdate", "SyncMaskPosition", "ignoretimescale", true));

                popupedTutorialImages.ForEach(image =>
                {
                    myIndex = popupedTutorialImages.IndexOf(image);
                    spacing = width * -(targetIndex - myIndex);
                    vec = new Vector3(spacing, 0);
                    iTween.MoveTo(image.gameObject, iTween.Hash("position", vec, "time", fadingTime, "islocal", true, "ignoretimescale", true));
                });

                currentImage = popupedTutorialImages.Find(i => i.sprite.name == currentItem.Sprite.name);
                myIndex = popupedTutorialImages.IndexOf(currentImage);
                spacing = width * -(targetIndex - myIndex);
                vec = new Vector3(spacing, 0);
                tutorialImagesParent.GetComponent<RectTransform>().localPosition = vec;

                // ボイスを止める
                TryPlayVoice();
            }
            else
            {
                isFullScreen = true;
                iTween.ValueTo(gameObject, iTween.Hash("from", ImageSize, "to", FullScreenSize, "time", fadingTime, "onupdatetarget", gameObject, "onupdate", "SyncImageSize", "ignoretimescale", true));
                iTween.ValueTo(gameObject, iTween.Hash("from", 127f, "to", 0f, "time", fadingTime, "onupdatetarget", gameObject, "onupdate", "SyncMaskPosition", "ignoretimescale", true));

                popupedTutorialImages.ForEach(image =>
                {
                    myIndex = popupedTutorialImages.IndexOf(image);
                    spacing = FullScreenSize.x * -(targetIndex - myIndex);
                    vec = new Vector3(spacing, 0);
                    iTween.MoveTo(image.gameObject, iTween.Hash("position", vec, "time", fadingTime, "islocal", true, "ignoretimescale", true));
                });

                currentImage = popupedTutorialImages.Find(i => i.sprite.name == currentItem.Sprite.name);
                myIndex = popupedTutorialImages.IndexOf(currentImage);
                spacing = FullScreenSize.x * -(targetIndex - myIndex);
                vec = new Vector3(spacing, 0);
                tutorialImagesParent.GetComponent<RectTransform>().localPosition = vec;

                // ボイスが存在したら再生
                TryPlayVoice();
            }
        }

        void OnFlick(TouchGestureDetector.Gesture gesture, TouchGestureDetector.TouchInfo touchInfo)
        {
            GameObject hitResult;
            if (touchInfo.HitDetection(out hitResult) && hitResult.tag == "Tips")
            {
                // 右フリックした時左側にチュートリアル画像が存在したら
                if (gesture == TouchGestureDetector.Gesture.FlickLeftToRight && PreviousItem != null)
                {
                    ChangeItem(Index.Previous);
                    FadingImage(Index.Previous);
                    OnChangeItem();
                }

                // 左フリックした時右側にチュートリアル画像が存在したら
                if (gesture == TouchGestureDetector.Gesture.FlickRightToLeft && NextItem != null)
                {
                    ChangeItem(Index.Next);
                    FadingImage(Index.Next);
                    OnChangeItem();
                }
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

        /// <summary>
        /// fading image
        /// </summary>
        void FadingImage(Index index)
        {
            var fromPosition = tutorialImagesParent.transform.localPosition;
            Vector3 toPosition;
            float amount = 0f;
            if (isFullScreen)
            {
                amount = FullScreenSize.x;
            }
            else
            {
                amount = ImageSize.x;
            }
            switch (index)
            {
                case Index.Next:
                    toPosition = tutorialImagesParent.transform.localPosition + new Vector3(-amount, 0, 0);
                    iTween.ValueTo(gameObject, iTween.Hash("from", fromPosition, "to", toPosition, "time", fadingTime, "onupdate", "SyncPosition", "onupdatetarget", gameObject, "ignoretimescale", true));
                    break;
                case Index.Previous:
                    toPosition = tutorialImagesParent.transform.localPosition + new Vector3(+amount, 0, 0);
                    iTween.ValueTo(gameObject, iTween.Hash("from", fromPosition, "to", toPosition, "time", fadingTime, "onupdate", "SyncPosition", "onupdatetarget", gameObject, "ignoretimescale", true));
                    break;
                case Index.Last:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Itemを変更した時
        /// </summary>
        void OnChangeItem()
        {
            TryPlayVoice();

            // 対象の画像以外透明にする
            popupedTutorialImages.ForEach(image =>
            {
#if true
                iTween.ValueTo(image.gameObject, iTween.Hash("from", Color.white, "to", Color.clear, "time", 1f, "onupdatetarget", gameObject, "onupdate", "SyncImageColor", "ignoretimescale", true));
#else
                if (image.sprite.name == currentItem.Sprite.name)
                {
                    image.color = Color.white;
                }
                else
                {
                    image.color = Color.clear;
                }
#endif
            });
        }

        /// <summary>
        /// 前の画面で再生されていた音声を停止して次の音声が存在する場合音声を再生させる
        /// </summary>
        void TryPlayVoice()
        {
            // 前の画面で再生されていた音声を停止して次の音声が存在する場合音声を再生させる
            var soundManaegr = SoundManager.Instance;
            SoundManager.Instance.StopWithFade(SoundManager.SoundTag.Voice);

            if (isFullScreen)
            {
                if (currentItem.useVoice)
                {
                    soundManaegr.PlayWithFade(SoundManager.SoundTag.Voice, currentItem.VoiceClip);
                }
            }
        }
        #endregion

        #region iTweenAnimateMethods

        void SyncPosition(Vector3 nextPosition)
        {
            tutorialImagesParent.transform.localPosition = nextPosition;
        }

        void SyncImageSize(Vector2 nextSize)
        {
            // maskのサイズも合わせる
            maskObject.sizeDelta = nextSize;

            popupedTutorialImages.ForEach(image => image.rectTransform.sizeDelta = nextSize);

            //// 対象の画像を見つけ出し対象の画像だけサイズを合わせる
            //var targetImage = popupedTutorialImages.Find(image => currentItem.Sprite.name == image.sprite.name);
            //targetImage.rectTransform.sizeDelta = nextSize;
        }

        void SyncMaskPosition(float nextYPosition)
        {
            maskObject.localPosition = new Vector3(maskObject.localPosition.x, nextYPosition, maskObject.localPosition.z);
        }

        /// <summary>
        /// animation of calor fading
        /// </summary>
        /// <param name="nextColor"></param>
        void SyncImageColor(Color nextColor)
        {
            // 対象の画像以外は透明に、対象の画像は表示させるアニメーション
            popupedTutorialImages.ForEach(image =>
            {
                var targetFlag = currentItem.Sprite.name == image.sprite.name;
                var nextItemFlag = NextItem != null && NextItem.Sprite.name == image.sprite.name;
                var previousItemFlag = PreviousItem != null && PreviousItem.Sprite.name == image.sprite.name;

                if (targetFlag)
                {
                    var red = 1 - nextColor.r;
                    var green = 1 - nextColor.g;
                    var blue = 1 - nextColor.b;
                    var alpha = 1 - nextColor.a;
                    var color = new Color((red < 0 ? 0 : red), (green < 0 ? 0 : green), (blue < 0 ? 0 : blue), (alpha < 0 ? 0 : alpha));

                    image.color = color;
                }
                else
                {
                    image.color = nextColor;
                }
            });
        }

        void AdjustmentImagesPosition(Vector3 nextVec)
        {
            tutorialImagesParent.GetComponent<RectTransform>().localPosition = nextVec;
        }
        #endregion
    }
}