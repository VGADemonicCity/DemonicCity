using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DemonicCity.StoryScene
{
    public class ImageFeeder : MonoBehaviour
    {
        [SerializeField] Image targetImage;
        [SerializeField] Sprite feedSprite;

        [Header("送り始めるまでの時間")]
        [SerializeField] float startTiming = 0f;
        [Header("再生にかける時間")]
        [SerializeField] float feedingTime = 0f;
        [Header("送り終わってからの待ち時間")]
        [SerializeField] float afterTime = 0f;

        [Header("画像送り後の処理")]
        [SerializeField] UnityEvent AfterEvent;
        delegate void Process();


        private void Awake()
        {
            Reset();
        }

        void Start()
        {
            StartCoroutine(WaitProcess(startTiming, FeedImage));
        }

        // Update is called once per frame
        void Update()
        {

        }


        /// <summary>
        /// ポジションのリセット。画面下にイメージを配置する
        /// </summary>
        void Reset()
        {
            targetImage.sprite = feedSprite;
            RectTransform targetRect = targetImage.rectTransform;
            Debug.Log(targetRect.sizeDelta.y + ":::" + Screen.height);
            float initY = (targetRect.sizeDelta.y + Screen.height) / 2;
            targetRect.localPosition = new Vector3(0, -initY, targetRect.localPosition.z);
        }

        /// <summary>
        /// feedingTimeに合うように画像を送る
        /// </summary>
        void FeedImage()
        {
            StartCoroutine(FeedImage(feedingTime));
        }



        IEnumerator FeedImage(float time)
        {
            RectTransform targetRect = targetImage.rectTransform;
            //送る量
            float targetY = Screen.height + targetRect.sizeDelta.y;
            targetY *= 1.1f;//遊び
            //秒間送り量
            float feedPS = targetY / time;
            //送った量
            float feededY = 0f;
            //現フレームで送る量
            float feedValue = 0f;

            while (targetY >= feededY)
            {
                feedValue = Time.deltaTime * feedPS;
                feededY += feedValue;
                targetRect.localPosition += new Vector3(0, feedValue, 0);
                yield return null;
            }
            AfterProcess();
        }



        /// <summary>
        /// 画像送り後の処理
        /// </summary>
        void AfterProcess()
        {
            StartCoroutine(WaitProcess(afterTime, AfterEvent));
        }


        /// <summary>
        /// 任意の時間待ってから処理を実行する
        /// </summary>
        /// <param name="time"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        IEnumerator WaitProcess(float time, Process process)
        {
            yield return new WaitForSecondsRealtime(time);
            process();
        }
        /// <summary>
        /// 任意の時間待ってから処理を実行する
        /// </summary>
        /// <param name="time"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        IEnumerator WaitProcess(float time, UnityEvent process)
        {
            yield return new WaitForSecondsRealtime(time);
            process.Invoke();
        }

    }
}