using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DemonicCity;
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

        /// <summary>現在のレベル</summary>
        private int currentlevel;

        /// <summary>現在の体力</summary>
        private int currentHitPoint;

        /// <summary>現在の攻撃力</summary>
        private int currentAttack;

        /// <summary>現在の防御力</summary>
        private int currentDefense;

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
        private float updatedExp;

        /// <summary>ゲージが変動する際の増加量</summary>
        private float addAmount = 0.02f;

        /// <summary>経験値ゲージ</summary>
        [SerializeField] private Image expGauge;

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

        [SerializeField] private Transform parent;

        private GameObject popUpObject;

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            panelCounter = new PanelCounter();

        }

        private void Start()
        {
            LoadBeforeStatus();
            UpdateText();

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    //アニメーションをスキップする処理
                    //タップでバトル後のステータスを表示
                    //次の章へ進むかホーム画面へ戻るかのウィンドウをポップアップ
                    StartCoroutine("AnimationExpGauge");
                }
            });
        }

        /// <summary>経験値ゲージのアニメーション演出</summary>
        /// <returns></returns>
        private IEnumerator AnimationExpGauge()
        {
            while (currentExp < updatedExp)
            {
                currentExp += addAmount;
                expGauge.fillAmount = currentExp / requiredExp;

                if (expGauge.fillAmount >= 1)
                {
                    popUpObject = Instantiate(levelUpImage, parent);
                    Destroy(popUpObject, 2);

                    updatedExp -= requiredExp;
                    currentExp -= requiredExp;
                    requiredExp = magia.GetRequiredExpToNextLevel(currentlevel + 1);
                    currentlevel += 1;
                    needExp = requiredExp - currentExp;
                    if (needExp < 0)
                    {
                        needExp = -needExp;
                    }
                    magia.LevelUp();
                    var getStats = magia.GetStats();
                    currentlevel = getStats.m_level;
                    updatedHitPoint = getStats.m_hitPoint;
                    updatedAttack = getStats.m_attack;
                    updatedDefense = getStats.m_defense;

                    for (int a = 0; a < updatedBasicStatusTexts.Length; a++)
                    {
                        updatedBasicStatusTexts[a].enabled = true;
                    }
                    addAmount *= 1.2f;
                    UpdateText();
                    expGauge.fillAmount = 0;

                }
                yield return new WaitForSeconds(0.01f);
            }
        }


        /// <summary>バトル前のステータスを表示</summary>
        public void LoadBeforeStatus()
        {
            var getStats = magia.GetStats();
            currentlevel = getStats.m_level;
            currentHitPoint = getStats.m_hitPoint;
            currentAttack = getStats.m_attack;
            currentDefense = getStats.m_defense;

            for (int i = 0; i < updatedBasicStatusTexts.Length; i++)
            {
                updatedBasicStatusTexts[i].enabled = false;
            }

            //currentExp = magia.TotalExperience;
            //destructionCount = panelCounter.TotalDestructionCount;
            currentExp = 4;
            destructionCount = 50;

            needExp = requiredExp - currentExp;
            if (needExp < 0)
            {
                needExp = -needExp;
            }
            updatedExp = currentExp + destructionCount;
            requiredExp = magia.GetRequiredExpToNextLevel(currentlevel);

            expGauge.fillAmount = currentExp / requiredExp;

            getStatusPoint = magia.AllocationPoint;
        }

        /// <summary>テキスト更新</summary>
        public void UpdateText()
        {
            levelText.text = currentlevel.ToString();
            currentBasicStatusTexts[0].text = currentHitPoint.ToString();
            currentBasicStatusTexts[1].text = currentAttack.ToString();
            currentBasicStatusTexts[2].text = currentDefense.ToString();

            updatedBasicStatusTexts[0].text = updatedHitPoint.ToString();
            updatedBasicStatusTexts[1].text = updatedAttack.ToString();
            updatedBasicStatusTexts[2].text = updatedDefense.ToString();

            destructionCountText.text = destructionCount.ToString();
            needExp = Mathf.CeilToInt(needExp);
            needExpText.text = needExp.ToString();
            getStatusPointText.text = getStatusPoint.ToString();
        }
    }
}
