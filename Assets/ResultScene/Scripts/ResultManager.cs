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
        LevelTextAnimation levelTextAnimation;
        private const int maxLevel = 200;

        List<int> levelDifference = new List<int>();
        List<int> hpDifference = new List<int>();
        List<int> attackDifference = new List<int>();
        List<int> defenceDifference = new List<int>();
        List<int> statusPointDifferences = new List<int>();

        /// <summary>各レベルで必要な総経験値</summary>
        List<int> requiredExperiences = new List<int>();
        List<int> experienceDifferences = new List<int>();

        /// <summary>レベルアップによって習得したスキルリスト</summary>
        List<string> masterdSkillNames = new List<string>();

        private int tapCount = 0;
        private GameObject levelUpImage = null;
        private GameObject skillMasterdMessageWindow = null;
        private GameObject[] masterdSkillNameObj = null;
        private int[] skillMasterdLevels = { 1, 11, 23, 31, 44, 58, 70, 82, 100, 111, 136, 160, 181, 198 };
        private string skillName;

        private TextMeshProUGUI currentLevelText = null;
        private TextMeshProUGUI nextLevelText = null;
        private TextMeshProUGUI beforeHpText = null;
        private TextMeshProUGUI beforeAttackText = null;
        private TextMeshProUGUI beforeDefenseText = null;
        private TextMeshProUGUI afterHpText = null;
        private TextMeshProUGUI afterAttackText = null;
        private TextMeshProUGUI afterDefenseText = null;

        private TextMeshProUGUI destructionCountText = null;
        private TextMeshProUGUI needDestructionCountText = null;
        private TextMeshProUGUI statusPointText = null;

        private GameObject[] rightArrows = null;
        [SerializeField] private GameObject toNextStoryPrefab = null;

        private Slider experienceGauge = null;
        private float addAmount = 0.1f;
        private bool isAnimation = false;
        private int index = 0;
        private int destructionCount;
        private bool isLevelUp = false;
        int totalExperience;
        int myExperience;
        int statusPoint;
        /// <summary>現在のレベルで必要とされる総経験値</summary>
        int nextLevelRequiredExperience;
        /// <summary>ポップアップが表示されたかどうか</summary>
        private bool isPopup = false;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            magia = Magia.Instance;
            panelCounter = PanelCounter.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }

        private void Start()
        {
            FindGameObjects();

            ReflectionBeforeStatus();
            if (beforeStatus.Level < maxLevel)
            {
                ResultCalculation();
            }
            afterStatus = magia.Stats;

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    tapCount++;

                    if (tapCount == 1)
                    {
                        isAnimation = true;
                    }
                    else if (tapCount == 2 && isAnimation)
                    {
                        ReflectionAfterStatus();
                        nextLevelText.text = "";
                        isAnimation = false;
                    }
                    else if (tapCount == 3 || !isAnimation)
                    {
                        SavableSingletonBase<Magia>.Instance.Save();
                        if (masterdSkillNames.Count > 0)
                        {
                            StartCoroutine(PopUpSkillMasterdMessageWindow());
                            isPopup = true;
                        }
                        tapCount++;
                    }
                    else if(tapCount == 4 || isPopup)
                    {
                        if (!isPopup)
                        {
                            Instantiate(toNextStoryPrefab, transform);
                            isPopup = true;
                        }
                    }
                }
            });
        }

        /// <summary>レベルアップ時に習得したスキルを表示する</summary>
        private string GetSkillName(int level)
        {

            switch (level)
            {
                case 1:
                    //passiveSkill = Magia.PassiveSkill.DevilsFist;
                    //skillContents[0].SetActive(true);
                    //skillContents[0].GetComponent<Text>().text = Magia.PassiveSkill.DevilsFist.ToString();
                    skillName = "魔拳";
                    break;
                case 11:
                    skillName = "高濃度魔力吸収";
                    break;
                case 23:
                    skillName = "自己再生";
                    break;
                case 31:
                    skillName = "爆炎熱風柱";
                    break;
                case 44:
                    skillName = "紅蓮障壁";
                    break;
                case 58:
                    skillName = "魔拳烈火ノ型";
                    break;
                case 70:
                    skillName = "心焔権現";
                    break;
                case 82:
                    skillName = "大紅蓮障壁";
                    break;
                case 100:
                    skillName = "豪炎爆砕掌";
                    break;
                case 111:
                    skillName = "魔王ノ細胞";
                    break;
                case 136:
                    skillName = "天照権現";
                    break;
                case 160:
                    skillName = "天照-爆炎-";
                    break;
                case 181:
                    skillName = "天照-焔壁-";
                    break;
                case 198:
                    skillName = "王ノ器";
                    break;

            }
            return skillName;
        }

        private void Update()
        {
            if (isAnimation && isLevelUp && destructionCount > 0)
            {
                LevelUpPerformance();
            }
            else if (isAnimation && !isLevelUp && destructionCount > 0)
            {
                NotLevelUpPerformance();
            }
        }

        private IEnumerator PopUpSkillMasterdMessageWindow()
        {
            for (int i = 0; i < levelDifference.Count; i++)
            {
                for (int a = 0; a < skillMasterdLevels.Length; a++)
                {
                    if (levelDifference[i] == skillMasterdLevels[a])
                    {
                        //そのレベルに応じたスキルをListに追加
                        masterdSkillNames.Add(GetSkillName(levelDifference[i]));
                        Debug.Log("Lv" + levelDifference[i] + "で" + GetSkillName(levelDifference[i]) + "を習得");
                    }
                }
            }

            yield return new WaitForSeconds(1f);
            skillMasterdMessageWindow.SetActive(true);

            for (int i = 0; i < masterdSkillNames.Count; i++)
            {
                masterdSkillNameObj[i].SetActive(true);
                masterdSkillNameObj[i].GetComponent<TextMeshProUGUI>().text = masterdSkillNames[i];
            }
            yield return new WaitForSeconds(5f);
            skillMasterdMessageWindow.SetActive(false);
        }

        /// <summary>経験値を計算</summary>
        void ResultCalculation()
        {

            totalExperience = myExperience + destructionCount;

            if (nextLevelRequiredExperience <= totalExperience)
            {
                isLevelUp = true;
                // 総経験値がレベルアップに必要な経験値よりも高かった場合条件が満たさなくなる迄レベルアップ処理を行う
                while (totalExperience >= nextLevelRequiredExperience && nextLevelRequiredExperience > 0 && isLevelUp)
                {
                    totalExperience -= nextLevelRequiredExperience;

                    magia.LevelUp();

                    levelDifference.Add(magia.Stats.Level);
                    hpDifference.Add(magia.Stats.HitPoint);
                    attackDifference.Add(magia.Stats.Attack);
                    defenceDifference.Add(magia.Stats.Defense);
                    statusPointDifferences.Add(magia.AllocationPoint);

                    nextLevelRequiredExperience = magia.GetRequiredExpToNextLevel(magia.Stats.Level);
                    requiredExperiences.Add(nextLevelRequiredExperience);
                }
                magia.MyExperience = totalExperience;
                myExperience = magia.MyExperience;

            }
            requiredExperiences.Add(nextLevelRequiredExperience);
            statusPointDifferences.Add(statusPoint);
        }



        /// <summary>レベルアップするときの演出</summary>
        private void LevelUpPerformance()
        {
            addAmount = 0.1f * levelDifference[0];
            experienceGauge.value += addAmount;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");

            if (experienceGauge.value >= experienceGauge.maxValue)
            {
                GameObject levelUpObj = levelUpImage;
                levelUpObj.SetActive(true);

                experienceGauge.value = 0;
                experienceGauge.maxValue = requiredExperiences[index];
                currentLevelText.text = "";

                nextLevelText.text = levelDifference[index].ToString();
                levelTextAnimation.TopToBottomAnimation(LevelTextAnimation.AnimationClip.TopToBottom);

                afterHpText.text = hpDifference[index].ToString();
                afterAttackText.text = attackDifference[index].ToString();
                afterDefenseText.text = defenceDifference[index].ToString();
                statusPointText.text = statusPointDifferences[index].ToString();

                for (int i = 0; i < rightArrows.Length; i++)
                {
                    rightArrows[i].SetActive(true);
                }

                addAmount = 0.1f * levelDifference[index];

                index++;

                if (index == levelDifference.Count)
                {
                    StartCoroutine(GaugeAnimation());
                    levelUpObj.SetActive(false);
                }
            }
        }
        /// <summary>レベルアップしたあとの残りの経験値を足すアニメーション</summary>
        /// <returns></returns>
        IEnumerator GaugeAnimation()
        {
            experienceGauge.maxValue = requiredExperiences.Last();

            while ((int)experienceGauge.value < myExperience)
            {
                experienceGauge.value += addAmount;
                needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");
                yield return new WaitForSeconds(0.01f);
            }
            experienceGauge.value = myExperience;
            needDestructionCountText.text = (requiredExperiences.Last() - myExperience).ToString();
            isAnimation = false;
        }

        /// <summary>レベルアップしないときの演出</summary>
        private void NotLevelUpPerformance()
        {

            addAmount = 0.1f * magia.Stats.Level;
            experienceGauge.value += addAmount;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");

            if ((int)experienceGauge.value == totalExperience)
            {
                isAnimation = false;
            }
        }

        /// <summary>バトル前のステータスをテキストに反映する</summary>
        private void ReflectionBeforeStatus()
        {
            beforeStatus = magia.Stats;

            currentLevelText.text = beforeStatus.Level.ToString();
            beforeHpText.text = beforeStatus.HitPoint.ToString();
            beforeAttackText.text = beforeStatus.Attack.ToString();
            beforeDefenseText.text = beforeStatus.Defense.ToString();

            afterHpText.text = "";
            afterAttackText.text = "";
            afterDefenseText.text = "";

            statusPoint = magia.AllocationPoint;
            statusPointText.text = statusPoint.ToString();

            nextLevelRequiredExperience = magia.GetRequiredExpToNextLevel(magia.Stats.Level);
            experienceGauge.maxValue = nextLevelRequiredExperience;
            myExperience = magia.MyExperience;
            experienceGauge.value = myExperience;

            needDestructionCountText.text = (nextLevelRequiredExperience - myExperience).ToString();
            destructionCount = panelCounter.TotalDestructionCount;
            destructionCount = 5000;//debug
            destructionCountText.text = destructionCount.ToString();
        }

        /// <summary>バトル後のステータスをテキストに反映する(演出スキップ)</summary>
        private void ReflectionAfterStatus()
        {
            currentLevelText.text = afterStatus.Level.ToString();
            afterHpText.text = afterStatus.HitPoint.ToString();
            afterAttackText.text = afterStatus.Attack.ToString();
            afterDefenseText.text = afterStatus.Defense.ToString();
            experienceGauge.value = myExperience;
            experienceGauge.maxValue = requiredExperiences.Last();
            needDestructionCountText.text = (requiredExperiences.Last() - myExperience).ToString();
            statusPointText.text = statusPointDifferences.Last().ToString();
        }

        /// <summary>シーン上にあるゲームオブジェクトを取得</summary>
        private void FindGameObjects()
        {
            levelUpImage = GameObject.Find("LevelUpImage");
            levelUpImage.SetActive(false);
            rightArrows = GameObject.FindGameObjectsWithTag("RightArrow");
            for (int i = 0; i < rightArrows.Length; i++)
            {
                rightArrows[i].SetActive(false);
            }
            currentLevelText = GameObject.Find("CurrentLevelText").GetComponent<TextMeshProUGUI>();
            nextLevelText = GameObject.Find("NextLevelText").GetComponent<TextMeshProUGUI>();
            nextLevelText.text = "";
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
            levelTextAnimation = FindObjectOfType<LevelTextAnimation>();
            skillMasterdMessageWindow = GameObject.Find("SkillMasterdMessageWindow");
            masterdSkillNameObj = GameObject.FindGameObjectsWithTag("MasterdSkillName");
            for (int i = 0; i < masterdSkillNameObj.Length; i++)
            {
                masterdSkillNameObj[i].GetComponent<TextMeshProUGUI>().text = "";
            }
            skillMasterdMessageWindow.SetActive(false);

        }

    }
}
