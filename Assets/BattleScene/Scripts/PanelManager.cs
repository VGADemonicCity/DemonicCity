using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class PanelManager : MonoBehaviour
    {

        /// <summary>ファクトリークラス。Touch情報をもとに適切な処理を行ってくれる</summary>
        private TouchInfoFactory m_touchInfoFactory;
        private GameObject m_go;
        /// <summary>RaycastDetection</summary>
        private RaycastDetection m_raycastDetection;
        private TouchInfo m_touchInfo;

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
                m_go = m_touchInfo.GetTouchedGameObject(TouchPhase.Ended); // Ended時のゲームオブジェクトを取得する
                if (m_go != null && m_go.tag == "Panel")
                {
                    var tpbp = m_go.GetComponent<TouchPhaseBeganProcess>();
                    tpbp.Rotation();
                }
            }
        }
    }
}

