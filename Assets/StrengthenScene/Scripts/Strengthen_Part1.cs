using UnityEngine;
using TMPro;
using System;
using DemonicCity.BattleScene;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DemonicCity.StrengthenScene
{
    public class Strengthen_Part1 : MonoBehaviour
    {
        /// <summary>Magiaクラスのインスタンス</summary>
        Magia magia;
        /// <summary>TouchGestureDetectorクラスのインスタンス</summary>
        TouchGestureDetector touchGestureDetector;

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

        /// <summary>ステータス確定前のメッセージウィンドウ</summary>
        [SerializeField] private GameObject confirmMessageWindow;
        /// <summary>ステータス初期化前のメッセージウィンドウ</summary>
        [SerializeField] private GameObject resetMessageWindow;
        /// <summary>確定ボタンとリセットボタン</summary>
        [SerializeField] private GameObject confirmResetButtons;
        /// <summary>習得済みスキル一覧ウィンドウ</summary>
        [SerializeField] private GameObject skillListWindow;
        /// <summary>各スキル名</summary>
        private GameObject[] skillNameTexts;
        /// <summary>各スキルの説明</summary>
        private GameObject[] skillExplanationTexts = null;
        private GameObject notAcquiredMessage = null;

        /// <summary>ポップアップウィンドウの表示/非表示</summary>
        private bool activePopUpWindow = false;
        /// <summary>スキル説明テキストの表示/非表示</summary>
        private int activeSkillDescriptionText;
        private bool setActiveSkillDescriptionText = false;
        /// <summary>確定/中止ボタンの表示/非表示</summary>
        private bool changedStatus = false;


        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
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
                            case "BackToHomeSceneButton":
                                SceneChanger.SceneChange(SceneName.Home);
                                break;

                            case "ShowSkillButton":
                                if (!activePopUpWindow)
                                {
                                    skillListWindow.SetActive(true);
                                    notAcquiredMessage = GameObject.Find("NotAcquiredMessage");
                                    notAcquiredMessage.SetActive(false);
                                    if (magia.MyPassiveSkill == Magia.PassiveSkill.Invalid)
                                    {
                                        notAcquiredMessage.SetActive(true);
                                    }

                                    skillExplanationTexts = GameObject.FindGameObjectsWithTag("SkillDescriptionText");
                                    for (int i = 0; i < skillExplanationTexts.Length; i++)
                                    {
                                        skillExplanationTexts[i].SetActive(false);
                                    }
                                    activePopUpWindow = true;
                                }
                                break;

                            case "BackToAllocationWindow":
                                if (activePopUpWindow)
                                {
                                    skillListWindow.SetActive(false);
                                    activePopUpWindow = false;
                                }
                                break;

                            //ここから各スキル名の処理
                            case "DevilsFist":
                                SkillDescriptionManager(0);
                                activeSkillDescriptionText = 0;
                                break;
                            case "HighConcentrationMagicalAbsorption":
                                SkillDescriptionManager(1);
                                activeSkillDescriptionText = 1;
                                break;
                            case "SelfRegeneration":
                                SkillDescriptionManager(2);
                                activeSkillDescriptionText = 2;
                                break;
                            case "ExplosiveFlamePillar":
                                SkillDescriptionManager(3);
                                activeSkillDescriptionText = 3;
                                break;
                            case "CrimsonBarrier":
                                SkillDescriptionManager(4);
                                activeSkillDescriptionText = 4;
                                break;
                            case "DevilsFistInfernoType":
                                SkillDescriptionManager(5);
                                activeSkillDescriptionText = 5;
                                break;
                            case "BraveHeartsIncarnation":
                                SkillDescriptionManager(6);
                                activeSkillDescriptionText = 6;
                                break;
                            case "GreatCrimsonBarrier":
                                SkillDescriptionManager(7);
                                activeSkillDescriptionText = 7;
                                break;
                            case "InfernosFist":
                                SkillDescriptionManager(8);
                                activeSkillDescriptionText = 8;
                                break;
                            case "SatansCell":
                                SkillDescriptionManager(9);
                                activeSkillDescriptionText = 9;
                                break;
                            case "AmaterasuIncanation":
                                SkillDescriptionManager(10);
                                activeSkillDescriptionText = 10;
                                break;
                            case "AmaterasuInferno":
                                SkillDescriptionManager(11);
                                activeSkillDescriptionText = 11;
                                break;
                            case "AmaterasuFlameWall":
                                SkillDescriptionManager(12);
                                activeSkillDescriptionText = 12;
                                break;
                            case "AllSkill":
                                SkillDescriptionManager(13);
                                activeSkillDescriptionText = 13;
                                break;
                            //ここまで各スキル名の処理

                            case "AddCharmButton":
                                ChangeUniqueStatus(ref addCharm, ref addUniqueStatusTexts[0]);
                                break;

                            case "AddDignityButton":
                                ChangeUniqueStatus(ref addDignity, ref addUniqueStatusTexts[1]);
                                break;

                            case "AddMuscularStrengthButton":
                                ChangeUniqueStatus(ref addMuscularStrength, ref addUniqueStatusTexts[2]);
                                break;

                            case "AddSenseButton":
                                ChangeUniqueStatus(ref addSense, ref addUniqueStatusTexts[3]);
                                break;

                            case "AddDurabilityButton":
                                ChangeUniqueStatus(ref addDurability, ref addUniqueStatusTexts[4]);
                                break;

                            case "AddKnowledgeButton":
                                ChangeUniqueStatus(ref addKnowledge, ref addUniqueStatusTexts[5]);
                                break;

                            case "ConfirmButton":
                                if (!activePopUpWindow)
                                {
                                    confirmMessageWindow.SetActive(true);
                                    activePopUpWindow = true;
                                }
                                break;

                            case "ResetButton":
                                if (!activePopUpWindow)
                                {
                                    resetMessageWindow.SetActive(true);
                                    activePopUpWindow = true;
                                }
                                break;

                            case "YesConfirm":
                                ConfirmStatus();
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
            });
        }


        private void Update()
        {
            if (changedStatus)
            {
                confirmResetButtons.SetActive(true);
            }
            else if (!changedStatus)
            {
                confirmResetButtons.SetActive(false);
            }
        }

        /// <summary>スキルの説明テキストを表示/非表示</summary>
        private void SkillDescriptionManager(int index)
        {
            if (activeSkillDescriptionText == index && !setActiveSkillDescriptionText)
            {
                skillExplanationTexts[index].SetActive(true);
                setActiveSkillDescriptionText = true;
            }
            else if (activeSkillDescriptionText == index && setActiveSkillDescriptionText)
            {
                skillExplanationTexts[index].SetActive(false);
                setActiveSkillDescriptionText = false;
            }
        }

        ///<summary>ScrollRectのスクロール位置をGameObjectにあわせる</summary>
        /// <param name="align">0:下、0.5:中央、1:上</param>
        private float ScrollToCore(ScrollRect scrollRect, GameObject go, float align)
        {
            var targetRect = go.transform.GetComponent<RectTransform>();
            var contentHeight = scrollRect.content.rect.height;
            var viewportHeight = scrollRect.viewport.rect.height;
            // スクロール不要
            if (contentHeight < viewportHeight)
            {
                return 0f;
            }
            // ローカル座標が contentHeight の上辺を0として負の値で格納されてる
            // これは現在のレイアウト特有なのかもしれないので、要確認
            var targetPos = contentHeight + (targetRect.localPosition.y + targetRect.rect.y) + targetRect.rect.height * align;
            var gap = viewportHeight * align; // 上端〜下端あわせのための調整量
            var normalizedPos = (targetPos - gap) / (contentHeight - viewportHeight);

            normalizedPos = Mathf.Clamp01(normalizedPos);
            scrollRect.verticalNormalizedPosition = normalizedPos;
            return normalizedPos;
        }

        /// <summary>魔力値を固有ステータスに割り振り、基礎ステータスに変換する</summary>
        /// <param name="uniqueStatus">固有ステータス</param>
        /// <param name="uniqueStatusText">固有ステータスのテキスト</param>
        /// <param name="addStatus">ステータスの増減判定</param>
        private void ChangeUniqueStatus(ref int uniqueStatus, ref TextMeshProUGUI uniqueStatusText)
        {
            uniqueStatus += AddStatusPoint(uniqueStatus);
            if (statusPoint > 0)
            {
                uniqueStatusText.text = "+" + uniqueStatus.ToString();
                //固有ステータスを基礎ステータスに変換
                updatedHitPoint = hp + (addCharm * 50) + (addDignity * 50);
                updatedAttack = attack + (addSense * 5) + (addMuscularStrength * 5);
                updatedDefense = defense + (addDurability * 5) + (addKnowledge * 5);

                updatedBasicStatusTexts[0].text = updatedHitPoint.ToString();
                updatedBasicStatusTexts[1].text = updatedAttack.ToString();
                updatedBasicStatusTexts[2].text = updatedDefense.ToString();

                changedStatus = true;
            }
        }

        /// <summary>ステータスの変動値を初期化</summary>
        private void ResetStatus()
        {
            var status = magia.GetStats();

            hp = status.HitPoint;
            attack = status.Attack;
            defense = status.Defense;
            charm = status.Charm;
            dignity = status.Dignity;
            muscularStrength = status.MuscularStrength;
            sense = status.Sense;
            durability = status.Durability;
            knowledge = status.Knowledge;

            addCharm = 0;
            addDignity = 0;
            addMuscularStrength = 0;
            addSense = 0;
            addDurability = 0;
            addKnowledge = 0;
            //statusPoint = magia.AllocationPoint;
            statusPoint = 99;//debug

            UpdateText();

            skillListWindow.SetActive(false);
            resetMessageWindow.SetActive(false);
            confirmMessageWindow.SetActive(false);
            activePopUpWindow = false;
            changedStatus = false;
        }

        /// <summary>変更したステータスを確定する</summary>
        private void ConfirmStatus()
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

            magia.Stats.HitPoint = hp;
            magia.Stats.Attack = attack;
            magia.Stats.Defense = defense;
            magia.Stats.Charm = charm;
            magia.Stats.Dignity = dignity;
            magia.Stats.MuscularStrength = muscularStrength;
            magia.Stats.Sense =sense;
            magia.Stats.Durability = durability;
            magia.Stats.Knowledge = knowledge;


            //magia.Sync();

            UpdateText();

            confirmMessageWindow.SetActive(false);
            activePopUpWindow = false;
            changedStatus = false;
        }

        /// <summary>割り振りポイント-1、固有ステータスポイント+1</summary>
        /// <param name="uniqueStatus">固有ステータス</param>
        private int AddStatusPoint(int uniqueStatus)
        {
            if (statusPoint > 0)
            {
                statusPoint -= 1;
                statusPointText.text = statusPoint.ToString();
                uniqueStatus = 1;
            }
            else
            {
                uniqueStatus = 0;
                statusPoint = 0;
            }
            return uniqueStatus;
        }

        /// <summary>テキストを更新</summary>
        private void UpdateText()
        {
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

            addUniqueStatusTexts[0].text = addCharm.ToString();
            addUniqueStatusTexts[1].text = addDignity.ToString();
            addUniqueStatusTexts[2].text = addMuscularStrength.ToString();
            addUniqueStatusTexts[3].text = addSense.ToString();
            addUniqueStatusTexts[4].text = addDurability.ToString();
            addUniqueStatusTexts[5].text = addKnowledge.ToString();

            statusPointText.text = statusPoint.ToString();
        }
    }
}