using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DemonicCity.StrengthenScene
{
    public class SceneManager : MonoBehaviour
    {
        Magia magia;
        TouchGestureDetector touchGestureDetector;

        /// <summary>ポップアップウィンドウ</summary>
        [SerializeField]
        GameObject[] popUpWindows = new GameObject[2];

        /// <summary>習得済みスキル</summary>
        private Magia.PassiveSkill passiveSkill;

        /// <summary>習得済みスキルの表示先</summary>
        [SerializeField]
        GameObject content;

        [SerializeField]
        GameObject skillDetailPrefab;

        /// <summary>スキルの説明テキスト</summary>
        private List<string> skillList = new List<string>();

        private GameObject skillDetail;

        [SerializeField]
        ScrollRect scrollRect;

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            passiveSkill = magia.MyPassiveSkill;
            scrollRect.verticalNormalizedPosition = 1f;
        }

        void Start()
        {
           
            skillList.Add("街破壊数1以上で発動\n街破壊数×攻撃力の1%\nを加算して攻撃");
            skillList.Add("街破壊数７以上で発動\n街破壊数×0.5%\n攻撃力防御力を上昇");
            popUpWindows[0].SetActive(false);
            popUpWindows[1].SetActive(false);

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    GameObject popUpButton;
                    touchInfo.HitDetection(out popUpButton);

                    if (popUpButton != null)
                    {
                        switch (popUpButton.name)
                        {
                            case ("SelectAttributeButton"):

                                popUpWindows[0].SetActive(true);
                                break;
                            case ("ShowSkillButton"):
                                popUpWindows[1].SetActive(true);
                                break;
                            case ("BackButton"):
                                popUpButton.gameObject.SetActive(false);
                                popUpWindows[0].SetActive(false);
                                popUpWindows[1].SetActive(false);
                                break;
                            case ("DevilsFist"):
                                PopUpSkillDetailWindow(0);
                                break;
                            case ("HighConcentrationMagicalAbsorption"):
                                PopUpSkillDetailWindow(1);
                                break;
                        }
                    }
                }
            });
        }
        /// <summary>スキルの説明を表示</summary>
        /// <param name="index">スキル名に対応した説明を指定</param>
        public void PopUpSkillDetailWindow(int index)
        {
            content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            skillDetail = Instantiate(skillDetailPrefab, content.transform);
            skillDetail.GetComponentInChildren<Text>().text = skillList[index];
        }
    }
}
