using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace DemonicCity.StrengthenScene
{
    public class AllocationStatusPoint : MonoBehaviour
    {
        Magia magia;
        //Statistics result = new Statistics();
        TouchGestureDetector m_touchGestureDetector;
        //現在の色霊

        /// <summary>レベル</summary>
        private int level;

        /// <summary>現在の体力</summary>
        private int hitPoint;

        /// <summary>現在の攻撃力</summary>
        private int attack;

        /// <summary>現在の防御力</summary>
        private int defense;

        /// <summary>変動後の体力</summary>
        private int updatedHitPoint;

        /// <summary>変動後の攻撃力</summary>
        private int updatedAttack;

        /// <summary>変動後の防御力</summary>
        private int updatedDefense;

        /// <summary>現在の耐久値</summary>
        private int durability;

        /// <summary>現在の筋力値</summary>
        private int muscularStrength;

        /// <summary>現在の知識値</summary>
        private int knowledge;

        /// <summary>現在の感覚値</summary>
        private int sense;

        /// <summary>現在の魅力値</summary>
        private int charm;

        /// <summary>現在の威厳値</summary>
        private int dignity;

        /// <summary>割り振りポイント(魔力値)</summary>
        private int MP;

        /// <summary>割り振られたポイントの合計値</summary>
        private int totalAddPoint;

        /// <summary>割り振られた魅力値</summary>
        private int addCharm;

        /// <summary>割り振られた耐久値</summary>
        private int addDurability;

        /// <summary>割り振られた筋力値</summary>
        private int addMuscularStrength;

        /// <summary>割り振られた知識値</summary>
        private int addKnowledge;

        /// <summary>割り振られた感覚値</summary>
        private int addSense;

        /// <summary>割り振られた威厳値</summary>
        private int addDignity;

        //各ステータスのパラメーターテキスト
        public TextMeshProUGUI levelText, hitPointText, attackText, defenseText,
            durabilityText, muscularStrengthText, knowledgeText, senseText,
            charmText, dignityText, MPText, updatedHitPointText, updatedAttackText, updatedDefenseText,
            addCharmText, addDignityText, addDurabilityText,
            addMuscularStrengthText, addSenseText, addKnowledgeText;

        /// <summary>固有ステータスを基礎ステータスに変換する際の倍率</summary>
        private int magnification;

        //確定ボタンと中止ボタン
        public GameObject ConfirmAndResetButtons;


        private void Awake()
        {
            magia = Magia.Instance;
            m_touchGestureDetector = TouchGestureDetector.Instance;

        }

        private void Start()
        {
            LoadCurrentStatus();

            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    GameObject go;
                    touchInfo.HitDetection(out go);

                    if (go != null)
                    {
                        if (go.name == "AddCharmButton")
                        {
                            AddStatusPoint(addCharm, addCharmText);
                        }
                        if (go.name == "ReductionCharmButton")
                        {
                            ReductionStatusPoint(addCharm, addCharmText);
                        }

                        if (go.name == "AddDignityButton")
                        {
                            AddStatusPoint(addDignity, addDignityText);
                        }
                        if (go.name == "ReductionDignityButton")
                        {
                            ReductionStatusPoint(addDignity, addDignityText);
                        }

                        if (go.name == "AddDurabilityButton")
                        {
                            AddStatusPoint(addDurability, addDurabilityText);
                        }
                        if (go.name == "ReductionDurabilityButton")
                        {
                            ReductionStatusPoint(addDurability, addDurabilityText);
                        }

                        if (go.name == "AddMuscularStrengthButton")
                        {
                            AddStatusPoint(addMuscularStrength, addMuscularStrengthText);
                        }
                        if (go.name == "ReductionMuscularStrengthButton")
                        {
                            ReductionStatusPoint(addMuscularStrength, addMuscularStrengthText);
                        }

                        if (go.name == "AddKnowledgeButton")
                        {
                            AddStatusPoint(addKnowledge, addKnowledgeText);
                        }
                        if (go.name == "ReductionKnowledgeButton")
                        {
                            ReductionStatusPoint(addKnowledge, addKnowledgeText);
                        }

                        if (go.name == "AddSenseButton")
                        {
                            AddStatusPoint(addSense, addSenseText);
                        }
                        if (go.name == "ReductionSenseButton")
                        {
                            ReductionStatusPoint(addSense, addSenseText);
                        }

                        if (go.name == "ConfirmButton")
                        {
                            magia.Update();
                        }
                        if (go.name == "ResetButton")
                        {
                            LoadCurrentStatus();
                        }
                    }
                    else
                    {
                        Debug.Log("どこ押してんだよ！");
                    }
                }
            });
        }

        private void Update()
        {
            //固有ステータスを基礎ステータスに変換
            updatedHitPoint = hitPoint + (addCharm * magnification) + (addDignity * magnification); // 魅力と威厳を体力に変換
            updatedAttack = attack + (addSense * magnification) + (addMuscularStrength * magnification);//筋力と感覚を攻撃力に変換
            updatedDefense = defense + (addDurability * magnification) + (addKnowledge * magnification);//耐久と知識を防御力に変換

            //計算結果をテキストに反映
            updatedHitPointText.text = updatedHitPoint.ToString();
            updatedAttackText.text = updatedAttack.ToString();
            updatedDefenseText.text = updatedDefense.ToString();
        }

        /// <summary>
        /// ステータスの変動値を初期化する
        /// </summary>
        public void LoadCurrentStatus()
        {
            //確定ボタンと中止ボタンを無効にする
            ConfirmAndResetButtons.SetActive(false);

            hitPoint = magia.GetStats().m_hitPoint;
            attack = magia.GetStats().m_attack;
            defense = magia.GetStats().m_defense;

            updatedHitPoint = hitPoint;
            updatedAttack = attack;
            updatedDefense = defense;

            durability = magia.GetStats().m_durability;
            muscularStrength = magia.GetStats().m_muscularStrength;
            knowledge = magia.GetStats().m_knowledge;
            sense = magia.GetStats().m_sense;
            charm = magia.GetStats().m_charm;
            dignity = magia.GetStats().m_dignity;
            //MP = magia.AllocationPoint;
            MP = 10;

            hitPointText.GetComponent<TextMeshProUGUI>().text = hitPoint.ToString();
            attackText.GetComponent<TextMeshProUGUI>().text = attack.ToString();
            defenseText.GetComponent<TextMeshProUGUI>().text = defense.ToString();

            updatedHitPointText.GetComponent<TextMeshProUGUI>().text = hitPoint.ToString();
            updatedAttackText.GetComponent<TextMeshProUGUI>().text = attack.ToString();
            updatedDefenseText.GetComponent<TextMeshProUGUI>().text = defense.ToString();

            durabilityText.GetComponent<TextMeshProUGUI>().text = durability.ToString();
            muscularStrengthText.GetComponent<TextMeshProUGUI>().text = muscularStrength.ToString();
            knowledgeText.GetComponent<TextMeshProUGUI>().text = knowledge.ToString();
            senseText.GetComponent<TextMeshProUGUI>().text = sense.ToString();
            charmText.GetComponent<TextMeshProUGUI>().text = charm.ToString();
            dignityText.GetComponent<TextMeshProUGUI>().text = dignity.ToString();

            MPText.GetComponent<TextMeshProUGUI>().text = MP.ToString();
        }

        /// <summary> ＋ボタンを押すと割り振りポイントが1減り、指定の固有ステータスポイントが割り振られる</summary>
        /// <param name="statusPoint"></param>
        public int AddStatusPoint(int addStatusPoint, TextMeshProUGUI addStatusPointText)
        {
            if (MP > 0)
            {
                MP -= 1;
                MPText.text = MP.ToString();
                addStatusPoint += 3;
                addStatusPointText.text = addStatusPoint.ToString();

                totalAddPoint += 3;
                ConfirmAndResetButtons.SetActive(true);
            }
            return addStatusPoint;
        }

        public int GetAddCharm()
        {
            return addCharm;
        }

        /// <summary> －ボタンを押すと指定の固有ステータスポイントが減り、割り振りポイントが1増える</summary>
        /// <param name="addStatusPoint"></param>
        public void ReductionStatusPoint(int addStatusPoint, TextMeshProUGUI addStatusPointText)
        {
            if (addStatusPoint > 0)
            {
                addStatusPoint -= 3;
                addStatusPointText.text = addStatusPoint.ToString();
                MP += 1;
                MPText.text = MP.ToString();

                totalAddPoint -= 3;

                if (totalAddPoint <= 0)
                {
                    totalAddPoint = 0;
                    ConfirmAndResetButtons.SetActive(false);
                }

                if (addStatusPoint <= 0)
                {
                    addStatusPoint = 0;
                }
            }
        }
    }
}