using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DemonicCity
{
    public class PutSentence : MonoBehaviour
    {
        /// <summary>/// 文字送り速度/// </summary>
        float charFeedSpeed = 0.1f;
        /// <summary>/// 表示用のTextComponent/// </summary>
        [SerializeField] TMP_Text text;
        /// <summary>/// /// </summary>
        TextStorage textContena = new TextStorage();
        /// <summary>/// 現在表示している文字列/// </summary>
        string sentence;
        /// <summary>/// /// </summary>
        int charCount = 0;
        /// <summary>コルーチンが終了しているか </summary>
        public bool end=false;
        /// <summary>全文表示しているか</summary>
        public bool onoff;
        /// <summary>コルーチンを保存する</summary>
        IEnumerator feedCoroutine;


        void Awake()
        {
            text.text = "";
        }
        /// <summary>/// コルーチンを終了のフラグを立て、全文表示する/// </summary>
        public void Totrue()
        {
            end = true;
            text.text = sentence;
            //Debug.Log("true");
        }
        /// <summary>フラグに応じて文字送りを始めるか、文字送りの終了を通知する</summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool A(string s)
        {
            //Debug.Log("A");
            if (end)
            {
                onoff = true;
            }
            else
            {
                StartCoroutine(SentenceFeed(s));
            }
            return onoff;
        }
        //public IEnumerator aSentenceFeed(string s)
        //{
        //    //Debug.Log("col");
        //    sentence = s;
        //    onoff = false;
        //    text.text = "";
        //    for (charCount = 0; charCount < sentence.Length; charCount++)
        //    {
        //        text.text += sentence[charCount];
        //        //end = false;
        //        if (end)
        //        {
        //            text.text = sentence;
        //            end = false;
        //            break;
        //        }
        //        yield return new WaitForSeconds(charFeedSpeed);
        //    }
        //    onoff = true;
        //    //end = false;

        //}










        /// <summary>引数のStringを一文字ずつ表示する。endがtrueならコルーチンが終了</summary>
        public IEnumerator SentenceFeed(string s)
        {
            sentence = s;
            text.text = "";
            for (charCount=0;charCount<sentence.Length;charCount++)
            {
                text.text += sentence[charCount];

                yield return new WaitForSeconds(charFeedSpeed);
            }
            end = true;
        }

        /// <summary>テキストをすべて表示する。</summary>
        public void FullTexts()
        {
            StopCoroutine(feedCoroutine);
            text.text = sentence;
            end = true;
        }
        /// <summary>コルーチンを開始</summary>
        public bool CallSentence(string s)
        {
            if (end)
            {
                
            }
            else
            {
                feedCoroutine = SentenceFeed(s);
                StartCoroutine(feedCoroutine);
                end = false;
            }
            
            return end;
        }

    }
}

