using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DemonicCity;

namespace DemonicCity.StrengthenScene
{
    public class AllocationStatusPoint : MonoBehaviour
    {
        [SerializeField]
        TMP_Text[] statusText = new TMP_Text[12];

        public TextMeshProUGUI hitPointText, attackText, defenseText,
           durabilityText, muscularStrengthText, knowledgeText, senseText,
           charmText, dignityText, MPText, updatedHitPointText, updatedAttackText, updatedDefenseText,
           addCharmText, addDignityText, addDurabilityText,
           addMuscularStrengthText, addSenseText, addKnowledgeText;

        Magia magia;
        TouchGestureDetector m_touchGestureDetector;

        /// <summary>現在の属性</summary>
        private Magia.Attribute attribute;

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
       
        public TextMeshProUGUI attributeText;


        /// <summary>確定ボタンと中止ボタン</summary>
        public GameObject ConfirmAndResetButtons;

        public enum ButtonName
        {
            AddCharm,
            ReductionCharm,

            AddDignity,
            ReductionDignity,


        }

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
                        switch (go.name)
                        {
                            case ("AddCharmButton"):
                                ChangeUniqueStatus(ref addCharm, ref addCharmText, true);
                                break;
                            case ("ReductionCharmButton"):
                                ChangeUniqueStatus(ref addCharm, ref addCharmText, false);
                                break;
                            case ("AddDignityButton"):
                                ChangeUniqueStatus(ref addDignity, ref addDignityText, true);
                                break;
                            case ("ReductionDignityButton"):
                                ChangeUniqueStatus(ref addDignity, ref addDignityText, false);
                                break;
                            case ("AddDurabilityButton"):
                                ChangeUniqueStatus(ref addDurability, ref addDurabilityText, true);
                                break;
                            case ("ReductionDurabilityButton"):
                                ChangeUniqueStatus(ref addDurability, ref addDurabilityText, false);
                                break;
                            case ("AddMuscularStrengthButton"):
                                ChangeUniqueStatus(ref addMuscularStrength, ref addMuscularStrengthText, true);
                                break;
                            case ("ReductionMuscularStrengthButton"):
                                ChangeUniqueStatus(ref addMuscularStrength, ref addMuscularStrengthText, false);
                                break;
                            case ("AddKnowledgeButton"):
                                ChangeUniqueStatus(ref addKnowledge, ref addKnowledgeText, true);
                                break;
                            case ("ReductionKnowledgeButton"):
                                ChangeUniqueStatus(ref addKnowledge, ref addKnowledgeText, false);
                                break;
                            case ("AddSenseButton"):
                                ChangeUniqueStatus(ref addSense, ref addSenseText, true);
                                break;
                            case ("ReductionSenseButton"):
                                ChangeUniqueStatus(ref addSense, ref addSenseText, false);
                                break;
                            case ("ConfirmButton"):
                                magia.Update();
                                break;
                            case ("ResetButton"):
                                LoadCurrentStatus();
                                break;
                        }
                    }
                }
            });
        }

        private void Update()
        {
            if (totalAddPoint > 0)
            {
                ConfirmAndResetButtons.SetActive(true);
            }
            else
            {
                totalAddPoint = 0;
                ConfirmAndResetButtons.SetActive(false);
            }
        }

        /// <summary>魔力値を固有ステータスに割り振り、基礎ステータスに変換する</summary>
        /// <param name="uniqueStatus"></param>
        /// <param name="uniqueStatusText"></param>
        /// <param name="addStatus"></param>
        public void ChangeUniqueStatus(ref int uniqueStatus, ref TextMeshProUGUI uniqueStatusText, bool addStatus)
        {
            if (addStatus)
            {
                uniqueStatus += AddStatusPoint(uniqueStatus);
            }
            else
            {
                uniqueStatus -= ReductionStatusPoint(uniqueStatus);
            }
            uniqueStatusText.text = uniqueStatus.ToString();

            //固有ステータスを基礎ステータスに変換
            updatedHitPoint = hitPoint + (addCharm * 50) + (addDignity * 50);
            updatedAttack = attack + (addSense * 5) + (addMuscularStrength * 5);
            updatedDefense = defense + (addDurability * 5) + (addKnowledge * 5);

            updatedHitPointText.text = updatedHitPoint.ToString();
            updatedAttackText.text = updatedAttack.ToString();
            updatedDefenseText.text = updatedDefense.ToString();
        }
        
        /// <summary>ステータスの変動値を初期化する</summary>
        public void LoadCurrentStatus()
        {
            ConfirmAndResetButtons.SetActive(false);

            attribute = magia.MyAttribute;

            switch (attribute)
            {
                case Magia.Attribute.Standard:
                case Magia.Attribute.MaleWarrior:
                case Magia.Attribute.FemaleWarrior:
                case Magia.Attribute.MaleWizard:
                case Magia.Attribute.FemaleWitch:
                case Magia.Attribute.FemaleTrancendental:
                    attributeText.GetComponent<TextMeshProUGUI>().text = attribute.ToString();
                    break;
            }

            hitPoint = magia.GetStats().m_hitPoint;
            attack = magia.GetStats().m_attack;
            defense = magia.GetStats().m_defense;
            durability = magia.GetStats().m_durability;
            muscularStrength = magia.GetStats().m_muscularStrength;
            knowledge = magia.GetStats().m_knowledge;
            sense = magia.GetStats().m_sense;
            charm = magia.GetStats().m_charm;
            dignity = magia.GetStats().m_dignity;
            //MP = magia.AllocationPoint;
            MP = 10;

            hitPointText.GetComponent<TMP_Text>().text = hitPoint.ToString();
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

        /// <summary>割り振りポイントが1減り、指定の固有ステータスポイントが1割り振られる</summary>
        /// <param name="uniqueStatus"></param>
        public int AddStatusPoint(int uniqueStatus)
        {
            if (MP > 0)
            {
                MP -= 1;
                MPText.text = MP.ToString();
                uniqueStatus = 1;

                totalAddPoint += 1;
            }
            else
            {
                uniqueStatus = 0;
            }
            return uniqueStatus;
        }

        /// <summary>指定の固有ステータスポイントが1減り、割り振りポイントが1増える</summary>
        /// <param name="uniqueStatus"></param>
        public int ReductionStatusPoint(int uniqueStatus)
        {
            if (uniqueStatus > 0)
            {
                uniqueStatus = 1;
                MP += 1;
                MPText.text = MP.ToString();

                totalAddPoint -= 1;
            }
            else
            {
                uniqueStatus = 0;
            }
            return uniqueStatus;
        }

        public void ConfirmStatus()
        {
            hitPoint = updatedHitPoint;
            attack = updatedAttack;
            defense = updatedDefense;
        }
    }
}