using System.Collections;
using UnityEngine;
using TMPro;
using DemonicCity.BattleScene;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;


namespace DemonicCity.ResultScene
{
    public class ResultManager : MonoBehaviour,IPointerDownHandler
    {
        Magia magia;
        PanelCounter panelCounter;
        Status beforeStatus;
        Status afterStatus;
        private int levelUpCount = 0;
        private float addAmount;

        List<Status> StatusDifference = new List<Status>();

        ///<summary>現在のレベルテキスト</summary>
        [SerializeField] TextMeshProUGUI levelText;
        /// <summary>現在の基礎ステータステキスト</summary>
        [SerializeField] TextMeshProUGUI[] currentBasicStatusTexts = new TextMeshProUGUI[3];
        /// <summary>変動後の基礎ステータステキスト</summary>
        [SerializeField] TextMeshProUGUI[] updatedBasicStatusTexts = new TextMeshProUGUI[3];
        /// <summary>経験値ゲージ</summary>
        [SerializeField] private Slider experienceGauge;
        /// <summary>バトルでの街破壊数テキスト</summary>
        [SerializeField] TextMeshProUGUI destructionCountText;
        /// <summary>レベルアップしたときに表示するイメージ</summary>
        [SerializeField] private GameObject levelUpImage;
        /// <summary>次のレベルアップに必要な街破壊数テキスト</summary>
        [SerializeField] TextMeshProUGUI needExpText;
        /// <summary>獲得した割り振りポイントテキスト(魔力値)</summary>
        [SerializeField] TextMeshProUGUI getStatusPointText;
        /// <summary>ストーリーに進むか選択するウィンドウ</summary>
        [SerializeField] private GameObject loadStoryWindow;
        /// <summary>描画スピード</summary>
        [SerializeField] private float drawingSpeed = 1f;


        private void Start()
        {
            magia = Magia.Instance;
            panelCounter = PanelCounter.Instance;
            beforeStatus = magia.GetStats();
            ResultCalculation();
            ShowStatusText();

            // 経験値ゲージの初期化
            experienceGauge.value = magia.TotalExperience;
            experienceGauge.maxValue = magia.GetRequiredExpToNextLevel(magia.Stats.m_level);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < levelUpCount; i++)
                {

                    while (experienceGauge.value < experienceGauge.maxValue)
                    {
                      //  experienceGauge.value += 

                    }
                    experienceGauge.value = 0;
                }
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StartCoroutine(LevelupAnimation());
        }

        IEnumerator LevelupAnimation()
        {
            for (int i = 0; i < levelUpCount; i++)
            {

                while (experienceGauge.value < experienceGauge.maxValue)
                {
                    experienceGauge.value += addAmount;

                }
                experienceGauge.value = 0;
            }
            yield return true;
        }

        private void ResultCalculation()
        {
            var totalExperience = magia.TotalExperience + panelCounter.TotalDestructionCount;
            var requiredTotalExperiences = new List<int>();
            requiredTotalExperiences.Add(magia.GetRequiredExpToNextLevel(magia.GetStats().m_level));
            StatusDifference.Add(magia.GetStats());

            // 総経験値がレベルアップに必要な経験値よりも高かった場合条件が満たさなくなる迄レベルアップ処理を行う
            while (totalExperience >= requiredTotalExperiences.Last())
            {
                magia.LevelUp();
                StatusDifference.Add(magia.GetStats());
                requiredTotalExperiences.Add(magia.GetRequiredExpToNextLevel(magia.GetStats().m_level));
                levelUpCount++;
            }
            //var diff = requiredTotalExperience - totalExperience;
            afterStatus = magia.GetStats();
        }

        /// <summary>テキスト更新</summary>
        private void ShowStatusText()
        {
            levelText.text = beforeStatus.m_level.ToString();
            destructionCountText.text = panelCounter.TotalDestructionCount.ToString();
            //  needExpText.text = diff;
            currentBasicStatusTexts[0].text = beforeStatus.m_hitPoint.ToString();
            currentBasicStatusTexts[1].text = beforeStatus.m_attack.ToString();
            currentBasicStatusTexts[2].text = beforeStatus.m_defense.ToString();
            getStatusPointText.text = magia.AllocationPoint.ToString();
        }

        private void UpdateStatusText()
        {
            updatedBasicStatusTexts[0].text = afterStatus.m_level.ToString();
            updatedBasicStatusTexts[1].text = afterStatus.ToString();
            updatedBasicStatusTexts[2].text = afterStatus.ToString();

            // needExpText.text = diff;
        }


    }
}
