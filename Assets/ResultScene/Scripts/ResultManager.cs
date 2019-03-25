using System.Collections;
using UnityEngine;
using TMPro;
using DemonicCity.BattleScene;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;


namespace DemonicCity.ResultScene
{
    public class ResultManager : MonoBehaviour
    {
        Magia magia;
        PanelCounter panelCounter;
        TouchGestureDetector touchGestureDetector;
        Status beforeStatus;
        Status afterStatus;

        List<int> levelDifference = new List<int>();
        List<int> hpDifference = new List<int>();
        List<int> attackDifference = new List<int>();
        List<int> defenceDifference = new List<int>();
        List<int> requiredExperiences = new List<int>();

        private int tapCount = 0;
        private int needRemainExperience;
        private GameObject levelUpImage = null;

        private TextMeshProUGUI levelText = null;
        private TextMeshProUGUI beforeHpText = null;
        private TextMeshProUGUI beforeAttackText = null;
        private TextMeshProUGUI beforeDefenseText = null;
        private TextMeshProUGUI afterHpText = null;
        private TextMeshProUGUI afterAttackText = null;
        private TextMeshProUGUI afterDefenseText = null;

        private TextMeshProUGUI destructionCountText = null;
        private TextMeshProUGUI needDestructionCountText = null;
        private TextMeshProUGUI statusPointText = null;

        private Slider experienceGauge = null;
        private float addAmount = 0.1f;
        private bool isAnimation = false;
        int index = 0;
        private int destructionCount;
        private int currentLevelTotalExperience;
        private bool isLevelUp = false;
        int afterTotalExperience;
        int experienceDifference;

