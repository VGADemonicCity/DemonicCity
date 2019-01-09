﻿using UnityEngine;
using TMPro;
using System;
using DemonicCity.BattleScene;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DemonicCity.StrengthenScene
{
    public class AllocationStatusPoint : MonoBehaviour
    {
        /// <summary>Magiaクラスのインスタンス</summary>
        Magia magia;

        /// <summary>TouchGestureDetectorクラスのインスタンス</summary>
        TouchGestureDetector touchGestureDetector;

        /// <summary>現在の属性</summary>
        private Magia.Attribute attribute;

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
        private int statusPoint;

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

        /// <summary>確定ボタンと中止ボタン</summary>
        [SerializeField]
        private GameObject ConfirmAndResetButtons;

        /// <summary>属性テキスト</summary>
        [SerializeField]
        private Text currentAttributeText;

        /// <summary>現在の基礎ステータステキスト</summary>
        [SerializeField]
        TextMeshProUGUI[] currentBasicStatusTexts = new TextMeshProUGUI[3];

        /// <summary>変動後の基礎ステータステキスト</summary>
        [SerializeField]
        TextMeshProUGUI[] updatedBasicStatusTexts = new TextMeshProUGUI[3];

        /// <summary>現在の固有ステータステキスト</summary>
        [SerializeField]
        TextMeshProUGUI[] currentUniqueStatusTexts = new TextMeshProUGUI[6];

        /// <summary>割り振った固有ステータステキスト</summary>
        [SerializeField]
        TextMeshProUGUI[] addUniqueStatusTexts = new TextMeshProUGUI[6];

        /// <summary>割り振りポイントテキスト(魔力値)</summary>
        [SerializeField]
        TextMeshProUGUI statusPointText;

        /// <summary>現在の属性</summary>
        [SerializeField]
        private Image attributeImage;

        /// <summary>6種類の属性スプライト</summary>
        [SerializeField]
        private Sprite[] attributeSprite;

        /// <summary>ステータス確定前のメッセージ</summary>
        [SerializeField]
        private GameObject confirm_WarningMessage;

        /// <summary>ステータス初期化前のメッセージ</summary>
        [SerializeField]
        private GameObject reset_WarningMessage;

        /// <summary>ポップアップウィンドウの生成先</summary>
        [SerializeField]
        private Transform parent;

        /// <summary>ポップアップウィンドウを一時的に保持する変数</summary>
        private GameObject popUpWindow = null;

        /// <summary>確定ボタンとリセットボタンを一時的に保持する変数</summary>
        private GameObject confirmResetButtons = null;

        [SerializeField]
        private GameObject selectAttributeWindow;

        [SerializeField]
        private GameObject showDetailWindow;

        /// <summary>習得済みスキル</summary>
        private Magia.PassiveSkill passiveSkill;

        /// <summary>習得済みスキルの表示先</summary>
        private GameObject content;

        [SerializeField]
        private GameObject skillDetailText;

        /// <summary>スキルの説明テキスト</summary>
        private List<string> skillDetailList = new List<string>();

        private GameObject skillDetail;

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            passiveSkill = magia.MyPassiveSkill;

            //      scrollRect.verticalNormalizedPosition = 1f;
            skillDetailList.Add("街破壊数1以上で発動\n街破壊数×攻撃力の1%\nを加算して攻撃");
            skillDetailList.Add("街破壊数７以上で発動\n街破壊数×0.5%\n攻撃力防御力を上昇");

        }

        private void Start()
        {
            ResetStatus();

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    GameObject button;
                    touchInfo.HitDetection(out button);

                    if (button != null)
                    {
                        switch (button.name)
                        {
                            case "SelectAttributeButton":
                                if (popUpWindow == null)
                                {
                                    popUpWindow = Instantiate(selectAttributeWindow, parent);
                                }
                                break;
                            case "ShowSkillButton":
                                if (popUpWindow == null)
                                {
                                    popUpWindow = Instantiate(showDetailWindow, parent);
                                    //        content = GameObject.Find("Content");
                                    content = GameObject.FindGameObjectWithTag("Content");
                                    //     scrollRect = popUpWindow.GetComponentInChildren<ScrollRect>();
                                }
                                break;
                            case "BackButton":
                                if (popUpWindow != null)
                                {
                                    Destroy(popUpWindow);
                                }
                                break;
                            case "DevilsFist":
                                PopUpSkillDetailWindow(0);
                                break;
                            case ("HighConcentrationMagicalAbsorption"):
                                PopUpSkillDetailWindow(1);
                                break;

                            case "AddCharmButton":
                                ChangeUniqueStatus(ref addCharm, ref addUniqueStatusTexts[0], true);
                                break;
                            case "ReductionCharmButton":
                                ChangeUniqueStatus(ref addCharm, ref addUniqueStatusTexts[0], false);
                                break;
                            case "AddDignityButton":
                                ChangeUniqueStatus(ref addDignity, ref addUniqueStatusTexts[1], true);
                                break;
                            case "ReductionDignityButton":
                                ChangeUniqueStatus(ref addDignity, ref addUniqueStatusTexts[1], false);
                                break;
                            case "AddMuscularStrengthButton":
                                ChangeUniqueStatus(ref addMuscularStrength, ref addUniqueStatusTexts[2], true);
                                break;
                            case "ReductionMuscularStrengthButton":
                                ChangeUniqueStatus(ref addMuscularStrength, ref addUniqueStatusTexts[2], false);
                                break;
                            case "AddSenseButton":
                                ChangeUniqueStatus(ref addSense, ref addUniqueStatusTexts[3], true);
                                break;
                            case "ReductionSenseButton":
                                ChangeUniqueStatus(ref addSense, ref addUniqueStatusTexts[3], false);
                                break;
                            case "AddDurabilityButton":
                                ChangeUniqueStatus(ref addDurability, ref addUniqueStatusTexts[4], true);
                                break;
                            case "ReductionDurabilityButton":
                                ChangeUniqueStatus(ref addDurability, ref addUniqueStatusTexts[4], false);
                                break;
                            case "AddKnowledgeButton":
                                ChangeUniqueStatus(ref addKnowledge, ref addUniqueStatusTexts[5], true);
                                break;
                            case "ReductionKnowledgeButton":
                                ChangeUniqueStatus(ref addKnowledge, ref addUniqueStatusTexts[5], false);
                                break;
                            case "Mado":
                                attribute = Magia.Attribute.Standard;
                                UpdateText();
                                if (confirmResetButtons == null)
                                {
                                    confirmResetButtons = Instantiate(ConfirmAndResetButtons, parent);
                                }
                                break;
                            case "Kenki":
                                attribute = Magia.Attribute.FemaleWarrior;
                                UpdateText();
                                if (confirmResetButtons == null)
                                {
                                    confirmResetButtons = Instantiate(ConfirmAndResetButtons, parent);
                                }
                                break;
                            case "Jinou":
                                attribute = Magia.Attribute.MaleWarrior;
                                UpdateText();
                                if (confirmResetButtons == null)
                                {
                                    confirmResetButtons = Instantiate(ConfirmAndResetButtons, parent);
                                }
                                break;
                            case "Jotei":
                                attribute = Magia.Attribute.FemaleWitch;
                                UpdateText();
                                if (confirmResetButtons == null)
                                {
                                    confirmResetButtons = Instantiate(ConfirmAndResetButtons, parent);
                                }
                                break;
                            case "Kokuo":
                                attribute = Magia.Attribute.MaleWizard;
                                UpdateText();
                                if (confirmResetButtons == null)
                                {
                                    confirmResetButtons = Instantiate(ConfirmAndResetButtons, parent);
                                }
                                break;
                            case "Majin":
                                attribute = Magia.Attribute.FemaleTrancendental;
                                UpdateText();
                                if (confirmResetButtons == null)
                                {
                                    confirmResetButtons = Instantiate(ConfirmAndResetButtons, parent);
                                }
                                break;
                            case "ConfirmButton":
                                if (popUpWindow == null)
                                {
                                    popUpWindow = Instantiate(confirm_WarningMessage, parent);
                                }
                                break;
                            case "YesConfirm":
                                ConfirmStatus();
                                if (popUpWindow != null)
                                {
                                    Destroy(popUpWindow);
                                }
                                break;
                            case "NoConfirm":
                                if (popUpWindow != null)
                                {
                                    Destroy(popUpWindow);
                                }
                                break;
                            case "ResetButton":
                                if (popUpWindow == null)
                                {
                                    popUpWindow = Instantiate(reset_WarningMessage, parent);
                                }
                                break;
                            case "YesReset":
                                ResetStatus();
                                if (popUpWindow != null)
                                {
                                    Destroy(popUpWindow);
                                }
                                break;
                            case "NoReset":
                                if (popUpWindow != null)
                                {
                                    Destroy(popUpWindow);
                                }
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
            //            content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            skillDetail = Instantiate(skillDetailText, content.transform);
            skillDetail.GetComponentInChildren<Text>().text = skillDetailList[index];
        }

        /// <summary>魔力値を固有ステータスに割り振り、基礎ステータスに変換する</summary>
        /// <param name="uniqueStatus">固有ステータス</param>
        /// <param name="uniqueStatusText">固有ステータスのテキスト</param>
        /// <param name="addStatus">ステータスの増減判定</param>
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

            updatedBasicStatusTexts[0].text = updatedHitPoint.ToString();
            updatedBasicStatusTexts[1].text = updatedAttack.ToString();
            updatedBasicStatusTexts[2].text = updatedDefense.ToString();
        }

        /// <summary>ステータスの変動値を初期化</summary>
        public void ResetStatus()
        {
            attribute = magia.MyAttribute;
            hitPoint = magia.GetStats().m_hitPoint;
            attack = magia.GetStats().m_attack;
            defense = magia.GetStats().m_defense;
            charm = magia.GetStats().m_charm;
            dignity = magia.GetStats().m_dignity;
            muscularStrength = magia.GetStats().m_muscularStrength;
            sense = magia.GetStats().m_sense;
            durability = magia.GetStats().m_durability;
            knowledge = magia.GetStats().m_knowledge;
            addCharm = 0;
            addDignity = 0;
            addMuscularStrength = 0;
            addSense = 0;
            addDurability = 0;
            addKnowledge = 0;
            //StatusPoint = magia.AllocationPoint;
            statusPoint = 10;
            UpdateText();
        }

        /// <summary>変更したステータスを確定する</summary>
        public void ConfirmStatus()
        {

            hitPoint = updatedHitPoint;
            attack = updatedAttack;
            defense = updatedDefense;

            charm += addCharm;
            dignity += addDignity;
            muscularStrength += addMuscularStrength;
            sense += addSense;
            durability += addDurability;
            knowledge += addKnowledge;

            addCharm = 0;
            addDignity = 0;
            addMuscularStrength = 0;
            addSense = 0;
            addDurability = 0;
            addKnowledge = 0;

            UpdateText();
            magia.Update();
        }

        /// <summary>テキストを更新</summary>
        public void UpdateText()
        {
            switch (attribute)
            {
                case Magia.Attribute.Standard:
                    attributeImage.GetComponent<Image>().sprite = attributeSprite[0];
                    currentAttributeText.GetComponent<Text>().text = "魔童";
                    break;
                case Magia.Attribute.MaleWarrior:
                    attributeImage.GetComponent<Image>().sprite = attributeSprite[1];
                    currentAttributeText.GetComponent<Text>().text = "刀皇";
                    break;
                case Magia.Attribute.FemaleWarrior:
                    attributeImage.GetComponent<Image>().sprite = attributeSprite[2];
                    currentAttributeText.GetComponent<Text>().text = "剣姫";
                    break;
                case Magia.Attribute.MaleWizard:
                    attributeImage.GetComponent<Image>().sprite = attributeSprite[3];
                    currentAttributeText.GetComponent<Text>().text = "黒王";
                    break;
                case Magia.Attribute.FemaleWitch:
                    attributeImage.GetComponent<Image>().sprite = attributeSprite[4];
                    currentAttributeText.GetComponent<Text>().text = "女帝";
                    break;
                case Magia.Attribute.FemaleTrancendental:
                    attributeImage.GetComponent<Image>().sprite = attributeSprite[5];
                    currentAttributeText.GetComponent<Text>().text = "魔神";
                    break;
            }

            currentBasicStatusTexts[0].text = hitPoint.ToString();
            currentBasicStatusTexts[1].text = attack.ToString();
            currentBasicStatusTexts[2].text = defense.ToString();

            updatedBasicStatusTexts[0].text = "";
            updatedBasicStatusTexts[1].text = "";
            updatedBasicStatusTexts[2].text = "";

            currentUniqueStatusTexts[0].text = charm.ToString();
            currentUniqueStatusTexts[1].text = dignity.ToString();
            currentUniqueStatusTexts[2].text = muscularStrength.ToString();
            currentUniqueStatusTexts[3].text = sense.ToString();
            currentUniqueStatusTexts[4].text = durability.ToString();
            currentUniqueStatusTexts[5].text = knowledge.ToString();

            addUniqueStatusTexts[0].text = "+ " + addCharm.ToString();
            addUniqueStatusTexts[1].text = "+ " + addDignity.ToString();
            addUniqueStatusTexts[2].text = "+ " + addMuscularStrength.ToString();
            addUniqueStatusTexts[3].text = "+ " + addSense.ToString();
            addUniqueStatusTexts[4].text = "+ " + addDurability.ToString();
            addUniqueStatusTexts[5].text = "+ " + addKnowledge.ToString();

            statusPointText.text = statusPoint.ToString();
        }

        /// <summary>割り振りポイント-1、固有ステータスポイント+1</summary>
        /// <param name="uniqueStatus">固有ステータス</param>
        public int AddStatusPoint(int uniqueStatus)
        {
            if (statusPoint > 0)
            {
                statusPoint -= 1;
                statusPointText.text = statusPoint.ToString();
                uniqueStatus = 1;

                totalAddPoint += 1;
                if (totalAddPoint > 0 && confirmResetButtons == null)
                {
                    confirmResetButtons = Instantiate(ConfirmAndResetButtons, parent);
                }
            }
            else
            {
                uniqueStatus = 0;
                statusPoint = 0;
            }
            return uniqueStatus;
        }

        /// <summary>固有ステータスポイント-1、割り振りポイント+1</summary>
        /// <param name="uniqueStatus">固有ステータス</param>
        public int ReductionStatusPoint(int uniqueStatus)
        {
            if (uniqueStatus > 0)
            {
                uniqueStatus = 1;
                statusPoint += 1;
                statusPointText.text = statusPoint.ToString();

                totalAddPoint -= 1;
                if (totalAddPoint <= 0 && confirmResetButtons != null)
                {
                    totalAddPoint = 0;
                    Destroy(confirmResetButtons);
                }
            }
            else
            {
                uniqueStatus = 0;
            }
            return uniqueStatus;
        }
    }
}