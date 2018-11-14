using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{


    /// <summary>
    /// Panelの全体管理クラス
    /// </summary>
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] float m_waitTime = 3f;

        /// <summary>ファクトリークラス。Touch情報をもとに適切な処理を行ってくれる</summary>
        private TouchInfoFactory m_touchInfoFactory;
        private GameObject m_go;
        /// <summary>RaycastDetection</summary>
        private RaycastDetection m_raycastDetection;
        private TouchInfomation m_touchInfo;
        private Touch m_touch;
        private Flags m_flag = Flags.isWorking;

        private void Start()
        {
            m_raycastDetection = GetComponent<RaycastDetection>();
            InstantiatePanels ip = GetComponent<InstantiatePanels>();
            m_touchInfo = GetComponent<TouchInfomation>();
            ip.GeneratePanels(); //Panel生成処理
        }

        /// <summary>アップデートメソッド : Update method</summary>
        public void Update()
        {
            if (Input.touchCount > 0 && ((m_flag & Flags.PanelSelectPhase) == Flags.PanelSelectPhase)) //タッチカウント > 0 且つパネルセレクトフェーズのフラグが立っている時
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

        /// <summary>
        /// Waits the selecting panel.
        /// </summary>
        /// <returns>The selecting panel.</returns>
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

