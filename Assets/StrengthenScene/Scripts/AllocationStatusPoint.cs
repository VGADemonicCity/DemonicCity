using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DemonicCity.StrengthenScene
{
    public class AllocationStatusPoint : MonoBehaviour
    {
        Magia magia;
        //Statistics result = new Statistics();
        TouchGestureDetector m_touchGestureDetector;
        private int level, hitPoint, attack, defense;
        public TextMeshProUGUI levelText, hitPointText, attackText, defenseText;

        void Awake()
        {
            magia = Magia.Instance;
            m_touchGestureDetector = TouchGestureDetector.Instance;
        }

        void Start()
        {
            Debug.Log(magia.GetStats().m_hitPoint);

            //マギアの現在のステータスを取得
            level = magia.GetStats().m_level;
            hitPoint = magia.GetStats().m_hitPoint;
            attack = magia.GetStats().m_attack;
            defense = magia.GetStats().m_defense;
            
            //画面上に表示
            levelText.GetComponent<TextMeshProUGUI>().text = "" + level.ToString();
            hitPointText.GetComponent<TextMeshProUGUI>().text = "" + hitPoint.ToString();
            attackText.GetComponent<TextMeshProUGUI>().text = "" + attack.ToString();
            defenseText.GetComponent<TextMeshProUGUI>().text = "" + defense.ToString();

            

            /*
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        //PressedConfirm();
                        break;
                }
            });
            */
        }


        //ユーザーによるボタン押下時の処理
        /// <summary>
        /// 確定ボタンを押すと反映された割り振りポイントを確定し保存
        /// </summary>
        public void PressedConfirm()
        {
            Debug.Log("aaaa");
        }

        /// <summary>
        /// 中止ボタンを押すと反映されたポイントを編集前に初期化
        /// </summary>
        public void PressedReset()
        {
            Debug.Log("bbbb");
        }
    }
}