        private void Awake()
        {
            magia = Magia.Instance;
            panelCounter = PanelCounter.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }
        private void Start()
        {
            GetGameObject();

            beforeStatus = magia.Stats;

            ReflectionBeforeStatus();


            ResultCalculation();
            afterStatus = magia.Stats;

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    tapCount ++;

                    if (tapCount == 1)
                    {
                        isAnimation = true;
                    }
                    else if (tapCount == 2 && isAnimation)
                    {
                        ReflectionAfterStatus();
                        isAnimation = false;
                    }
                    else if (tapCount == 3 || !isAnimation)
                    {
                        SceneChanger.SceneChange(SceneName.Story);
                    }
                }
            });
        }

        private void Update()
        {

            if (isAnimation && isLevelUp)
            {
                LevelUpPerformance();
            }
            else if (isAnimation && !isLevelUp)
            {
                NotLevelUpPerformance();
            }
        }

        void ResultCalculation()
        {
            currentLevelTotalExperience = magia.TotalExperience;
            //currentExperience = 1;//debug
            destructionCount= panelCounter.TotalDestructionCount;
            destructionCount = 6;//debug

            afterTotalExperience = currentLevelTotalExperience + destructionCount;

            int requiredTotalExperience = magia.GetRequiredExpToNextLevel(magia.Stats.Level);

            experienceGauge.value = currentLevelTotalExperience;
            experienceGauge.maxValue = requiredTotalExperience;//レベルアップに必要な総経験値をゲージの最大値に設定
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");

            //needRemainExperience = requiredTotalExperience - totalExperience;

            if (requiredTotalExperience - afterTotalExperience <= 0)
            {
                isLevelUp = true;
            }

            if (isLevelUp)
            {
                // 総経験値がレベルアップに必要な経験値よりも高かった場合条件が満たさなくなる迄レベルアップ処理を行う
                while (afterTotalExperience >= requiredTotalExperience)
                {
                    magia.LevelUp();

                    levelDifference.Add(magia.Stats.Level);
                    hpDifference.Add(magia.Stats.HitPoint);
                    attackDifference.Add(magia.Stats.Attack);
                    defenceDifference.Add(magia.Stats.Defense);

                    requiredTotalExperience = magia.GetRequiredExpToNextLevel(magia.Stats.Level);
                    requiredExperiences.Add(requiredTotalExperience);
                }
                //needRemainExperience = requiredExperiences.Last() - totalExperience;
            }
        }

        /// <summary>シーン上にあるオブジェクトを取得</summary>
        private void GetGameObject()
        {
            levelUpImage = GameObject.Find("LevelUpImage");
            levelUpImage.SetActive(false);
            levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
            beforeHpText = GameObject.Find("BeforeHpText").GetComponent<TextMeshProUGUI>();
            beforeAttackText = GameObject.Find("BeforeAttackText").GetComponent<TextMeshProUGUI>();
            beforeDefenseText = GameObject.Find("BeforeDefenseText").GetComponent<TextMeshProUGUI>();
            afterHpText = GameObject.Find("AfterHpText").GetComponent<TextMeshProUGUI>();
            afterAttackText = GameObject.Find("AfterAttackText").GetComponent<TextMeshProUGUI>();
            afterDefenseText = GameObject.Find("AfterDefenseText").GetComponent<TextMeshProUGUI>();
            destructionCountText = GameObject.Find("DestructionCountText").GetComponent<TextMeshProUGUI>();
            needDestructionCountText = GameObject.Find("NeedDestructionCountText").GetComponent<TextMeshProUGUI>();
            statusPointText = GameObject.Find("StatusPointText").GetComponent<TextMeshProUGUI>();
            experienceGauge = GameObject.Find("ExperienceGauge").GetComponent<Slider>();
        }

        /// <summary>バトル前のステータスをテキストに反映する</summary>
        private void ReflectionBeforeStatus()
        {
            levelText.text = beforeStatus.Level.ToString();
            beforeHpText.text = beforeStatus.HitPoint.ToString();
            beforeAttackText.text = beforeStatus.Attack.ToString();
            beforeDefenseText.text = beforeStatus.Defense.ToString();

            levelText.text = beforeStatus.Level.ToString();
            afterHpText.text = beforeStatus.HitPoint.ToString();
            afterAttackText.text = beforeStatus.Attack.ToString();
            afterDefenseText.text = beforeStatus.Defense.ToString();

            destructionCountText.text = panelCounter.TotalDestructionCount.ToString();
            statusPointText.text = magia.AllocationPoint.ToString();
        }


        /// <summary>レベルアップするときの演出</summary>
        private void LevelUpPerformance()
        {

            experienceGauge.value += addAmount;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");

            if (experienceGauge.value >= experienceGauge.maxValue)
            {
                levelUpImage.SetActive(true);
                addAmount *= 1.2f;
                experienceGauge.value = 0;
                experienceGauge.maxValue = requiredExperiences[index];
                levelText.text = levelDifference[index].ToString();
                afterHpText.text = hpDifference[index].ToString();
                afterAttackText.text = attackDifference[index].ToString();
                afterDefenseText.text = defenceDifference[index].ToString();
                index++;
                if (index == levelDifference.Count)
                {
                    experienceDifference = requiredExperiences.Last() - magia.TotalExperience;
                    experienceGauge.value = experienceDifference;
                    isAnimation = false;
                }
            }
        }

        /// <summary>レベルアップしないときの演出</summary>
        private void NotLevelUpPerformance()
        {
            experienceGauge.value += addAmount;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");

            if ((int)experienceGauge.value == afterTotalExperience)
            {
                isAnimation = false;
            }
        }

        /// <summary>バトル後のステータスをテキストに反映する(演出スキップ)</summary>
        private void ReflectionAfterStatus()
        {
            levelText.text = afterStatus.Level.ToString();
            afterHpText.text = afterStatus.HitPoint.ToString();
            afterAttackText.text = afterStatus.Attack.ToString();
            afterDefenseText.text = afterStatus.Defense.ToString();
            experienceGauge.value = experienceDifference;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");
        }
    }
}
