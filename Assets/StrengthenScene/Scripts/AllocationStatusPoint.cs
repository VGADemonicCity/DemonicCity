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
        private int level, hitPoint, attack, defense, durability, muscularStrength,
            knowledge, sense, charm, dignity;

        private int updatedHitPoint, updatedAttack, updatedDefense;

        private int allocationPoint, addCharmPoint;

        public TextMeshProUGUI levelText, hitPointText, attackText, defenseText,
            durabilityText, muscularStrengthText, knowledgeText, senseText,
            charmText, dignityText, allocationPointText, updatedHitPointText, updatedAttackText, updatedDefenseText,
            addCharmPointText;
        
        //画面表示用変数
        private int displayAllocationPoint, displayAddCharmPoint;

        public GameObject ConfirmAndResetButtons;

        void Awake()
        {
            magia = Magia.Instance;
            m_touchGestureDetector = TouchGestureDetector.Instance;
        }

        void Start()
        {
            ConfirmAndResetButtons.SetActive(false);//確定ボタンと中止ボタンを無効にしておく

            //マギアの現在のステータスを表示
            level = magia.GetStats().m_level;//レベル
            hitPoint = magia.GetStats().m_hitPoint;//体力
            attack = magia.GetStats().m_attack;//攻撃力
            defense = magia.GetStats().m_defense;//防御力
            durability = magia.GetStats().m_durability;//耐久
            muscularStrength = magia.GetStats().m_muscularStrength;//筋力
            knowledge = magia.GetStats().m_knowledge;//知識
            sense = magia.GetStats().m_sense;//感覚
            charm = magia.GetStats().m_charm;//魅力
            dignity = magia.GetStats().m_dignity;//威厳
            //allocationPoint = magia.AllocationPoint;    
            allocationPoint = 5;//デバッグ用
            displayAllocationPoint = allocationPoint;

            levelText.GetComponent<TextMeshProUGUI>().text = "" + level.ToString();
            hitPointText.GetComponent<TextMeshProUGUI>().text = "" + hitPoint.ToString();
            attackText.GetComponent<TextMeshProUGUI>().text = "" + attack.ToString();
            defenseText.GetComponent<TextMeshProUGUI>().text = "" + defense.ToString();
            durabilityText.GetComponent<TextMeshProUGUI>().text = "" + durability.ToString();
            muscularStrengthText.GetComponent<TextMeshProUGUI>().text = "" + muscularStrength.ToString();
            knowledgeText.GetComponent<TextMeshProUGUI>().text = "" + knowledge.ToString();
            senseText.GetComponent<TextMeshProUGUI>().text = "" + sense.ToString();
            charmText.GetComponent<TextMeshProUGUI>().text = "" + charm.ToString();
            dignityText.GetComponent<TextMeshProUGUI>().text = "" + dignity.ToString();

            
            //編集中のステータスを表示
            updatedHitPoint = hitPoint;
            updatedAttack = attack;
            updatedDefense = defense;

            updatedHitPointText.GetComponent<TextMeshProUGUI>().text = "" + updatedHitPoint.ToString();
            updatedAttackText.GetComponent<TextMeshProUGUI>().text = "" + updatedAttack.ToString();
            updatedDefenseText.GetComponent<TextMeshProUGUI>().text = "" + updatedDefense.ToString();

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

        private void Update()
        {
            if(displayAllocationPoint != allocationPoint)
                displayAllocationPoint = allocationPoint;
            
            allocationPointText.text = displayAllocationPoint.ToString();

            if (displayAddCharmPoint != addCharmPoint)
                displayAddCharmPoint = addCharmPoint;

            addCharmPointText.text = displayAddCharmPoint.ToString();

        }

        //ユーザーによるボタン押下時の処理
        /// <summary>
        /// 確定ボタンを押すと反映された割り振りポイントを確定し保存
        /// </summary>
        public void PressedConfirm()
        {
            magia.Update();
        }

        /// <summary>
        /// 中止ボタンを押すと反映されたポイントを編集前に初期化
        /// </summary>
        public void PressedReset()
        {
            Debug.Log("bbbb");
        }

        //+ボタンを押すと割り振りポイントが1減り、指定の固有ステータスポイントが追加される
        public void AddCharmPoint()
        {
            if (allocationPoint > 0)
            {
                allocationPoint -= 1;
                addCharmPoint += 3;
                addCharmPointText.GetComponent<TextMeshProUGUI>().text = "+" + addCharmPoint.ToString();

                ConfirmAndResetButtons.SetActive(true);//割り振られた際に確定ボタンと中止ボタンを有効にする
            }   
        }
        //-ボタンを押すと指定の固有ステータスポイントが減り、割り振りポイントが1増える
        public void ReductionCharmPoint()
        {
            if (addCharmPoint > 0)
            {
                addCharmPoint -= 3;
                allocationPoint += 1;
                
                if (addCharmPoint <= 0)
                {
                    addCharmPoint = 0;

                }
            }
        }
    }
}
