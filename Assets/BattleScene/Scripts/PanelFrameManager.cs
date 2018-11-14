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
        /// <summary>タッチ情報格納オブジェクト : Touch information strorage object</summary>
        private Touch m_touch;
        /// <summary>raycastで取ってきたgameObject</summary>
        private GameObject m_go;
        /// <summary>フリック時のbit論理演算用</summary>
        private int m_flag = 2;
        private bool m_wait = true;
        private RaycastDetection m_raycastDetection;
        /// <summary>フリック反応感度</summary>
        [SerializeField] private float m_flickSensitivity = 100f;


        public void Start()
        {
            m_raycastDetection = GetComponent<RaycastDetection>();
        }

        /// <summary>アップデートメソッド : Update method</summary>
        public void Update()
        {

            if (Input.touchCount > 0) //タッチされている＆パネルの処理を行っていない場合タッチ処理を行う
            {
                m_touch = Input.GetTouch(0); //タッチ情報の取得 : Acquire touch information.
                m_go = m_raycastDetection.DetectHitGameObject(m_touch.position); //指定レイヤーのオブジェクトのみレイキャストしてくる
                if (m_go != null && m_go.tag == "PanelFrame" && m_wait)
                {
                    StartCoroutine(Moving());
                }
            }
        }

        IEnumerator Moving()
        {
            if (m_touch.deltaPosition.x > m_flickSensitivity && (m_flag & (int)Flag.Left) == 0) //右にスワイプされた時左側にいなければ
            {
                iTween.MoveBy(m_go, iTween.Hash(("x"), 3.65f));
                m_flag = m_flag << 1;
                Debug.Log("m_flag is :" + m_flag);
                m_wait = false;
                yield return new WaitForSeconds(2f);
                m_wait = true;
            }
            else if (m_touch.deltaPosition.x < -m_flickSensitivity && (m_flag & (int)Flag.Right) == 0)//左にスワイプされた時右側にいなければ
            {
                iTween.MoveBy(m_go, iTween.Hash(("x"), -3.65f));
                m_flag = m_flag >> 1;
                Debug.Log("m_flag is :" + m_flag);
                m_wait = false;
                yield return new WaitForSeconds(2f);
                m_wait = true;
            }
        }
    }
}
