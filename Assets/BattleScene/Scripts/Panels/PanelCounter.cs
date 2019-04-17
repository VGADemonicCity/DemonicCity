using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// パネルカウンタークラス
    /// パネルの種類に応じてカウントをとる
    /// 敵パネルを引いた際はStateMachineを遷移させる
    /// </summary>
    public class PanelCounter : MonoSingleton<PanelCounter>
    {
        /// <summary>m_counterFoeShuffleSkillのプロパティ</summary>
        public int CounterForShuffleSkill
        {
            get
            {
                return m_counterForShuffleSkill;
            }
            private set
            {
                m_counterForShuffleSkill = value;
            }
        }

        /// <summary>
        /// Gets the destruction count.
        /// </summary>
        /// <value>The destruction count.</value>
        public int DestructionCount
        {
            get
            {
                return m_destructionCount;
            }
        }

        /// <summary>
        /// Gets the total destruction count.
        /// </summary>
        /// <value>The total destruction count.</value>
        public int TotalDestructionCount
        {
            get
            {
                return m_totalDestructionCount;
            }
            set
            {
                m_totalDestructionCount = value;
            }
        }

        /// <summary>シャッフルスキル専用カウンター : inspectorに表示させる為アクセサーと使用変数を分けている</summary>
        [SerializeField] int m_counterForShuffleSkill;
        /// <summary>ターン毎のパネルカウントのカウント変数</summary>
        [SerializeField] int m_cityCount;
        /// <summary>ターン毎のダブルパネルのカウント変数</summary>
        [SerializeField] int m_doubleCount;
        /// <summary>そのターンの街破壊数</summary>
        [SerializeField] int m_destructionCount;
        /// <summary>ターン毎のトリプルパネルのカウント変数</summary>
        [SerializeField] int m_tripleCount;
        /// <summary>そのバトルにおけるパネルの総カウント数</summary>
        [SerializeField] int m_totalPanelCount;
        /// <summary>そのバトルにおける総街破壊数</summary>
        [SerializeField] int m_totalDestructionCount;

        Subject enemyPanelOpen = Subject.FirstPanelOpen_4;
        Subject afterFirstPanelOpen = Subject.FirstPanelOpen_2 | Subject.FirstPanelOpen_3;


        /// <summary>固有スキルゲージ</summary>
        [SerializeField] UniqueSkillGauge m_uniqueSkillGauge;
        /// <summary>攻撃ボタンを表示する</summary>
        [SerializeField] AttackButtonProcess attackButtonProcess;
        [SerializeField] PanelComplete panelComplete;
        /// <summary>街破壊数のカウンター</summary>
        [SerializeField] DestructionCounterBox destructionCounterBox;

        /// <summary>BattleManagerのシングルトンインスタンスの参照</summary>
        BattleManager m_battleManager;

        /// <summary>バトルボタンを表示するフラグ</summary>
        bool isDisplayed;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
            if (m_battleManager == null) Debug.LogError("BattleManager is null.");
        }




        /// <summary>
        /// return the total count.
        /// </summary>
        /// <returns>The total count.</returns>
        public int PanelCount()
        {
            return m_cityCount + m_doubleCount + m_tripleCount; // そのターンパネルを引いた枚数を返す. 固有スキル用
        }

        /// <summary>
        /// Inits the counts.
        /// </summary>
        public void InitializeCounter()
        {
            // カウンターを全てリセットする
            m_cityCount = 0;
            m_doubleCount = 0;
            m_tripleCount = 0;
            m_destructionCount = 0;

            // 攻撃ボタンを表示可能にする
            isDisplayed = false;


            if (m_battleManager.m_StateMachine.m_State == BattleManager.StateMachine.State.Init) // ゲーム開始時のみトータルパネルカウントとスキル用カウンターを初期化する
            {
                m_totalPanelCount = 0;
                m_counterForShuffleSkill = 0;
                m_totalDestructionCount = 0;
            }
        }

        /// <summary>
        /// Resets the shuffle skill flag.
        /// シャッフルスキルが使われたらこのメソッドを呼んでシャッフルスキル用カウンターを0に戻す
        /// </summary>
        public void ResetShuffleSkillCounter()
        {
            m_counterForShuffleSkill = 0;
        }

        /// <summary>
        /// パネルのタイプで判別して対応した処理をする
        /// </summary>
        /// <param name="panel">Panel.</param>
        public void PanelJudger(Panel panel)
        {
            var destructionCount = 0; // 街破壊数の一時保存変数
            switch (panel.MyPanelType)
            {
                case PanelType.City: // cityパネルを引いた時
                    m_cityCount++; // シティパネルカウントアップ
                    destructionCount++;
                    if (!isDisplayed)
                    {
                        isDisplayed = true;
                        attackButtonProcess.DisplayAttackButton();
                    }
                    // UIに更新
                    destructionCounterBox.OnPanelCount(destructionCount);
                    // 条件に沿って適切なチュートリアルを表示する
                    TryDisplayTutorials(afterFirstPanelOpen);
                    break;

                case PanelType.DoubleCity: // doubleパネルを引いた時
                    m_doubleCount++;// ダブルパネルカウントアップ
                    destructionCount += 2;
                    if (!isDisplayed)
                    {
                        isDisplayed = true;
                        attackButtonProcess.DisplayAttackButton();
                    }
                    // UIに更新
                    destructionCounterBox.OnPanelCount(destructionCount);
                    // 条件に沿って適切なチュートリアルを表示する
                    TryDisplayTutorials(afterFirstPanelOpen);
                    break;

                case PanelType.TripleCity: // tripleパネルを引いた時
                    m_tripleCount++;// トリプルパネルカウントアップ
                    destructionCount += 3;
                    if (!isDisplayed)
                    {
                        isDisplayed = true;
                        attackButtonProcess.DisplayAttackButton();
                    }
                    // UIに更新
                    destructionCounterBox.OnPanelCount(destructionCount);
                    // 条件に沿って適切なチュートリアルを表示する
                    TryDisplayTutorials(afterFirstPanelOpen);
                    break;

                case PanelType.Enemy: // enemyパネルを引いた時ペナルティ処理を行う
                    // 攻撃ボタンを閉じてフラグを降ろす
                    attackButtonProcess.ButtonClose();

                    // =========================================
                    // イベント呼び出し : StateMachine.EnemyAttack
                    // =========================================
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.EnemyAttack);
                    // 条件に沿って適切なチュートリアルを表示する
                    TryDisplayTutorials(enemyPanelOpen);
                    break;
            }
            m_totalPanelCount++; // 全てのパネルカウントアップ
            m_counterForShuffleSkill++; // シャッフルスキル専用カウンターアップ
            m_totalDestructionCount += destructionCount; // 総計に加算
            m_destructionCount += destructionCount;
            m_uniqueSkillGauge.Sync(); // 固有スキルゲージに同期

            // パネルコンプリート
            if (PanelManager.Instance.IsOpenedAllPanelsExceptEnemyPanels && m_battleManager.m_StateMachine.m_State == BattleManager.StateMachine.State.PlayerChoice)
            {
                attackButtonProcess.ButtonClose();
                StartCoroutine(panelComplete.PlayPanelCompleteSkillAnimation());
            }
        }

        void TryDisplayTutorials(Subject item)
        {
            // Inspectorで指定したフラグをここで代入する
            var targetTutorials = new Subject();
            targetTutorials = targetTutorials | item;
            if(targetTutorials == 0)
            {
                return;
            }

            // Tutorialのフラグが立っていた時のみチュートリアルを再生しフラグを下げ二度と呼ばれないようにする
            var progress = Progress.Instance;
            var tutorialFlag = progress.TutorialProgressInBattleScene;
            if (targetTutorials == (tutorialFlag & targetTutorials))
            {
                BattleSceneTutorialsPopper.Instance.Popup(targetTutorials);
                progress.SetTutorialProgress(targetTutorials, false);
            }
        }
    }
}