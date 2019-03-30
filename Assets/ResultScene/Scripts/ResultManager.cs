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

        List<int> levelDifference = new List<int>();
        List<int> hpDifference = new List<int>();
        List<int> attackDifference = new List<int>();
        List<int> defenceDifference = new List<int>();
        List<int> statusPointDifferences = new List<int>();

        /// <summary>各レベルで必要な総経験値</summary>
        List<int> requiredExperiences = new List<int>();
        List<int> experienceDifferences = new List<int>();
        private int tapCount = 0;
        private GameObject levelUpImage = null;
       
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
        int index = 0;
        private int destructionCount;
        private bool isLevelUp = false;
        int totalExperience;
        int MyExperience;
        int statusPoint;
        /// <summary>現在のレベルで必要とされる総経験値</summary>
        int nextLevelRequiredTotalExperience;
        /// <summary>ポップアップが表示されたかどうか</summary>
        private bool isPopup = false;
        private bool isSlot = true;
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
            GetGameObject();
           
            ReflectionBeforeStatus();
            ResultCalculation();
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
                        isAnimation = false;
                    }
                    else if (tapCount == 3 || !isAnimation)
                    {
                        SavableSingletonBase<Magia>.Instance.Save();
                        if (!isPopup)
                        {
                            Instantiate(toNextStoryPrefab, transform);
                            isPopup = true;
                        }
                    }
                }
            });
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

        /// <summary>経験値を計算</summary>
        void ResultCalculation()
        {

            totalExperience = MyExperience + destructionCount;

            if (nextLevelRequiredTotalExperience <= totalExperience)
            {
                isLevelUp = true;
            }
            requiredExperiences.Add(nextLevelRequiredTotalExperience);
            statusPointDifferences.Add(statusPoint);
            if (isLevelUp)
            {
                // 総経験値がレベルアップに必要な経験値よりも高かった場合条件が満たさなくなる迄レベルアップ処理を行う
                while (totalExperience >= nextLevelRequiredTotalExperience)
                {
                    totalExperience -= nextLevelRequiredTotalExperience;

                    magia.LevelUp();

                    levelDifference.Add(magia.Stats.Level);

                    
                    hpDifference.Add(magia.Stats.HitPoint);
                    attackDifference.Add(magia.Stats.Attack);
                    defenceDifference.Add(magia.Stats.Defense);
                    statusPointDifferences.Add(magia.AllocationPoint);

                    nextLevelRequiredTotalExperience = magia.GetRequiredExpToNextLevel(magia.Stats.Level);
                    requiredExperiences.Add(nextLevelRequiredTotalExperience);
                }
                magia.MyExperience = totalExperience;
                MyExperience = magia.MyExperience;
            }
        }



        /// <summary>レベルアップするときの演出</summary>
        private void LevelUpPerformance()
        {
            addAmount = 0.1f * levelDifference[0];
            experienceGauge.value += addAmount;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");
    
            if (experienceGauge.value >= experienceGauge.maxValue)
            {
                GameObject levelUp = levelUpImage;
                levelUp.SetActive(true);
               
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
                    Destroy(levelUp, 2);
                }
            }
        }
        /// <summary>レベルアップしたあとの残りの経験値を足すアニメーション</summary>
        /// <returns></returns>
        IEnumerator GaugeAnimation()
        {
            experienceGauge.maxValue = requiredExperiences.Last();

            while ((int)experienceGauge.value < MyExperience)
            {
                experienceGauge.value += addAmount;
                needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");
                yield return new WaitForSeconds(0.01f);
            }
            experienceGauge.value = MyExperience;
            needDestructionCountText.text = (requiredExperiences.Last() - MyExperience).ToString();
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

            nextLevelRequiredTotalExperience = magia.GetRequiredExpToNextLevel(magia.Stats.Level);
            experienceGauge.maxValue = nextLevelRequiredTotalExperience;
            MyExperience = magia.MyExperience;
            experienceGauge.value = MyExperience;

            needDestructionCountText.text = (nextLevelRequiredTotalExperience - MyExperience).ToString();
            destructionCount = panelCounter.TotalDestructionCount;
            destructionCount = 600;//debug
            //destructionCountText.text = destructionCount.ToString();
        }

        /// <summary>バトル後のステータスをテキストに反映する(演出スキップ)</summary>
        private void ReflectionAfterStatus()
        {
            currentLevelText.text = afterStatus.Level.ToString();
            afterHpText.text = afterStatus.HitPoint.ToString();
            afterAttackText.text = afterStatus.Attack.ToString();
            afterDefenseText.text = afterStatus.Defense.ToString();
            experienceGauge.value = MyExperience;
            experienceGauge.maxValue = requiredExperiences.Last();
            needDestructionCountText.text = (requiredExperiences.Last() - MyExperience).ToString();
            statusPointText.text = statusPointDifferences.Last().ToString();
        }

        /// <summary>シーン上にあるゲームオブジェクトを取得</summary>
        private void GetGameObject()
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
        }

    }
}
