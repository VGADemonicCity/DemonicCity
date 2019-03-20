using System.Collections;
using UnityEngine;
using TMPro;
using DemonicCity.BattleScene;
using UnityEngine.UI;


namespace DemonicCity.ResultScene
{
    public class ShowStatus : MonoBehaviour
    {
        /// <summary>Magiaクラスのインスタンス</summary>
        Magia magia;

        /// <summary>TouchGestureDetectorクラスのインスタンス</summary>
        TouchGestureDetector touchGestureDetector;

        /// <summary>PanelCounterクラスのインスタンス</summary>
        PanelCounter panelCounter;

        /// <summary>バトル開始前のマギアのステータス</summary>
        private Status beforeStatus;

        /// <summary>変動後の体力</summary>
        private int updatedHitPoint;

        /// <summary>変動後の攻撃力</summary>
        private int updatedAttack;


        /// <summary>変動後の防御力</summary>
        private int updatedDefense;

        /// <summary>割り振りポイント(魔力値)</summary>
        private int statusPoint;

        /// <summary>獲得した割り振りポイント(魔力値)</summary>
        private int getStatusPoint;

        /// <summary>1度のバトルでの街破壊数</summary>
        private int destructionCount;

        /// <summary>次のレベルアップに必要な総経験値</summary>
        private float requiredExp;

        /// <summary>レベルアップに必要な残り経験値</summary>
        private float needExp;

        /// <summary>現在の経験値(累計街破壊数)</summary>
        private float currentExp;

        /// <summary>変動後の経験値</summary>
        private float afterExp;

        /// <summary>現在のレベルテキスト</summary>
        [SerializeField] TextMeshProUGUI levelText;

        /// <summary>現在の基礎ステータステキスト</summary>
        [SerializeField] TextMeshProUGUI[] currentBasicStatusTexts = new TextMeshProUGUI[3];

        /// <summary>変動後の基礎ステータステキスト</summary>
        [SerializeField] TextMeshProUGUI[] updatedBasicStatusTexts = new TextMeshProUGUI[3];

        /// <summary>獲得した割り振りポイントテキスト(魔力値)</summary>
        [SerializeField] TextMeshProUGUI getStatusPointText;

        /// <summary>レベルアップしたときに表示する</summary>
        [SerializeField] private GameObject levelUpImage;

        /// <summary>バトルでの街破壊数テキスト</summary>
        [SerializeField] TextMeshProUGUI destructionCountText;

        /// <summary>次のレベルアップに必要な街破壊数テキスト</summary>
        [SerializeField] TextMeshProUGUI needExpText;

        /// <summary>オブジェクトの生成先</summary>
        [SerializeField] private Transform parent;

        /// <summary>経験値ゲージ</summary>
        [SerializeField] private Slider expGauge;

        /// <summary>ストーリーに進むか選択するウィンドウウィンドウ</summary>
        [SerializeField] private GameObject loadStoryWindow;

        /// <summary>画面をタップした回数</summary>
        private int touchCount = 0;

        /// <summary>アニメーションコルーチン</summary>
        private IEnumerator coroutine;

        private float waitTime = 0;

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            panelCounter = PanelCounter.Instance;
        }

        private void Start()
        {
            LoadBeforeStatus();
            
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    if (touchCount == 0)
                    {
                        coroutine = AnimationExpGauge(0.2f);
                        StartCoroutine(coroutine);
                        touchCount = 1;
                    }
                    else if (touchCount == 1)
                    {
                        loadStoryWindow.SetActive(true);
                    }

                    GameObject button;
                    touchInfo.HitDetection(out button);

                    if (button != null)
                    {
                        if (button.name == "Yes")
                        {
                            Debug.Log("yes");
                            SceneChanger.SceneChange(SceneName.Story);
                        }
                        else if (button.name == "No")
                        {
                            Debug.Log("no");
                            loadStoryWindow.SetActive(false);
                        }
                    }
                }
            });
        }

        private void Update()
        {
            needExpText.text = (expGauge.maxValue - expGauge.value).ToString("f0");
        }


        /// <summary>ステータス変動アニメーション</summary>
        /// <param name="addAmount">経験値ゲージの増加量</param>
        /// <returns></returns>
        private IEnumerator AnimationExpGauge(float addAmount)
        {
            while (currentExp < afterExp)
            {
                currentExp += addAmount;
                expGauge.maxValue = requiredExp;
                expGauge.value = currentExp;

                if (expGauge.value >= expGauge.maxValue)//レベルアップしたら
                {
                    levelUpImage.SetActive(true);
                    waitTime += Time.deltaTime;
                    if (waitTime > 2)
                    {
                        levelUpImage.SetActive(false);
                    }

                    afterExp -= requiredExp;
                    currentExp -= requiredExp;
                    requiredExp = magia.GetRequiredExpToNextLevel( + 1);
                    expGauge.maxValue = requiredExp;
                    needExp = expGauge.maxValue - expGauge.value;

                    if (needExp < 0)
                    {
                        needExp = -needExp;
                    }
                    magia.LevelUp();

                    var getStats = magia.GetStats();
                    beforeStatus.Level = getStats.Level;
                    updatedHitPoint = getStats.HitPoint;
                    updatedAttack = getStats.Attack;
                    updatedDefense = getStats.Defense;
                    getStatusPoint = magia.AllocationPoint;

                    UpdateText();
                    expGauge.value = 0;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>バトル前のステータスを取得する</summary>
        public void LoadBeforeStatus()
        {
            loadStoryWindow.SetActive(false);
            levelUpImage.SetActive(false);

            // バトル前のステータスと必要経験値を取得する
            beforeStatus = magia.GetStats();
            currentExp = magia.TotalExperience;
            destructionCount = panelCounter.TotalDestructionCount;
            requiredExp = magia.GetRequiredExpToNextLevel(beforeStatus.Level);

            
            needExp = requiredExp - currentExp;

            if (needExp < 0)
            {
                needExp = -needExp;
            }

            afterExp = currentExp + destructionCount;
            expGauge.maxValue = requiredExp;
            expGauge.value = currentExp;
            

            getStatusPoint = magia.AllocationPoint;

            UpdateText();
            updatedBasicStatusTexts[0].text = beforeStatus.HitPoint.ToString();
            updatedBasicStatusTexts[1].text = beforeStatus.Attack.ToString();
            updatedBasicStatusTexts[2].text = beforeStatus.Defense.ToString();
        }

        /// <summary>変動後のステータスを表示</summary>
        private void UpdateStatus()
        {
            

           
        }

        /// <summary>テキスト更新</summary>
        private void UpdateText()
        {
            levelText.text = beforeStatus.Level.ToString();
            currentBasicStatusTexts[0].text = beforeStatus.HitPoint.ToString();
            currentBasicStatusTexts[1].text = beforeStatus.Attack.ToString();
            currentBasicStatusTexts[2].text = beforeStatus.Defense.ToString();

            updatedBasicStatusTexts[0].text = updatedHitPoint.ToString();
            updatedBasicStatusTexts[1].text = updatedAttack.ToString();
            updatedBasicStatusTexts[2].text = updatedDefense.ToString();

            destructionCountText.text = destructionCount.ToString();
            needExpText.text = needExp.ToString();
            getStatusPointText.text = getStatusPoint.ToString();
        }
    }
}
