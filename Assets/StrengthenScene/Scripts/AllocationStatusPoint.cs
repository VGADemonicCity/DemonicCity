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

        //現在のステータス
        private int level, hitPoint, attack, defense, durability, muscularStrength,
            knowledge, sense, charm, dignity;
        //変動後の基礎ステータス
        private int updatedHitPoint, updatedAttack, updatedDefense;

        //割り振りポイント(残り魔力値)
        private int MP;

        //割り振られたポイントの合計値
        private int totalAddPoint;

        //割り振られた分の固有ステータス
        private int addCharm, addDurability, addMuscularStrength,
            addKnowledge, addSense, addDignity;

        //各ステータスのパラメーターテキスト
        public TextMeshProUGUI levelText, hitPointText, attackText, defenseText,
            durabilityText, muscularStrengthText, knowledgeText, senseText,
            charmText, dignityText, MPText, updatedHitPointText, updatedAttackText, updatedDefenseText,
            addCharmText, addDignityText, addDurabilityText,
            addMuscularStrengthText, addSenseText, addKnowledgeText;

        ////各固有ステータスの＋ボタンと－ボタン
        //private GameObject AddCharmButton, ReductionCharmButton, AddDignityButton, ReductionDignityButton, AddMuscularStrengthButton, ReductionMuscularStrengthButton,
        //    AddDurabilityButton, ReductionDurabilityButton, AddKnowledgeButton, ReductionKnowlegdeButton, AddSenseButton, ReductionSenseButton;

        //固有ステータスを基礎ステータスに変換する際の倍率
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
            // Attribute attribute = GetComponent<Attribute>();

            //確定ボタンと中止ボタンを無効にする
            ConfirmAndResetButtons.SetActive(false);

            //マギアの現在のステータスを参照し表示
            hitPoint = magia.GetStats().m_hitPoint;//体力
            attack = magia.GetStats().m_attack;//攻撃力
            defense = magia.GetStats().m_defense;//防御力

            updatedHitPoint = hitPoint;
            updatedAttack = attack;
            updatedDefense = defense;

            durability = magia.GetStats().m_durability;//耐久
            muscularStrength = magia.GetStats().m_muscularStrength;//筋力
            knowledge = magia.GetStats().m_knowledge;//知識
            sense = magia.GetStats().m_sense;//感覚
            charm = magia.GetStats().m_charm;//魅力
            dignity = magia.GetStats().m_dignity;//威厳
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

        /// <summary>
        /// ＋ボタンを押すと割り振りポイントが1減り、指定の固有ステータスポイントが割り振られる
        /// </summary>
        /// <param name="statusPoint"></param>
        public void AddStatusPoint(int addStatusPoint, TextMeshProUGUI addStatusPointText)
        {
            if (MP > 0)
            {
                MP -= 1;
                MPText.text = MP.ToString();
                addStatusPoint += 3;
                addStatusPointText.text = addStatusPoint.ToString();

                totalAddPoint += 3;
                Debug.Log(totalAddPoint);
                //魔力値が割り振られた際に確定ボタンと中止ボタンを有効にする
                ConfirmAndResetButtons.SetActive(true);
            }
        }

        /// <summary>
        /// －ボタンを押すと指定の固有ステータスポイントが減り、割り振りポイントが1増える
        /// </summary>
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
                Debug.Log(totalAddPoint);
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
