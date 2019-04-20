using System.Collections;
using UnityEngine;
using TMPro;
using DemonicCity.BattleScene;
using DemonicCity.StrengthenScene;
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

        /// <summary>バトル前のちゃんマギのステータス</summary>
        Status beforeStatus;

        /// <summary>バトル後のちゃんマギのステータス</summary>
        Status afterStatus;

        /// <summary></summary>
        LevelTextAnimation levelTextAnimation;

        /// <summary>ちゃんマギの最大レベル</summary>
        private const int maxLevel = 200;

        /// <summary>経験値ゲージの枠画像</summary>
        [SerializeField] private Sprite defaultGaugeSprite = null;

        /// <summary>レベルアップしたときの経験値ゲージの枠画像</summary>
        [SerializeField] private Sprite levelUpGaugeSprite = null;

        List<int> levelDifference = new List<int>();
        List<int> hpDifference = new List<int>();
        List<int> attackDifference = new List<int>();
        List<int> defenceDifference = new List<int>();
        List<int> statusPointDifferences = new List<int>();

        /// <summary>獲得した総魔力値</summary>
        private int getStatusPoint = 0;

        /// <summary>各レベルで必要な総経験値</summary>
        List<int> requiredExperiences = new List<int>();
        List<int> experienceDifferences = new List<int>();

        /// <summary>レベルアップによって習得したスキル</summary>
        List<string> masterdSkillNames = new List<string>();

        /// <summary>画面をタップした回数</summary>
        private int tapCount = 0;

        /// <summary>レベルアップの画像</summary>
        [SerializeField] private GameObject levelUpImage = null;

        /// <summary>最大レベルの画像</summary>
        [SerializeField] private GameObject levelMaxImage = null;


        /// <summary>スキル習得通知ウィンドウ</summary>
        [SerializeField] private GameObject skillMasterdMessageWindow = null;
        private GameObject[] masterdSkillNameText = null;

        /// <summary>各スキルを習得するレベル</summary>
        private int[] skillMasterdLevelList = { 1, 11, 23, 31, 44, 58, 70, 82, 100, 111, 136, 160, 181, 198 };

        /// <summary>スキル名</summary>
        private string skillName;

        private TextMeshProUGUI currentLevelText = null;
        private TextMeshProUGUI nextLevelText = null;
        private GameObject beforeHpText = null;
        private GameObject beforeAttackText = null;
        private GameObject beforeDefenceText = null;
        private TextMeshProUGUI afterHpText = null;
        private TextMeshProUGUI afterAttackText = null;
        private TextMeshProUGUI afterDefenseText = null;

        private TextMeshProUGUI destructionCountText = null;
        private TextMeshProUGUI needDestructionCountText = null;
        private TextMeshProUGUI statusPointText = null;

        private GameObject[] rightArrows = null;
        [SerializeField] private GameObject toNextStoryPrefab = null;

        private Slider experienceGauge = null;
        private Image gaugeBackGround = null;


        private float addAmount = 0;
        private bool isAnimation = false;
        private int index = 0;
        private int destructionCount;
        private bool isLevelUp = false;

        int totalExperience = 0;
        int myExperience = 0;

        [SerializeField] private float gaugeMoveSpeed = 0.05f;

        /// <summary>現在のレベルからレベルアップするまでに必要とされる総経験値</summary>
        int requiredToNextLevelTotalExperience;

        //private Strengthen_Part1 strengthen;

        private void Awake()
        {
            magia = Magia.Instance;
            panelCounter = PanelCounter.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }

        private void Start()
        {
            SavableSingletonBase<Magia>.Instance.Clear();//debug

            GetGameObjects();

            ReflectionBeforeStatus();
            if (beforeStatus.Level < maxLevel)
            {
                ResultCalculation();
                afterStatus = magia.Stats;
            }

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    tapCount++;

                    if (tapCount == 1)
                    {
                        isAnimation = true;
                    }
                    else if (tapCount == 2 && isAnimation)//演出スキップ
                    {
                        ReflectionAfterStatus();
                        nextLevelText.text = "";
                        isAnimation = false;
                    }
                    else if ((tapCount == 3 && !isAnimation) || (tapCount == 2 && isAnimation))//スキップした場合||スキップしなかった場合
                    {
                        gaugeBackGround.sprite = defaultGaugeSprite;

                        if (isLevelUp)
                        {
                            //レベルアップした際のレベルとスキルを習得するレベルを照合
                            for (int i = 0; i < levelDifference.Count; i++)
                            {
                                for (int a = 0; a < skillMasterdLevelList.Length; a++)
                                {
                                    if (levelDifference[i] == skillMasterdLevelList[a])//一致した場合、そのレベルに応じたスキルを習得したと判定する
                                    {
                                        masterdSkillNames.Add(GetSkillName(levelDifference[i]));
                                    }
                                }
                            }

                            if (masterdSkillNames.Count > 0)//スキルを1つ以上習得した場合
                            {
                                StartCoroutine(PopUp_SkillMasterdMessageWindow());
                            }
                        }

                        if (beforeStatus.Level >= maxLevel)
                        {
                            magia.Stats.Level = maxLevel;
                            experienceGauge.value = experienceGauge.maxValue;
                            needDestructionCountText.text = 0.ToString();
                            //  levelUpImage.SetActive(false);
                            StartCoroutine(ClosePopUpAnimation(levelUpImage));
                            //  maxLevelImage.SetActive(true);
                        }
                    }
                    else if (tapCount == 4 || tapCount == 3 || tapCount == 2)
                    {
                        SavableSingletonBase<Magia>.Instance.Save();
                        Instantiate(toNextStoryPrefab, transform);
                    }
                }
            });
        }

        /// <summary>レベルに対応したスキル名を返す</summary>
        private string GetSkillName(int level)
        {
            switch (level)
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
                case 31:
                    skillName = "爆炎熱風柱";
                    magia.MyPassiveSkill |= Magia.PassiveSkill.ExplosiveFlamePillar;

                    break;
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
            Debug.Log(magia.MyPassiveSkill);
            return skillName;
        }

        private void Update()
        {
            if (isAnimation && isLevelUp && destructionCount > 0)
            {
                LevelUpPerformance();
            }
            else if (isAnimation && !isLevelUp && destructionCount >= 0)
            {
                NotLevelUpPerformance();
            }
        }

        private IEnumerator PopUp_SkillMasterdMessageWindow()
        {
            skillMasterdMessageWindow.SetActive(true);

            for (int i = 0; i < masterdSkillNames.Count; i++)
            {
                masterdSkillNameText[i].SetActive(true);
                masterdSkillNameText[i].GetComponent<TextMeshProUGUI>().text = masterdSkillNames[i];//習得したスキルを全て表示
            }
            yield return new WaitForSeconds(5f);
            StartCoroutine(ClosePopUpAnimation(skillMasterdMessageWindow));
        }

        /// <summary>ウィンドウが閉じるときのアニメーション処理</summary>
        private IEnumerator ClosePopUpAnimation(GameObject window)
        {
            window.GetComponent<Animator>().CrossFadeInFixedTime(Strengthen_Part1.PopUpAnimation.Close_PopUpWindow.ToString(), 0);
            yield return new WaitForSeconds(0.5f);
            window.SetActive(false);
        }

        /// <summary>経験値を計算</summary>
        private void ResultCalculation()
        {
            totalExperience = myExperience + destructionCount;

            if (requiredToNextLevelTotalExperience <= totalExperience)
            {
                isLevelUp = true;
                int getTotalStatusPoint = 0;

                // 総経験値がレベルアップに必要な経験値よりも高かった場合条件が満たさなくなる迄レベルアップ処理を行う
                while (totalExperience >= requiredToNextLevelTotalExperience && requiredToNextLevelTotalExperience > 0 && isLevelUp && magia.Stats.Level < maxLevel)
                {
                    totalExperience -= requiredToNextLevelTotalExperience;

                    magia.LevelUp(out getStatusPoint);
                    levelDifference.Add(magia.Stats.Level);
                    hpDifference.Add(magia.Stats.HitPoint);
                    attackDifference.Add(magia.Stats.Attack);
                    defenceDifference.Add(magia.Stats.Defense);
                    getTotalStatusPoint += getStatusPoint;
                    statusPointDifferences.Add(getTotalStatusPoint);
                    requiredToNextLevelTotalExperience = magia.Stats.Level + 5;
                    requiredExperiences.Add(requiredToNextLevelTotalExperience);
                }
                magia.MyExperience = totalExperience;
            }
        }

        /// <summary>レベルアップするときの演出</summary>
        private void LevelUpPerformance()
        {
            addAmount = gaugeMoveSpeed * levelDifference[0];
            experienceGauge.value += addAmount;
            needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");

            if (experienceGauge.value >= experienceGauge.maxValue)
            {
                gaugeBackGround.sprite = levelUpGaugeSprite;


                experienceGauge.value = 0;
                experienceGauge.maxValue = requiredExperiences[index];

                levelUpImage.SetActive(true);

                currentLevelText.text = "";
                nextLevelText.text = levelDifference[index].ToString() + "\n" + (levelDifference[index] - 1).ToString();
                levelTextAnimation.TextAnimation(LevelTextAnimation.AnimationClip.TopToBottom);

                beforeHpText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f, -197, 0);
                beforeAttackText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f, -297, 0);
                beforeDefenceText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f, -397, 0);

                afterHpText.text = hpDifference[index].ToString();
                afterAttackText.text = attackDifference[index].ToString();
                afterDefenseText.text = defenceDifference[index].ToString();
                statusPointText.text = statusPointDifferences[index].ToString();

                for (int i = 0; i < rightArrows.Length; i++)
                {
                    rightArrows[i].SetActive(true);
                }

                addAmount = gaugeMoveSpeed * levelDifference[index];

                if (levelDifference[index] >= maxLevel)//レベルアップ中に最大レベルに達したら
                {
                    magia.Stats.Level = maxLevel;
                    experienceGauge.value = experienceGauge.maxValue;
                    needDestructionCountText.text = 0.ToString();
                    //levelUpImage.SetActive(false);
                    StartCoroutine(ClosePopUpAnimation(levelUpImage));
                    levelMaxImage.SetActive(true);
                    isAnimation = false;
                    gaugeBackGround.sprite = defaultGaugeSprite;
                }

                index++;

                if (index == levelDifference.Count)
                {
                    StartCoroutine(GaugeAnimation());
                    gaugeBackGround.sprite = defaultGaugeSprite;

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
                yield return new WaitForSeconds(0.02f);
            }
            experienceGauge.value = myExperience;
            needDestructionCountText.text = (requiredExperiences.Last() - myExperience).ToString();
            isAnimation = false;
        }

        /// <summary>レベルアップしないときの演出</summary>
        private void NotLevelUpPerformance()
        {
            addAmount = gaugeMoveSpeed * magia.Stats.Level;
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
            Debug.Log("Called");
            currentLevelText.text = beforeStatus.Level.ToString();
            beforeHpText.GetComponent<TextMeshProUGUI>().text = beforeStatus.HitPoint.ToString();
            beforeAttackText.GetComponent<TextMeshProUGUI>().text = beforeStatus.Attack.ToString();
            beforeDefenceText.GetComponent<TextMeshProUGUI>().text = beforeStatus.Defense.ToString();

            afterHpText.text = "";
            afterAttackText.text = "";
            afterDefenseText.text = "";

            destructionCount = panelCounter.TotalDestructionCount;
           // destructionCount = 150;//debug
            destructionCountText.text = destructionCount.ToString();

            statusPointText.text = 0.ToString();

            requiredToNextLevelTotalExperience = beforeStatus.Level + 5;
            experienceGauge.maxValue = requiredToNextLevelTotalExperience;
            myExperience = magia.MyExperience;
            experienceGauge.value = myExperience;

            needDestructionCountText.text = (requiredToNextLevelTotalExperience - myExperience).ToString();

            if (beforeStatus.Level == maxLevel)
            {
                needDestructionCountText.text = 0.ToString();
                levelMaxImage.SetActive(true);
                experienceGauge.value = experienceGauge.maxValue;
            }
        }

        /// <summary>バトル後のステータスをテキストに反映する(演出スキップ)</summary>
        private void ReflectionAfterStatus()
        {
            if (isLevelUp)
            {
                levelUpImage.SetActive(true);
                beforeHpText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f, -197, 0);
                beforeAttackText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f, -297, 0);
                beforeDefenceText.GetComponent<RectTransform>().localPosition = new Vector3(-82.5f, -397, 0);

                for (int i = 0; i < rightArrows.Length; i++)
                {
                    rightArrows[i].SetActive(true);
                }

                currentLevelText.text = afterStatus.Level.ToString();
                afterHpText.text = afterStatus.HitPoint.ToString();
                afterAttackText.text = afterStatus.Attack.ToString();
                afterDefenseText.text = afterStatus.Defense.ToString();
            }


            if (destructionCount > 0)
            {
                experienceGauge.value = myExperience;
                experienceGauge.maxValue = requiredExperiences.Last();
                needDestructionCountText.text = (requiredExperiences.Last() - myExperience).ToString();
                statusPointText.text = statusPointDifferences.Last().ToString();
            }

            if (afterStatus.Level >= maxLevel)
            {
                magia.Stats.Level = maxLevel;
                currentLevelText.text = maxLevel.ToString();
                experienceGauge.value = experienceGauge.maxValue;
                needDestructionCountText.text = 0.ToString();
                //levelUpImage.SetActive(false);
                StartCoroutine(ClosePopUpAnimation(levelUpImage));
                levelMaxImage.SetActive(true);
            }
        }

        /// <summary>シーン上にあるゲームオブジェクトを取得</summary>
        private void GetGameObjects()
        {
            levelUpImage.SetActive(false);
            levelMaxImage.SetActive(false);

            rightArrows = GameObject.FindGameObjectsWithTag("RightArrow");
            for (int i = 0; i < rightArrows.Length; i++)
            {
                rightArrows[i].SetActive(false);
            }
            currentLevelText = GameObject.Find("CurrentLevelText").GetComponent<TextMeshProUGUI>();
            nextLevelText = GameObject.Find("NextLevelText").GetComponent<TextMeshProUGUI>();
            nextLevelText.text = "";
            beforeHpText = GameObject.Find("BeforeHpText");
            beforeAttackText = GameObject.Find("BeforeAttackText");
            beforeDefenceText = GameObject.Find("BeforeDefenseText");
            afterHpText = GameObject.Find("AfterHpText").GetComponent<TextMeshProUGUI>();
            afterAttackText = GameObject.Find("AfterAttackText").GetComponent<TextMeshProUGUI>();
            afterDefenseText = GameObject.Find("AfterDefenseText").GetComponent<TextMeshProUGUI>();
            destructionCountText = GameObject.Find("DestructionCountText").GetComponent<TextMeshProUGUI>();
            needDestructionCountText = GameObject.Find("NeedDestructionCountText").GetComponent<TextMeshProUGUI>();
            statusPointText = GameObject.Find("StatusPointText").GetComponent<TextMeshProUGUI>();
            experienceGauge = GameObject.Find("ExperienceGauge").GetComponent<Slider>();
            levelTextAnimation = FindObjectOfType<LevelTextAnimation>();
            masterdSkillNameText = GameObject.FindGameObjectsWithTag("MasterdSkillName");
            for (int i = 0; i < masterdSkillNameText.Length; i++)
            {
                masterdSkillNameText[i].SetActive(false);
            }
            skillMasterdMessageWindow.SetActive(false);

            gaugeBackGround = GameObject.Find("GaugeBackGround").GetComponent<Image>();

        }

    }
}
