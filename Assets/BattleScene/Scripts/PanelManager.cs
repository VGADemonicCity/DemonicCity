using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 各種フラグ用のenumビットフィールド表現
    /// </summary>
    [Flags]
    enum Flags
    {
        isWorking = 1,
        Dummy = 2,
        Dummy2 = 4,
        All = (isWorking | Dummy | Dummy2) // 要素全て
    }

    /// <summary>
    /// Panelの全体管理クラス
    /// </summary>
    public class PanelManager : MonoBehaviour
    {

        /// <summary>ファクトリークラス。Touch情報をもとに適切な処理を行ってくれる</summary>
        private TouchInfoFactory m_touchInfoFactory;
        /// <summary>
        /// 
        /// </summary>
        private GameObject m_go;
        /// <summary>RaycastDetection</summary>
        private RaycastDetection m_raycastDetection;
        private TouchInfo m_touchInfo;
        private Touch m_touch;
        [SerializeField] private float m_waitTime = 3f;
        private Flags m_flag = Flags.isWorking;

        private void Start()
        {
            m_raycastDetection = GetComponent<RaycastDetection>();
            InstantiatePanels ip = GetComponent<InstantiatePanels>();
            m_touchInfo = GetComponent<TouchInfo>();
            ip.GeneratePanels(); //Panel生成処理
        }

        /// <summary>アップデートメソッド : Update method</summary>
        public void Update()
        {
            if (Input.touchCount > 0) //タッチされている＆パネルの処理を行っていない場合タッチ処理を行う
            {
                m_touch = Input.GetTouch(0);
                m_go = m_touchInfo.GetTouchedGameObject(TouchPhase.Ended); // Ended時のゲームオブジェクトを取得する
                if (m_touch.phase == TouchPhase.Ended && m_go != null && m_go.tag == "Panel" &&  (m_flag & Flags.isWorking) == Flags.isWorking)
                {
                    var tpbp = m_go.GetComponent<TouchPhaseBeganProcess>();
                    tpbp.Rotation();
                    StartCoroutine(WaitSelectingPanel());
                }
            }
        }

        IEnumerator WaitSelectingPanel()
        {
            m_flag = m_flag ^ Flags.isWorking; //isWorkingをoffにする
            Debug.Log("オフに戻した後 :" + m_flag);
            yield return new WaitForSeconds(m_waitTime);
            Debug.Log("オンにする前 :" + m_flag);
            m_flag = m_flag | Flags.isWorking; //isWorkingをonにする
            Debug.Log("オンにした後 :" + m_flag);
        }
    }
}

