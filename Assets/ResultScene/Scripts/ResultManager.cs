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
        List<Status> StatusDifference = new List<Status>();
        List<int> requiredExperience = new List<int>();

        private int tapCount = 0;
        private int experienceDifference;

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
        int addAmount = 1;
        private bool isAnimation = false;

        private void Awake()
        {
            magia = Magia.Instance;
            panelCounter = PanelCounter.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }
        private void Start()
        {
            GetGameObject();
            needDestructionCountText.text = "";
            beforeStatus = magia.Stats;

            ReflectionBeforeStatus();


            ResultCalclation();
            afterStatus = magia.Stats;

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    tapCount += 1;

                    if (tapCount == 1)
                    {
                        isAnimation = true;
                        LevelUpPerformance();
                    }
                    else if (tapCount == 2)
                    {
                        isAnimation = false;
                        ReflectionAfterStatus();
                    }
                    else if (tapCount == 3)
                    {
                        SceneChanger.SceneChange(SceneName.Story);
                    }
                }
            });
        }

        private void Update()
        {
            if (isAnimation)
            {
                experienceGauge.value += 0.01f;
                needDestructionCountText.text = (experienceGauge.maxValue - experienceGauge.value).ToString("f0");
            }
        }

        void ResultCalclation()
        {
            //var totalExperience = magia.TotalExperience + panelCounter.TotalDestructionCount;
            int totalExperience = 50;//debug

            int requiredTotalExperience = magia.GetRequiredExpToNextLevel(magia.Stats.Level);


            // 総経験値がレベルアップに必要な経験値よりも高かった場合条件が満たさなくなる迄レベルアップ処理を行う
            while (totalExperience >= requiredTotalExperience)
            {
                magia.LevelUp();
                StatusDifference.Add(magia.Stats);
                requiredTotalExperience = magia.GetRequiredExpToNextLevel(magia.Stats.Level);
                requiredExperience.Add(requiredTotalExperience);
            }
            experienceDifference = requiredTotalExperience - totalExperience;
        }

        /// <summary>シーン上にあるオブジェクトを取得</summary>
        private void GetGameObject()
        {
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
            beforeDefenseText.text = beforeStatus.HitPoint.ToString();

            levelText.text = beforeStatus.Level.ToString();
            afterHpText.text = beforeStatus.HitPoint.ToString();
            afterAttackText.text = beforeStatus.Attack.ToString();
            afterDefenseText.text = beforeStatus.HitPoint.ToString();

            destructionCountText.text = panelCounter.TotalDestructionCount.ToString();
            statusPointText.text = magia.AllocationPoint.ToString();
        }


        /// <summary>レベルアップ演出</summary>
        private void LevelUpPerformance()
        {
            //レベルアップした回数分処理を行う
            for (int i = 0; i < StatusDifference.Count; i++)
            {
                Debug.Log(StatusDifference.Count);
                int currentRequiredExp = requiredExperience[i];
                //Debug.Log(currentRequiredExp);
                experienceGauge.maxValue = currentRequiredExp;
                isAnimation = true;
                //StartCoroutine(MoveGauge(currentRequiredExp));
                //StopAllCoroutines();
                levelText.text = StatusDifference[i].Level.ToString();
                afterHpText.text = StatusDifference[i].HitPoint.ToString();
                afterAttackText.text = StatusDifference[i].Attack.ToString();
                afterDefenseText.text = StatusDifference[i].Defense.ToString();
                isAnimation = false;
            }
        }


        IEnumerator MoveGauge(int requiredExp)
        {
            while (requiredExp > experienceGauge.value)
            {
                experienceGauge.value += addAmount;

                yield return new WaitForSeconds(0.5f);

                if (requiredExp == experienceGauge.value)
                {
                    experienceGauge.value = 0;

                    yield break;
                }
            }
        }

        /// <summary>バトル後のステータスをテキストに反映する(演出スキップ)</summary>
        private void ReflectionAfterStatus()
        {
            levelText.text = afterStatus.Level.ToString();
            afterHpText.text = afterStatus.HitPoint.ToString();
            afterAttackText.text = afterStatus.Attack.ToString();
            afterDefenseText.text = afterStatus.HitPoint.ToString();

            needDestructionCountText.text = experienceDifference.ToString();
        }
    }
}
