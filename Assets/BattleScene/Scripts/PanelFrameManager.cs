using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{

    [Flags]
    public enum Flag
    {
        /// <summary>右にいる時,0001</summary>
        Right = 1,
        /// <summary>真ん中にいる時,0010</summary>
        Middle = 2,
        /// <summary>左にいる時,0100</summary>
        Left = 4
    }

    public class PanelFrameManager : MonoBehaviour
    {
        /// <summary>The touch gesture detector.</summary>
        private TouchGestureDetector m_touchGestureDetector;
        /// <summary>フリック時のbit論理演算用</summary>
        int m_flag = 2;
        bool m_wait = true;

        void Awake()
        {
            m_touchGestureDetector = GetComponent<TouchGestureDetector>();
        }

        public void Start()
        {
            //m_touchGestureDetector.hitCheck = false; // isHitを不使用にしてどこでも検知できる様にする
           
            // UnityEvent機能を使ってメソッドを登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture) // タッチ情報が左右のフリックだったら
                {
                    case TouchGestureDetector.Gesture.FlickLeftToRight: // 右フリックの時
                        if ((m_flag & (int)Flag.Left) == 0 && m_wait) // 右にスワイプされた時左側にいなければ
                        {
                            StartCoroutine(Moving(Flag.Right)); // デフォルト引数を使い左にシフトさせる
                        }
                        break;
                    case TouchGestureDetector.Gesture.FlickRightToLeft: // 左フリックの時
                        if((m_flag & (int)Flag.Right) == 0 && m_wait) // 左にスワイプされた時右側にいなければ
                        {
                            StartCoroutine(Moving(Flag.Left)); // マイナス1を渡して右にシフトさせる
                        }
                        break;
                    case TouchGestureDetector.Gesture.Click:
                        Debug.Log("click呼ばれたよ");
                        break;
                    case TouchGestureDetector.Gesture.TouchBegin:
                        Debug.Log("begin呼ばれたよ");
                        break;
                    case TouchGestureDetector.Gesture.TouchEnd:
                        Debug.Log("end呼ばれたよ");
                        break;
                    case TouchGestureDetector.Gesture.TouchMove:
                        Debug.Log("move呼ばれたよ");
                        break;
                    case TouchGestureDetector.Gesture.TouchStationary:
                        Debug.Log("stationary呼ばれたよ");
                        break;
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// パネル枠をフリックに応じて移動させる
        /// </summary>
        /// <returns>The moving.</returns>
        /// <param name="flag">Flag.</param>
        IEnumerator Moving(Flag flag)
        {
            m_wait = false; //処理が終わるまで呼ばれない様にする
            switch (flag)
            {
                case Flag.Right: // もし右にフリックされたら
                    iTween.MoveBy(gameObject, iTween.Hash(("x"), 3.65f)); // 枠を右に移動
                    m_flag = m_flag << 1; // フラグを左にシフト
                    break;
                case Flag.Left: // もし左にフリックされたら
                    iTween.MoveBy(gameObject, iTween.Hash(("x"), -3.65f)); // 枠を左に移動
                    m_flag = m_flag >> 1; // フラグを右にシフト
                    break;
            }
            yield return new WaitForSeconds(2f); //2秒待つ
            m_wait = true; // 処理終了。またこのメソッドを呼べる様になる
        }
    }
}
