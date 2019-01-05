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

        /// <summary>バトルでの街破壊数</summary>
        private int destructionCount;

        /// <summary>次のレベルアップに必要な経験値</summary>
        private int getRequiredExp;

        /// <summary>現在の経験値(累計街破壊数)</summary>
        private int currentExp;

        /// <summary>経験値ゲージ</summary>
        [SerializeField]
        private Image expGauge;

        /// <summary>勝敗テキスト</summary>
        [SerializeField]
        TextMeshProUGUI resultText;

        /// <summary>現在のレベルテキスト</summary>
        [SerializeField]
        TextMeshProUGUI levelText;

        /// <summary>現在の基礎ステータステキスト</summary>
        [SerializeField]
        TextMeshProUGUI[] currentBasicStatusTexts = new TextMeshProUGUI[3];

        /// <summary>変動後の基礎ステータステキスト</summary>
        [SerializeField]
        TextMeshProUGUI[] updatedBasicStatusTexts = new TextMeshProUGUI[3];

        /// <summary>獲得した割り振りポイントテキスト(魔力値)</summary>
        [SerializeField]
        TextMeshProUGUI getStatusPointText;

        /// <summary>レベルアップしたときに表示するテキスト</summary>
        [SerializeField]
        TextMeshProUGUI levelUpText;

        /// <summary>勝利したぞーー！テキスト</summary>
        [SerializeField]
        TextMeshProUGUI winText;

        /// <summary>バトルでの街破壊数テキスト</summary>
        [SerializeField]
        TextMeshProUGUI destructionCountText;

        /// <summary>次のレベルアップに必要な街破壊数テキスト</summary>
        [SerializeField]
        TextMeshProUGUI getRequiredExpText;



        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            panelCounter = PanelCounter.Instance;
        }

        private void Start()
        {
            //resultText.enabled = false;
            levelUpText.enabled = false;
            LoadResultScene();
            UpdateText();

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    //アニメーションをスキップする処理
                    //タップでバトル後のステータスを表示
                    //次の章へ進むかホーム画面へ戻るかのウィンドウをポップアップ
                }
            });
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                magia.LevelUp();
                levelUpText.enabled = true;
                currentlevel = magia.GetStats().m_level;
                currentHitPoint = magia.GetStats().m_hitPoint;
                currentAttack = magia.GetStats().m_attack;
                currentDefense = magia.GetStats().m_defense;
                UpdateText();
                expGauge.fillAmount = (currentExp) / (magia.GetRequiredExpToNextLevel(currentlevel));
            }
        }

        //public void GetExp()
        //{

        //    expGauge.fillAmount = (currentExp) / (magia.GetRequiredExpToNextLevel(currentlevel));
        //}

        /// <summary>バトル前のステータスを表示</summary>
        public void LoadResultScene()
        {

            //if(勝利したら)
            //{
            //    ポップアップアニメーション
            //    resultText.enabled = true;
            //}
            //if(レベルアップしたら)
            //{
            //　　ポップアップアニメーション
            //    levelUpText.enabled = true;
            //}

            currentlevel = magia.GetStats().m_level;
            currentHitPoint = magia.GetStats().m_hitPoint;
            currentAttack = magia.GetStats().m_attack;
            currentDefense = magia.GetStats().m_defense;

            updatedHitPoint = currentHitPoint;
            updatedAttack = currentAttack;
            updatedDefense = currentDefense;

            //destructionCount = panelCounter.DestructionCount;
            destructionCount = 6;
            getRequiredExp = magia.GetRequiredExpToNextLevel(currentlevel);
            Debug.Log(destructionCount);
            //currentExp = 80;
            //needExp = 100;
            //prevNeedExp = needExp - currentExp;// = nextLevelDestruction
           
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
            getRequiredExpText.text = getRequiredExp.ToString();
            getStatusPointText.text = getStatusPoint.ToString();
        }

    }
}
