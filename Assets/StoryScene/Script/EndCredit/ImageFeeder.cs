using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.StoryScene
{
    public class ImageFeeder : MonoBehaviour
    {
        [SerializeField] Image targetImage;
        [SerializeField] Sprite feedImage;

        [Header("送り始めるまでの時間")]
        [SerializeField] float startTiming = 0f;
        [Header("再生にかける時間")]
        [SerializeField] float feedingTime = 0f;
        [Header("送り終わってからの待ち時間")]
        [SerializeField] float afterTime = 0f;


        delegate void Process();
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        /// <summary>
        /// ポジションのリセット。画面下にイメージを配置する
        /// </summary>
        void PositionReset()
        {

        }

        /// <summary>
        /// 引数の秒数に合うように画像を送る
        /// </summary>
        /// <param name="time">送りきるのにかける時間</param>
        void FeedImage(float time)
        {

        }

        /// <summary>
        /// 画像送り後の処理
        /// </summary>
        void AfterProcess()
        {

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

    }
}