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
        private int hp;

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
        private int allocationPoint;

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

        /// <summary>現在の属性色</summary>
        [SerializeField] private Image currentAttributeColor;

        /// <summary>現在の属性テキスト</summary>
        [SerializeField] private TextMeshProUGUI currentAttributeText;

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
        [SerializeField] private GameObject confirmMessageWindow;

        /// <summary>ステータス初期化前のメッセージウィンドウ</summary>
        [SerializeField] private GameObject resetMessageWindow;

        /// <summary>確定ボタンとリセットボタン</summary>
        [SerializeField] private GameObject confirmResetButtons;

        /// <summary>属性選択ウィンドウ</summary>
        [SerializeField] private GameObject selectAttributeWindow;

        /// <summary>習得済みスキル一覧ウィンドウ</summary>
        [SerializeField] private GameObject skillListWindow;

        /// <summary>習得済みスキル</summary>
        private Magia.PassiveSkill passiveSkill;

        /// <summary>各スキルの説明ウィンドウ</summary>
        [SerializeField] private GameObject[] skillExplanationText = null;

        /// <summary>各スキルの説明</summary>
        private List<string> skillExplanationList = new List<string>();

        /// <summary>ポップアップウィンドウの表示/非表示</summary>
        private bool activePopUpWindow = false;

        /// <summary>スキル説明テキストの表示/非表示</summary>
        private bool setActiveSkillExplanation = false;

        /// <summary>確定/中止ボタンの表示/非表示</summary>
        private bool changedStatus = false;

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }

        private void Start()
        {
            skillExplanationList.Add("街破壊数1以上で発動\n街破壊数×攻撃力の1%\nを加算して攻撃");
            skillExplanationList.Add("街破壊数7以上で発動\n街破壊数×0.5%\n攻撃力防御力を上昇");
            skillExplanationList.Add("街破壊数14以上で発動\n街破壊数×最大 HPの1%回復");
            skillExplanationList.Add("自分の攻撃力2分の1を敵に防御力を\n無視してダメージを与える");
            skillExplanationList.Add("街破壊数19以上で発動\n次の敵の攻撃を5%軽減");
            skillExplanationList.Add("街破壊数21以上で発動\n街破壊数×攻撃力の0.5%を加算して攻撃");
            skillExplanationList.Add("街破壊数24以上で発動　街破壊数×0.5%攻撃力上昇");
            skillExplanationList.Add("街破壊数27以上で発動\n次の敵の攻撃を10%軽減");
            skillExplanationList.Add("破壊数30以上で発動\n破壊数×攻撃力の0.5%を加算して攻撃");
            skillExplanationList.Add("街破壊数32枚以上で発動\n街破壊数×最大HPの2%回復");
            skillExplanationList.Add("街破壊数34以上で発動\n街破壊数×0.5%攻撃力上昇");
            skillExplanationList.Add("街破壊数38以上で発動\n自分の攻撃力1倍を敵に防御力を無視してダメージを与える");
            skillExplanationList.Add("街破壊数36以上で発動\n次の敵の攻撃を無効化");
            skillExplanationList.Add("スキル発動枚数-10");

            for (int i = 0; i < skillExplanationText.Length; i++)
            {
                skillExplanationText[i].GetComponentInChildren<TextMeshProUGUI>().text = skillExplanationList[i];
                skillExplanationText[i].SetActive(false);
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
                                if (!activePopUpWindow)
                                {
                                    selectAttributeWindow.SetActive(true);
                                    activePopUpWindow = true;
                                }
                                break;

                            case "BackAttribute":
                                if (activePopUpWindow)
                                {
                                    selectAttributeWindow.SetActive(false);
                                    activePopUpWindow = false;
                                }
                                break;

                            case "ShowSkillButton":
                                if (!activePopUpWindow)
                                {
                                    skillListWindow.SetActive(true);
                                    activePopUpWindow = true;
                                }
                                break;

                            case "BackSkill":
                                if (activePopUpWindow)
                                {
                                    skillListWindow.SetActive(false);
                                    activePopUpWindow = false;
                                }
                                break;

                            //ここから各スキル名の処理
                            case "DevilsFist":
                                SkillExplanationManager(0);
                                break;
                            case "HighConcentrationMagicalAbsorption":
                                SkillExplanationManager(1);
                                break;
                            case "SelfRegeneration":
                                SkillExplanationManager(2);
                                break;
                            case "ExplosiveFlamePillar":
                                SkillExplanationManager(3);
                                break;
                            case "CrimsonBarrier":
                                SkillExplanationManager(4);
                                break;
                            case "DevilsFistInfernoType":
                                SkillExplanationManager(5);
                                break;
                            case "BraveHeartsIncarnation":
                                SkillExplanationManager(6);
                                break;
                            case "GreatCrimsonBarrier":
                                SkillExplanationManager(7);
                                break;
                            case "InfernosFist":
                                SkillExplanationManager(8);
                                break;
                            case "SatansCell":
                                SkillExplanationManager(9);
                                break;
                            case "AmaterasuIncanation":
                                SkillExplanationManager(10);
                                break;
                            case "AmaterasuInferno":
                                SkillExplanationManager(11);
                                break;
                            case "AmaterasuFlameWall":
                                SkillExplanationManager(12);
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
                                if (!confirmResetButtons)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                changedStatus = true;
                                break;

                            case "Kenki":
                                attribute = Magia.Attribute.BladeEmperor;
                                UpdateText();
                                if (!confirmResetButtons)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                changedStatus = true;
                                break;

                            case "Kokuo":
                                attribute = Magia.Attribute.Empress;
                                UpdateText();
                                if (!confirmResetButtons)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                changedStatus = true;
                                break;

                            case "Majin":
                                attribute = Magia.Attribute.DevilsGod;
                                UpdateText();
                                if (!confirmResetButtons)
                                {
                                    confirmResetButtons.SetActive(true);
                                }
                                changedStatus = true;
                                break;
                            //ここまで各属性名の処理

                            case "ConfirmButton":
                                if (!activePopUpWindow)
                                {
                                    confirmMessageWindow.SetActive(true);
                                    activePopUpWindow = true;
                                    confirmMessageWindow.GetComponentInChildren<Text>().text = "変更したステータスを確定します";
                                }
                                break;

                            case "ResetButton":
                                if (!activePopUpWindow)
                                {
                                    confirmMessageWindow.SetActive(true);
                                    activePopUpWindow = true;
                                    confirmMessageWindow.GetComponentInChildren<Text>().text = "変更したステータスを初期化します";
                                }
                                break;

                            case "YesConfirm":
                                ConfirmStatus();

                                if (activePopUpWindow)
                                {
                                    confirmMessageWindow.SetActive(false);
                                    activePopUpWindow = false;
                                }
                                changedStatus = false;
                                break;

                            case "YesReset":
                                ResetStatus();
                                break;

                            case "No":
                                if (activePopUpWindow)
                                {
                                    confirmMessageWindow.SetActive(false);
                                    resetMessageWindow.SetActive(false);
                                    activePopUpWindow = false;
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

        private void Update()
        {
            if (changedStatus && allocationPoint > 0)
            {
                confirmResetButtons.SetActive(true);
            }
            else if (!changedStatus && allocationPoint <= 0)
            {
                confirmResetButtons.SetActive(false);
            }
        }

        /// <summary>スキルの説明テキストを表示/非表示</summary>
        /// <param name="index"></param>
        public void SkillExplanationManager(int index)
        {
            if (setActiveSkillExplanation)
            {
                skillExplanationText[index].SetActive(false);
                setActiveSkillExplanation = false;
            }
            else
            {
                skillExplanationText[index].SetActive(true);
                setActiveSkillExplanation = true;
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
            uniqueStatusText.text = "+ " + uniqueStatus.ToString();

            //固有ステータスを基礎ステータスに変換
            updatedHitPoint = hp + (addCharm * 50) + (addDignity * 50);
            updatedAttack = attack + (addSense * 5) + (addMuscularStrength * 5);
            updatedDefense = defense + (addDurability * 5) + (addKnowledge * 5);

            updatedBasicStatusTexts[0].text = updatedHitPoint.ToString();
            updatedBasicStatusTexts[1].text = updatedAttack.ToString();
            updatedBasicStatusTexts[2].text = updatedDefense.ToString();

            changedStatus = true;
        }

        /// <summary>ステータスの変動値を初期化</summary>
        public void ResetStatus()
        {
            skillListWindow.SetActive(false);
            selectAttributeWindow.SetActive(false);
            resetMessageWindow.SetActive(false);
            confirmMessageWindow.SetActive(false);

            activePopUpWindow = false;

           
            changedStatus = false;

            passiveSkill = magia.MyPassiveSkill;
            attribute = magia.MyAttribute;

            var getStats = magia.GetStats();
            hp = getStats.m_hitPoint;
            attack = getStats.m_attack;
            defense = getStats.m_defense;
            charm = getStats.m_charm;
            dignity = getStats.m_dignity;
            muscularStrength = getStats.m_muscularStrength;
            sense = getStats.m_sense;
            durability = getStats.m_durability;
            knowledge = getStats.m_knowledge;

            addCharm = 0;
            addDignity = 0;
            addMuscularStrength = 0;
            addSense = 0;
            addDurability = 0;
            addKnowledge = 0;
            //StatusPoint = magia.AllocationPoint;
            statusPoint = 10;//debug

            UpdateText();
        }

        /// <summary>変更したステータスを確定する</summary>
        public void ConfirmStatus()
        {
            hp = updatedHitPoint;
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

        /// <summary>割り振りポイント-1、固有ステータスポイント+1</summary>
        /// <param name="uniqueStatus">固有ステータス</param>
        public int AddStatusPoint(int uniqueStatus)
        {
            if (statusPoint > 0)
            {
                statusPoint -= 1;
                statusPointText.text = statusPoint.ToString();
                uniqueStatus = 1;

                allocationPoint += 1;
                if (!changedStatus)
                {
                    changedStatus = true;
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

                allocationPoint -= 1;
                if (allocationPoint <= 0 && changedStatus)
                {
                    allocationPoint = 0;
                    changedStatus = false;
                }
            }
            else
            {
                uniqueStatus = 0;
            }
            return uniqueStatus;
        }

        /// <summary>テキストを更新</summary>
        public void UpdateText()
        {
            switch (attribute)
            {
                case Magia.Attribute.Standard:
                    attributeImage.sprite = attributeSprite[0];
                    currentAttributeText.text = "魔童";
                    currentAttributeColor.color = new Color(1, 0, 0, 0.5f);//赤
                    break;

                case Magia.Attribute.BladeEmperor:
                    attributeImage.sprite = attributeSprite[1];
                    currentAttributeText.text = "剣姫";
                    currentAttributeColor.color = new Color(0, 0, 1, 0.5f);//青
                    break;

                case Magia.Attribute.Empress:
                    attributeImage.sprite = attributeSprite[2];
                    currentAttributeText.text = "黒王";
                    currentAttributeColor.color = new Color(0, 1, 0, 0.5f);//緑
                    break;

                case Magia.Attribute.DevilsGod:
                    attributeImage.sprite = attributeSprite[3];
                    currentAttributeText.text = "魔神";
                    currentAttributeColor.color = new Color(1, 0, 1, 0.5f);//紫
                    break;
            }

            currentBasicStatusTexts[0].text = hp.ToString();
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

            addUniqueStatusTexts[0].text = "+" + addCharm.ToString();
            addUniqueStatusTexts[1].text = "+" + addDignity.ToString();
            addUniqueStatusTexts[2].text = "+" + addMuscularStrength.ToString();
            addUniqueStatusTexts[3].text = "+" + addSense.ToString();
            addUniqueStatusTexts[4].text = "+" + addDurability.ToString();
            addUniqueStatusTexts[5].text = "+" + addKnowledge.ToString();

            statusPointText.text = statusPoint.ToString();
        }
    }
}