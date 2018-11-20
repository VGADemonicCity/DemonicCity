using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.StoryScene
{
    public class PutSentence : MonoBehaviour
    {

        /// <summary>/// 文字送り速度/// </summary>
        float charFeedSpeed = 0.1f;
        /// <summary>/// 表示用のTextComponent/// </summary>
        [SerializeField] Text text;
        /// <summary>/// /// </summary>
        TextStorage textContena = new TextStorage();
        /// <summary>/// 現在表示している文字列/// </summary>
        string sentence;
        /// <summary>/// /// </summary>
        int charCount = 0;
        /// <summary>/// /// </summary>
        bool end;
        public bool onoff;
        /// <summary>/// /// </summary>
        //public bool A()
        //{
        //    StartCoroutine(SentenceFeed());
        //    return end;
        //}
        //public IEnumerator SentenceFeed()
        //{
        //    end = false;
        //    text.text = "";
        //    for (charCount = 0; charCount < sentence.Length; charCount++)
        //    {
        //        if (end)
        //        {
        //            text.text = sentence;
        //            yield break;
        //        }
        //        text.text += sentence[charCount];
        //        yield return new WaitForSeconds(charFeedSpeed);
        //    }
        //}


        public void Totrue()
        {
            //charCount = sentence.Length;
            end = true;
            text.text = sentence;
            Debug.Log("true");
        }
        public bool A(string s)
        {
            Debug.Log("A");
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
        public IEnumerator SentenceFeed(string s)
        {
            Debug.Log("col");
            sentence = s;
            onoff = false;
            text.text = "";
            for (charCount = 0; charCount < sentence.Length; charCount++)
            {
                text.text += sentence[charCount];
                //end = false;
                if (end)
                {
                    text.text = sentence;
                    end = false;
                    break;
                }
                yield return new WaitForSeconds(charFeedSpeed);
            }
            onoff = true;
            //end = false;

        }
    }
}

