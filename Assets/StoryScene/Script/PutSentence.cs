using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DemonicCity.StoryScene
{
    public class PutSentence : MonoBehaviour
    {
        /// <summary>/// 文字送り速度/// </summary>
        float charFeedSpeed = 0.1f;
        /// <summary>/// 表示用のTextComponent/// </summary>
        [SerializeField] TMP_Text text;
        /// <summary>/// /// </summary>
        //TextStorage textContena = new TextStorage();
        /// <summary>/// 現在表示している文字列/// </summary>
        string sentence;
        /// <summary>/// /// </summary>
        int charCount = 0;
        /// <summary>コルーチンが終了しているか </summary>
        bool end = true;
        ///// <summary>全文表示しているか</summary>
        //public bool onoff;
        /// <summary>コルーチンを保存する</summary>
        IEnumerator feedCoroutine;
        public bool End { get { return end; } set { end = value; } }

        SoundManager soundM;

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            text.text = "";
            soundM = SoundManager.Instance;
        }


        /// <summary>引数のStringを一文字ずつ表示する。endがtrueならコルーチンが終了</summary>
        public IEnumerator SentenceFeed(string s)
        {
            sentence = s;
            text.text = "";
            for (charCount = 0; charCount < sentence.Length; charCount++)
            {
                text.text += sentence[charCount];

                yield return new WaitForSeconds(charFeedSpeed);
            }
            End = true;
        }

        /// <summary>テキストをすべて表示する。</summary>
        public void FullTexts()
        {
            if (feedCoroutine != null) { StopCoroutine(feedCoroutine); }
            text.text = sentence;
            End = true;
        }
        /// <summary>コルーチンを開始</summary>
        public void CallSentence(string s, AudioClip clip)
        {
            if (End)
            {
                PlayVoice(clip);
                feedCoroutine = SentenceFeed(s);
                StartCoroutine(feedCoroutine);
                End = false;
            }
            else
            {
                FullTexts();
            }
        }

        void PlayVoice(AudioClip clip)
        {
            if (clip != null)
            {
                isVoice = true;
                voiceEnd = false;
                soundM.PlayWithFade(SoundManager.SoundTag.Voice, clip);
                StartCoroutine(VoiceCheck(clip.length));
            }
            else
            {
                voiceEnd = true;
            }
        }

        IEnumerator VoiceCheck(float length)
        {
            isVoice = false;
            float voiceTime = 0;
            while (voiceTime < length)
            {
                voiceTime += Time.deltaTime;
                if (isVoice)
                {
                    yield break;
                }
                yield return null;
            }
            voiceEnd = true;
        }
        bool isVoice;
        public bool voiceEnd = false;
    }
}

