using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DemonicCity
{
    public class UIFader : MonoBehaviour
    {
        [SerializeField] Image targetImage;
        [SerializeField] Text targetText;
        [SerializeField] Sprite fadeSprite;

        //[Header("フェードし始めるまでの時間")]
        //[SerializeField] float startTiming = 0f;
        [Header("フェードにかける時間")]
        [SerializeField] float fadeingTime = 0f;
        [Header("表示する時間")]
        [SerializeField] float showTime = 0f;

        //[Header("フェード後の処理")]
        //[SerializeField] UnityEvent AfterEvent;
        delegate void Process<T>(T ui) where T : Graphic;
        delegate void Process();



        private void Awake()
        {
            Reset();
        }


        /// <summary>
        /// リセット。TextとImageを透明にする
        /// </summary>
        void Reset()
        {
            targetImage.sprite = fadeSprite;
            targetImage.color = Color.clear;
            targetText.color = Color.clear;
        }

        /// <summary>
        /// feedingTimeに合うように画像を送る
        /// </summary>
        void FadeUI<T>(T ui, bool isOut) where T : Graphic
        {
            StartCoroutine(FeedUI(fadeingTime, ui, isOut));
        }

        public void FadeImage(bool isOut)
        {
            FadeUI(targetImage, isOut);
        }

        public void FadeText(bool isOut)
        {
            FadeUI(targetText, isOut);
        }
        //public void FadeUIs(bool isOut, UnityEvent events = null)
        //{
        //    FadeUI(targetImage, isOut, events);
        //    FadeUI(targetText, isOut, events);
        //}

        public void FadeInUI()
        {
            FadeUI(targetImage, false);
            FadeUI(targetText, false);
        }

        void FadeOutUI()
        {
            FadeUI(targetImage, true);
            FadeUI(targetText, true);
        }

        void EndFade(bool isOut)
        {
            if (isOut)
            {
                SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Title);
            }
            else
            {
                StartCoroutine(WaitProcess(showTime, FadeOutUI));
            }
        }

        IEnumerator FeedUI<T>(float time, T _ui, bool isOut) where T : Graphic
        {
            T ui = _ui;
            Color tmpColor;
            float a;
            float targetAlpha;

            if (isOut)
            {
                a = 1f;
                targetAlpha = 0f;
                tmpColor = Color.white;
            }
            else
            {
                a = 0f;
                targetAlpha = 1f;
                tmpColor = Color.clear;
            }
            float diff = targetAlpha - a;
            RectTransform targetRect = targetImage.rectTransform;
            //現フレームで変更するアルファ値
            float fadeValue = 0f;
            int c = 0;
            while (a + targetAlpha < 2
                && 0 < a + targetAlpha)
            {
                fadeValue = Time.deltaTime * diff / time;
                a += fadeValue;
                tmpColor = new Color(1f, 1f, 1f, a);
                ui.color = tmpColor;
                yield return null;
                c++;
                if (c > 100000)
                {
                    Debug.LogError("over");
                    yield break;
                }
            }
            EndFade(isOut);
        }



        /// <summary>
        /// 画像送り後の処理
        /// </summary>
        //void AfterProcess()
        //{
        //    StartCoroutine(WaitProcess(showTime, AfterEvent));
        //}
        //void AfterProcess(UnityEvent afterEvent)
        //{
        //    StartCoroutine(WaitProcess(afterTime, afterEvent));
        //}

        /// <summary>
        /// 任意の時間待ってから処理を実行する
        /// </summary>
        /// <param name="time"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        //IEnumerator WaitProcess<T>(float time, Process<T> process, T ui) where T : Graphic
        //{
        //    yield return new WaitForSecondsRealtime(time);
        //    process(ui);
        //}
        /// <summary>
        /// 任意の時間待ってから処理を実行する
        /// </summary>
        /// <param name="time"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        IEnumerator WaitProcess(float time, Process process)
        {
            yield return new WaitForSecondsRealtime(time);
            process.Invoke();
        }
    }
}