using UnityEngine;
using TMPro;
using System;
using DemonicCity.BattleScene;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

        /// <summary>属性テキスト</summary>
        [SerializeField] private Text currentAttributeText;

        /// <summary>現在の基礎ステータステキスト</summary>
        [SerializeField] TextMeshProUGUI[] currentBasicStatusTexts = new TextMeshProUGUI[3];

        /// <summary>変動後の基礎ステータステキスト</summary>
        [SerializeField] TextMeshProUGUI[] updatedBasicStatusTexts = new TextMeshProUGUI[3];

        /// <summary>現在の固有ステータステキスト</summary>
        [SerializeField] TextMeshProUGUI[] currentUniqueStatusTexts = new TextMeshProUGUI[6];

        /// <summary>割り振った固有ステータステキスト</summary>
        [SerializeField] TextMeshProUGUI[] addUniqueStatusTexts = new TextMeshProUGUI[6];

        /// <summary>割り振りポイントテキスト(魔力値)</summary>
        [SerializeField] TextMeshProUGUI statusPointText;

        /// <summary>現在の属性</summary>
        [SerializeField] private Image attributeImage;

        /// <summary>6種類の属性スプライト</summary>
        [SerializeField] private Sprite[] attributeSprite;

        /// <summary>ステータス確定前のメッセージウィンドウ</summary>
        [SerializeField] private GameObject confirmWarningMessageWindow;

        /// <summary>ステータス初期化前のメッセージウィンドウ</summary>
        [SerializeField] private GameObject resetWarningMessageWindow;

        /// <summary>確定ボタンとリセットボタン</summary>
        [SerializeField] private GameObject confirmResetButtons;

        /// <summary>属性選択ウィンドウ</summary>
        [SerializeField] private GameObject selectAttributeWindow;

        /// <summary>習得済みスキル一覧ウィンドウ</summary>
        [SerializeField] private GameObject skillListWindow;

        /// <summary>習得済みスキル</summary>
        private Magia.PassiveSkill passiveSkill;

        /// <summary>各スキルの説明ウィンドウ</summary>
        [SerializeField] private GameObject[] skillDescriptionText;

        /// <summary>各スキルの説明</summary>
        private List<string> skillDescriptionList = new List<string>();

        /// <summary>ポップアップウィンドウの表示/非表示</summary>
        private bool setActivePopUpWindow = false;

        /// <summary>スキル説明テキストの表示/非表示</summary>
        private bool setActiveSkillDescription = false;

        /// <summary>確定/中止ボタンの表示/非表示</summary>
        private bool setActiveConfirmResetButtons = false;

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }

        public enum BUTTONNAME
        {
            BACK,
        }

        private void Start()
        {
            skillDescriptionList.Add("街破壊数1以上で発動\n街破壊数×攻撃力の1%\nを加算して攻撃");
            skillDescriptionList.Add("街破壊数7以上で発動\n街破壊数×0.5%\n攻撃力防御力を上昇");
            skillDescriptionList.Add("街破壊数14以上で発動\n街破壊数×最大 HPの1%回復");
            skillDescriptionList.Add("自分の攻撃力2分の1を敵に防御力を\n無視してダメージを与える");
            skillDescriptionList.Add("街破壊数19以上で発動\n次の敵の攻撃を5%軽減");
            skillDescriptionList.Add("街破壊数21以上で発動\n街破壊数×攻撃力の0.5%を加算して攻撃");
            skillDescriptionList.Add("街破壊数24以上で発動　街破壊数×0.5%攻撃力上昇");
            skillDescriptionList.Add("街破壊数27以上で発動\n次の敵の攻撃を10%軽減");
            skillDescriptionList.Add("破壊数30以上で発動\n破壊数×攻撃力の0.5%を加算して攻撃");
            skillDescriptionList.Add("街破壊数32枚以上で発動\n街破壊数×最大HPの2%回復");
            skillDescriptionList.Add("街破壊数34以上で発動\n街破壊数×0.5%攻撃力上昇");
            skillDescriptionList.Add("街破壊数38以上で発動\n自分の攻撃力1倍を敵に防御力を無視してダメージを与える");
            skillDescriptionList.Add("街破壊数36以上で発動\n次の敵の攻撃を無効化");
            skillDescriptionList.Add("スキル発動枚数-10");

            for (int i = 0; i < skillDescriptionText.Length; i++)
            {
                skillDescriptionText[i].SetActive(false);
                skillDescriptionText[i].GetComponentInChildren<Text>().text = skillDescriptionList[i];
            }

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
                            case "BackToHome":
                                SceneChanger.SceneChange(SceneName.Home);
                                break;

                            case "SelectAttributeButton":
                                if (setActivePopUpWindow == false)
                                {
                                    selectAttributeWindow.SetActive(true);
                                    setActivePopUpWindow = true;
                                }
                                break;

                            case "BackAttribute":
                                if (setActivePopUpWindow)
                                {
                                    selectAttributeWindow.SetActive(false);
                                    setActivePopUpWindow = false;
                                }
                                break;

                            case "ShowSkillButton":
                                if (setActivePopUpWindow == false)
                                {
                                    skillListWindow.SetActive(true);
                                    setActivePopUpWindow = true;
                                }
                                break;

                            case "BackSkill":
                                if (setActivePopUpWindow)
                                {
                                    skillListWindow.SetActive(false);
                                    setActivePopUpWindow = false;
                                }
                                break;

                            //ここから各スキル名の処理
                            case "DevilsFist":
                                SkillDescriptionManager(0);
                                break;
                            case "HighConcentrationMagicalAbsorption":
                                SkillDescriptionManager(1);
                                break;
                            case "SelfRegeneration":
                                SkillDescriptionManager(2);
                                break;
                            case "ExplosiveFlamePillar":
                                SkillDescriptionManager(3);
                                break;
                            case "CrimsonBarrier":
                                SkillDescriptionManager(4);
                                break;
                            case "DevilsFistInfernoType":
                                SkillDescriptionManager(5);
                                break;
                            case "BraveHeartsIncarnation":
                                SkillDescriptionManager(6);
                                break;
                            case "GreatCrimsonBarrier":
                                SkillDescriptionManager(7);
                                break;
                            case "InfernosFist":
                                SkillDescriptionManager(8);
                                break;
                            case "SatansCell":
                                SkillDescriptionManager(9);
                                break;
                            case "AmaterasuIncanation":
                                SkillDescriptionManager(10);
                                break;
                            case "AmaterasuInferno":
                                SkillDescriptionManager(11);
                                break;
                            case "AmaterasuFlameWall":
                                SkillDescriptionManager(12);
                                break;
                            //ここまで各スキル名の処理

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

                                //ここから各属性名の処理
                            case "Mado":
                                attribute = Magia.Attribute.Standard;
                                UpdateText();
                                if (confirmResetButtons == false)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                break;

                            case "Kenki":
                                attribute = Magia.Attribute.FemaleWarrior;
                                UpdateText();
                                if (confirmResetButtons == false)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                break;

                            case "Jinou":
                                attribute = Magia.Attribute.MaleWarrior;
                                UpdateText();
                                if (confirmResetButtons == false)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                break;

                            case "Jotei":
                                attribute = Magia.Attribute.FemaleWitch;
                                UpdateText();
                                if (confirmResetButtons == false)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                break;

                            case "Kokuo":
                                attribute = Magia.Attribute.MaleWizard;
                                UpdateText();
                                if (confirmResetButtons == false)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                break;

                            case "Majin":
                                attribute = Magia.Attribute.FemaleTrancendental;
                                UpdateText();
                                if (confirmResetButtons == false)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                break;
                                //ここまで各属性名の処理

                            case "ConfirmButton":
                                if (setActivePopUpWindow == false)
                                {
                                    confirmWarningMessageWindow.SetActive(true);
                                    setActivePopUpWindow = true;
                                    confirmWarningMessageWindow.GetComponentInChildren<Text>().text = "変更したステータスを確定します";
                                }
                                break;

                            case "ResetButton":
                                if (setActivePopUpWindow == false)
                                {
                                    confirmWarningMessageWindow.SetActive(true);
                                    setActivePopUpWindow = true;
                                    confirmWarningMessageWindow.GetComponentInChildren<Text>().text = "変更したステータスを初期化します";
                                }
                                break;

                            case "YesConfirm":
                                ConfirmStatus();
                                if (setActivePopUpWindow)
                                {
                                    confirmWarningMessageWindow.SetActive(false);
                                    setActivePopUpWindow = false;
                                }
                                confirmResetButtons.SetActive(false);

                                break;

                            case "YesReset":
                                ResetStatus();
                                break;

                            case "No":
                                if (setActivePopUpWindow)
                                {
                                    confirmWarningMessageWindow.SetActive(false);
                                    resetWarningMessageWindow.SetActive(false);
                                    setActivePopUpWindow = false;
                                }
                                break;
                        }
                    }
                }
                if (gesture == TouchGestureDetector.Gesture.TouchMove)
                {
                    //touchGestureDetector.TouchInfo.Diff();

                }
            });
        }

        /// <summary>スキルの説明テキストを表示/非表示</summary>
        /// <param name="index"></param>
        public void SkillDescriptionManager(int index)
        {
            if (setActiveSkillDescription == false)
            {
                skillDescriptionText[index].SetActive(true);
                setActiveSkillDescription = true;
            }
            else
            {
                skillDescriptionText[index].SetActive(false);
                setActiveSkillDescription = false;
            }
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
            skillListWindow.SetActive(false);
            selectAttributeWindow.SetActive(false);
            resetWarningMessageWindow.SetActive(false);
            confirmWarningMessageWindow.SetActive(false);

            setActivePopUpWindow = false;

            confirmResetButtons.SetActive(false);

            setActiveConfirmResetButtons = false;

            passiveSkill = magia.MyPassiveSkill;
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
            magia.Sync();
        }

        /// <summary>テキストを更新</summary>
        public void UpdateText()
        {
            switch (attribute)
            {
                case Magia.Attribute.Standard:
                    attributeImage.sprite = attributeSprite[0];
                    currentAttributeText.text = "魔童";
                    break;
                case Magia.Attribute.MaleWarrior:
                    attributeImage.sprite = attributeSprite[1];
                    currentAttributeText.text = "刀皇";
                    break;
                case Magia.Attribute.FemaleWarrior:
                    attributeImage.sprite = attributeSprite[2];
                    currentAttributeText.text = "剣姫";
                    break;
                case Magia.Attribute.MaleWizard:
                    attributeImage.sprite = attributeSprite[3];
                    currentAttributeText.text = "黒王";
                    break;
                case Magia.Attribute.FemaleWitch:
                    attributeImage.sprite = attributeSprite[4];
                    currentAttributeText.text = "女帝";
                    break;
                case Magia.Attribute.FemaleTrancendental:
                    attributeImage.sprite = attributeSprite[5];
                    currentAttributeText.text = "魔神";
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
                if (totalAddPoint > 0 && setActiveConfirmResetButtons == false)
                {
                    confirmResetButtons.SetActive(true);
                    setActiveConfirmResetButtons = true;
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
                if (totalAddPoint <= 0 && setActiveConfirmResetButtons)
                {
                    totalAddPoint = 0;
                    confirmResetButtons.SetActive(false);
                    setActiveConfirmResetButtons = false;
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