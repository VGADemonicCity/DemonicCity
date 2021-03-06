﻿using System.Collections;
using UnityEngine;
using TMPro;
using DemonicCity.BattleScene;
using DemonicCity.StrengthenScene;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace DemonicCity.ResultScene
{
    public class ResultManager : MonoBehaviour
    {
        /// <summary>ちゃんマギの最大レベル</summary>
        private const int maxLevel = 200;

        Magia magia;
        PanelCounter panelCounter;
        TouchGestureDetector touchGestureDetector;
        Progress progress;

        /// <summary>バトル前のちゃんマギのステータス</summary>
        Status beforeStatus;

        /// <summary>バトル後のちゃんマギのステータス</summary>
        Status afterStatus;

        /// <summary></summary>
        LevelTextAnimation levelTextAnimation;

        /// <summary>経験値ゲージの枠画像</summary>
        [SerializeField] private Sprite defaultGaugeSprite = null;

        /// <summary>レベルアップしたときの経験値ゲージの枠画像</summary>
        [SerializeField] private Sprite levelUpGaugeSprite = null;

        /// <summary>1レベルアップごとに格納する各パラメータ</summary>
        List<int> levelDifference = new List<int>();
        List<int> hpDifference = new List<int>();
        List<int> attackDifference = new List<int>();
        List<int> defenceDifference = new List<int>();
        List<int> statusPointDifferences = new List<int>();

        /// <summary>各レベルで必要な総経験値</summary>
        List<int> requiredExperiences = new List<int>();
        List<int> experienceDifferences = new List<int>();

        /// <summary>レベルアップによって習得したスキル</summary>
        List<string> masterdSkillNames = new List<string>();

        /// <summary>画面をタップした回数</summary>
        private int tapCount = 0;

        /// <summary>獲得した総魔力値</summary>
        private int getStatusPoint = 0;

        /// <summary>レベルアップの画像</summary>
        [SerializeField] private GameObject levelUpImage = null;

        /// <summary>最大レベルの画像</summary>
        [SerializeField] private GameObject levelMaxImage = null;

        /// <summary>スキル習得通知ウィンドウ</summary>
        [SerializeField] private GameObject skillMasterdMessageWindow = null;
        private GameObject[] masterdSkillNameText = null;

        /// <summary>各スキルを習得するレベル</summary>
        private int[] skillMasterdLevelList = { 1,11,23,44,58,70,82,100,111,136,160,181,198 };

        /// <summary>スキル名</summary>
        private string skillName;

        private ChapterManager chapterManager;

        [SerializeField] private TextMeshProUGUI currentLevelText = null;
        [SerializeField] private TextMeshProUGUI nextLevelText = null;
        [SerializeField] private GameObject beforeHpText = null;
        [SerializeField] private GameObject beforeAttackText = null;
        [SerializeField] private GameObject beforeDefenceText = null;
        [SerializeField] private TextMeshProUGUI afterHpText = null;
        [SerializeField] private TextMeshProUGUI afterAttackText = null;
        [SerializeField] private TextMeshProUGUI afterDefenseText = null;

        [SerializeField] private TextMeshProUGUI destructionCountText = null;
        [SerializeField] private TextMeshProUGUI needDestructionCountText = null;
        [SerializeField] private TextMeshProUGUI getStatusPointText = null;

        [SerializeField] private GameObject[] rightArrows = null;

        [SerializeField] private Slider experienceGauge = null;
        [SerializeField] private Image gaugeBackGround = null;

        [SerializeField] private GameObject toNextWindow = null;
        [SerializeField] private Transform backGround = null;
        [SerializeField] private float gaugeMoveSpeed = 0.05f;

        private float addAmount = 0;
        private bool isAnimation = false;
        private int index = 0;
        private int destructionCount;
        private bool isLevelUp = false;

        int totalExperience = 0;
        int myExperience = 0;

        private bool isCalculation = true;
        private bool isPopup = false;

        /// <summary>現在のレベルからレベルアップするまでに必要とされる総経験値</summary>
        int requiredToNextLevelTotalExperience;

        private void Awake()
        {
            chapterManager = ChapterManager.Instance;
            magia = Magia.Instance;
            panelCounter = PanelCounter.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            progress = Progress.Instance;
        }

        private void Start()
        {
            //SavableSingletonBase<Magia>.Instance.Clear();//debug

            //クリアフラグを立てる
            progress.QuestClear();
            SavableSingletonBase<Progress>.Instance.Save();

            InvalidObjects();

            ReflectionBeforeStatus();

            if(beforeStatus.Level < maxLevel && destructionCount > 0)
            {
                ResultCalculation();
                afterStatus = magia.Stats;
            }
            else if(beforeStatus.Level == maxLevel)
            {
                levelMaxImage.SetActive(true);
                gaugeBackGround.sprite = levelUpGaugeSprite;
                experienceGauge.value = experienceGauge.maxValue;
                needDestructionCountText.text = "0";
                isCalculation = false;
            }

            //ステータスをセーブ
            SavableSingletonBase<Magia>.Instance.Save();

            touchGestureDetector.onGestureDetected.AddListener((gesture,touchInfo) =>
            {
                if(gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    if(isCalculation)
                    {
                        tapCount++;

                        if(tapCount == 1)
                        {
                            isAnimation = true;
                        }
                        else if(tapCount == 2 && isAnimation)//演出スキップ
                        {
                            ReflectionAfterStatus();
                            nextLevelText.text = "";
                            isAnimation = false;
                        }
                        else if(tapCount == 3 || tapCount == 2)//スキップした場合||スキップしなかった場合
                        {
                            gaugeBackGround.sprite = defaultGaugeSprite;

                            if(masterdSkillNames.Count > 0)//スキルを1つ以上習得した場合
                            {
                                StartCoroutine(PopUp_SkillMasterdMessageWindow());//スキル習得したことを通知
                            }

                            if(beforeStatus.Level >= maxLevel)
                            {
                                magia.Stats.Level = maxLevel;
                                experienceGauge.value = experienceGauge.maxValue;
                                needDestructionCountText.text = "0";
                                StartCoroutine(ClosePopUpAnimation(levelUpImage));
                            }
                        }
                        else if(tapCount == 4)
                        {
                            if(ChapterManager.Instance.GetChapter().isStory)//会話シーンがあれば
                            {
                                SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Story);//会話シーンへ
                            }
                            else if(!isPopup && !ChapterManager.Instance.GetChapter().isStory)
                            {
                                Instantiate(toNextWindow,backGround);//ホーム画面か次の章へ
                                isPopup = true;

                            }
                        }
                    }
                    else
                    {
                        if(ChapterManager.Instance.GetChapter().isStory)
                        {
                            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Story);
                        }
                        else if(!isPopup && !ChapterManager.Instance.GetChapter().isStory)
                        {
                            Instantiate(toNextWindow,backGround);//ホーム画面か次の章へ
                            isPopup = true;
                        }
                    }
                }
            });
        }

        /// <summary>レベルに対応したスキル名を返す</summary>
        private string GetSkillName(int level)
        {
            switch(level)
            {
                //case 1:
                //    skillName = "魔拳";
                //    magia.MyPassiveSkill |= Magia.PassiveSkill.DevilsFist;
                //    break;
                case 11:
                    skillName = "高濃度魔力吸収";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.HighConcentrationMagicalAbsorption;
                    break;
                case 23:
                    skillName = "自己再生";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.SelfRegeneration;
                    break;
                //case 31:
                //    skillName = "爆炎熱風柱";
                //    magia.MyPassiveSkill |= Magia.PassiveSkill.ExplosiveFlamePillar;
                //    break;
                case 44:
                    skillName = "紅蓮障壁";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.CrimsonBarrier;
                    break;
                case 58:
                    skillName = "魔拳烈火ノ型";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.DevilsFistInfernoType;
                    break;
                case 70:
                    skillName = "心焔権現";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.BraveHeartsIncarnation;
                    break;
                case 82:
                    skillName = "大紅蓮障壁";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.GreatCrimsonBarrier;
                    break;
                case 100:
                    skillName = "豪炎爆砕掌";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.InfernosFist;
                    break;
                case 111:
                    skillName = "魔王ノ細胞";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.SatansCell;
                    break;
                case 136:
                    skillName = "天照権現";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.AmaterasuIncarnation;
                    break;
                case 160:
                    skillName = "天照-爆炎-";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.AmaterasuInferno;
                    break;
                case 181:
                    skillName = "天照-焔壁-";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.AmaterasuFlameWall;
                    break;
                case 198:
                    skillName = "王ノ器";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.AllSkill;
                    break;
            }
            return skillName;
        }

        private void Update()
        {
            if(isAnimation && isLevelUp && destructionCount > 0)
            {
                LevelUpPerformance();
            }
            else if(isAnimation && !isLevelUp && destructionCount >= 0)
            {
                NotLevelUpPerformance();
            }
        }

        private IEnumerator PopUp_SkillMasterdMessageWindow()
        {
            skillMasterdMessageWindow.SetActive(true);

            for(int i = 0; i < masterdSkillNames.Count; i++)
            {
                masterdSkillNameText[i].SetActive(true);
                //習得したスキルを全て表示
                masterdSkillNameText[i].GetComponent<TextMeshProUGUI>().text = masterdSkillNames[i];
            }
            yield return new WaitForSeconds(5f);
        }

        /// <summary>ウィンドウが閉じるときのアニメーション処理</summary>
        private IEnumerator ClosePopUpAnimation(GameObject window)
        {
            window.GetComponent<Animator>().CrossFadeInFixedTime(Strengthen_Part1.PopUpAnimation.Close_PopUpWindow.ToString(),0);
            yield return new WaitForSeconds(0.5f);
            window.SetActive(false);
        }

        /// <summary>経験値を計算</summary>
        private void ResultCalculation()
        {
            totalExperience = myExperience + destructionCount;

            if(requiredToNextLevelTotalExperience <= totalExperience)
            {
                isLevelUp = true;
                int getTotalStatusPoint = 0;

                // 総経験値がレベルアップに必要な経験値よりも高かった場合条件が満たさなくなる迄レベルアップ処理を行う
                while(totalExperience >= requiredToNextLevelTotalExperience && requiredToNextLevelTotalExperience > 0 && isLevelUp && magia.Stats.Level < maxLevel)
                {
                    totalExperience -= requiredToNextLevelTotalExperience;

                    magia.LevelUp(out getStatusPoint);
                    levelDifference.Add(magia.Stats.Level);
                    hpDifference.Add(magia.Stats.HitPoint);
                    attackDifference.Add(magia.Stats.Attack);
                    defenceDifference.Add(magia.Stats.Defense);
                    getTotalStatusPoint += getStatusPoint;
                    statusPointDifferences.Add(getTotalStatusPoint);
                    requiredToNextLevelTotalExperience = GetRequiredTotalExperience(magia.Stats.Level);
                    requiredExperiences.Add(requiredToNextLevelTotalExperience);
                }

                magia.MyExperience = totalExperience;

                //レベルアップした際のレベルとスキルを習得するレベルを照合
                for(int i = 0; i < levelDifference.Count; i++)
                {
                    for(int a = 0; a < skillMasterdLevelList.Length; a++)
                    {
                        if(levelDifference[i] == skillMasterdLevelList[a])//一致した場合、そのレベルに応じたスキルを習得したと判定する
                        {
                            string name = GetSkillName(skillMasterdLevelList[a]);
                            masterdSkillNames.Add(name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// レベルに応じた必要な総経験値を返す
        /// </summary>
        private int GetRequiredTotalExperience(int currentLevel)
        {
            int requiredExp = 0;

            if(1 <= currentLevel && currentLevel <= 30)
            {
                requiredExp = 5;
            }
            else if(31 <= currentLevel && currentLevel <= 80)
            {
                requiredExp = 10;
            }
            else if(81 <= currentLevel && currentLevel <= 140)
            {
                requiredExp = 15;
            }
            else if(141 <= currentLevel && currentLevel <= 200)
            {
                requiredExp = 20;
            }

            return requiredExp;
        }

        /// <summary>レベルアップするときの演出</summary>
        private void LevelUpPerformance()
        {
            addAmount = gaugeMoveSpeed;
            experienceGauge.value += addAmount;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");

            if(experienceGauge.value >= experienceGauge.maxValue)
            {
                gaugeBackGround.sprite = levelUpGaugeSprite;//目玉がぴかーーーん！


                experienceGauge.value = 0;
                if(requiredExperiences.Count > 0)
                {
                    experienceGauge.maxValue = requiredExperiences[index];
                }
                else
                {
                    experienceGauge.maxValue = GetRequiredTotalExperience(magia.Stats.Level);
                }

                levelUpImage.SetActive(true);

                currentLevelText.text = "";
                nextLevelText.text = levelDifference[index].ToString() + "\n" + (levelDifference[index] - 1).ToString();
                levelTextAnimation.TextAnimation(LevelTextAnimation.AnimationClip.TopToBottom);

                beforeHpText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f,-197,0);
                beforeAttackText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f,-297,0);
                beforeDefenceText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f,-397,0);

                afterHpText.text = hpDifference[index].ToString();
                afterAttackText.text = attackDifference[index].ToString();
                afterDefenseText.text = defenceDifference[index].ToString();
                getStatusPointText.text = statusPointDifferences[index].ToString();

                for(int i = 0; i < rightArrows.Length; i++)
                {
                    rightArrows[i].SetActive(true);
                }

                addAmount = gaugeMoveSpeed;

                if(levelDifference[index] >= maxLevel)//レベルアップ中に最大レベルに達したら
                {
                    magia.Stats.Level = maxLevel;
                    experienceGauge.maxValue = experienceDifferences.Last();
                    experienceGauge.value = experienceGauge.maxValue;
                    needDestructionCountText.text = "0";
                    StartCoroutine(ClosePopUpAnimation(levelUpImage));
                    levelMaxImage.SetActive(true);
                    isAnimation = false;
                }

                index++;

                if(index == levelDifference.Count)
                {
                    StartCoroutine(GaugeAnimation());
                    gaugeBackGround.sprite = defaultGaugeSprite;
                }
            }
        }
        /// <summary>レベルアップしたあとの残りの経験値を足すアニメーション</summary>
        IEnumerator GaugeAnimation()
        {
            experienceGauge.maxValue = requiredExperiences.Last();

            while((int)experienceGauge.value < myExperience)
            {
                experienceGauge.value += addAmount;
                needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");
                yield return new WaitForSeconds(0.02f);
            }
            experienceGauge.value = myExperience;
            needDestructionCountText.text = (requiredExperiences.Last() - myExperience).ToString();
            isAnimation = false;
        }

        /// <summary>レベルアップしないときの演出</summary>
        private void NotLevelUpPerformance()
        {
            addAmount = gaugeMoveSpeed;
            experienceGauge.value += addAmount;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");

            if((int)experienceGauge.value == totalExperience)
            {
                isAnimation = false;
            }
        }

        /// <summary>バトル前のステータスをテキストに反映する</summary>
        private void ReflectionBeforeStatus()
        {
            beforeStatus = magia.Stats;

            currentLevelText.text = beforeStatus.Level.ToString();
            beforeHpText.GetComponent<TextMeshProUGUI>().text = beforeStatus.HitPoint.ToString();
            beforeAttackText.GetComponent<TextMeshProUGUI>().text = beforeStatus.Attack.ToString();
            beforeDefenceText.GetComponent<TextMeshProUGUI>().text = beforeStatus.Defense.ToString();

            afterHpText.text = "";
            afterAttackText.text = "";
            afterDefenseText.text = "";

            destructionCount = panelCounter.TotalDestructionCount;
            int recommendationMinLevel = chapterManager.GetChapter().levelRange[0];//最低適正レベル
            int recommendationMaxLevel = chapterManager.GetChapter().levelRange[1];//最高適正レベル

            //destructionCount = 300;//debug

            //適正レベル以下だったら
            if(beforeStatus.Level <= recommendationMinLevel)
            {
                float temporary = destructionCount * 1.5f;
                destructionCount = Mathf.CeilToInt(temporary);
            }
            //適正レベル以上だったら
            else if(beforeStatus.Level >= recommendationMaxLevel)
            {
                destructionCount = 0;
                needDestructionCountText.text = "0";
                isCalculation = false;
            }
            destructionCountText.text = destructionCount.ToString();

            getStatusPointText.text = "0";

            requiredToNextLevelTotalExperience = GetRequiredTotalExperience(beforeStatus.Level);
            experienceGauge.maxValue = requiredToNextLevelTotalExperience;
            myExperience = magia.MyExperience;
            experienceGauge.value = myExperience;

            needDestructionCountText.text = (requiredToNextLevelTotalExperience - myExperience).ToString();
        }

        /// <summary>バトル後のステータスをテキストに反映する(演出スキップ)</summary>
        private void ReflectionAfterStatus()
        {
            if(isLevelUp)
            {
                levelUpImage.SetActive(true);

                //レベルアップ前のステータステキストを左に移動
                beforeHpText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f,-197,0);
                beforeAttackText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f,-297,0);
                beforeDefenceText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f,-397,0);

                for(int i = 0; i < rightArrows.Length; i++)
                {
                    rightArrows[i].SetActive(true);//→を表示
                }

                //レベルアップ後のステータスを表示
                currentLevelText.text = afterStatus.Level.ToString();
                afterHpText.text = afterStatus.HitPoint.ToString();
                afterAttackText.text = afterStatus.Attack.ToString();
                afterDefenseText.text = afterStatus.Defense.ToString();
            }


            if(destructionCount > 0)
            {
                experienceGauge.value = myExperience;

                if(isLevelUp)
                {
                    experienceGauge.maxValue = requiredExperiences.Last();
                    needDestructionCountText.text = (requiredExperiences.Last() - myExperience).ToString();
                    getStatusPointText.text = statusPointDifferences.Last().ToString();

                    if(afterStatus.Level >= maxLevel)
                    {
                        magia.Stats.Level = maxLevel;
                        currentLevelText.text = maxLevel.ToString();
                        experienceGauge.value = experienceGauge.maxValue;
                        needDestructionCountText.text = "0";
                        StartCoroutine(ClosePopUpAnimation(levelUpImage));
                        levelMaxImage.SetActive(true);
                    }
                }
                else
                {
                    experienceGauge.maxValue = myExperience;
                    needDestructionCountText.text = "0";
                    getStatusPointText.text = "0";
                }
            }

        }

        /// <summary>オブジェクトを無効にする</summary>
        private void InvalidObjects()
        {
            levelUpImage.SetActive(false);
            levelMaxImage.SetActive(false);

            for(int i = 0; i < rightArrows.Length; i++)
            {
                rightArrows[i].SetActive(false);
            }
            nextLevelText.text = "";
            levelTextAnimation = FindObjectOfType<LevelTextAnimation>();
            masterdSkillNameText = GameObject.FindGameObjectsWithTag("MasterdSkillName");
            for(int i = 0; i < masterdSkillNameText.Length; i++)
            {
                masterdSkillNameText[i].SetActive(false);
            }
            skillMasterdMessageWindow.SetActive(false);
        }
    }
}
