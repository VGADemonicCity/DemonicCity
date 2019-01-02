using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DemonicCity;
using DemonicCity.BattleScene;

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
        private int level;

        /// <summary>現在の体力</summary>
        private int hitPoint;

        /// <summary>現在の攻撃力</summary>
        private int attack;

        /// <summary>現在の防御力</summary>
        private int defense;

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
        private int numberOfDestruction;

        /// <summary>レベルアップに必要な街破壊数</summary>
        private int nextLevelDestruction;


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

        /// <summaryバトルでの街破壊数テキスト</summary>
        [SerializeField]
        TextMeshProUGUI numberOfDestructionText;

        /// <summary>レベルアップに必要な街破壊数テキスト</summary>
        [SerializeField]
        TextMeshProUGUI nextLevelDestructionText;

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            panelCounter = PanelCounter.Instance;
        }

        private void Start()
        {
            LoadResultScene();

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if(gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                   //アニメーションをスキップする処理
                   //次の章へ進むかホーム画面へ戻るかのウィンドウをポップアップ
                }
            });
        }
        /// <summary>リザルト画面に遷移したとき最初に行う処理</summary>
        public void  LoadResultScene()
        {
            levelUpText.enabled = false;

            level = magia.GetStats().m_level;
            hitPoint = magia.GetStats().m_hitPoint;
            attack = magia.GetStats().m_attack;
            defense = magia.GetStats().m_defense;

            numberOfDestruction = panelCounter.DestructionCount;
            Debug.Log(panelCounter.DestructionCount);

        }

    }
}
