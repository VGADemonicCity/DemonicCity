using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Shuffle panels.
    /// スキル : シャッフルパネル
    /// 引いたパネル枚数が30枚以上の時、画面に表示されている3*3のパネルを引いていない状態に戻してシャッフルする
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ShufflePanels : MonoBehaviour
    {

        /// <summary>
        /// パネルシャッフルスキルが発動可能かどうかの状態を表す
        /// </summary>
        public bool IsActivatable
        {
            get
            {
                if (panelCounter.CounterForShuffleSkill >= conditions)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int Conditions
        {
            get { return conditions; }
        }

        /// <summary>colliderを検出する為のcollider</summary>
        [SerializeField] BoxCollider2D m_sensor;
        /// <summary>Conditionsのバッキングフィールド</summary>
        [SerializeField] int conditions = 30;
        /// <summary>Positive button</summary>
        [SerializeField] Button PositiveButton;
        /// <summary>Negative button</summary>
        [SerializeField] Button NegativeButton;
        /// <summary>popup system</summary>
        [SerializeField] PopupSystem popupSystem;
        /// <summary>Skill animator</summary>
        [SerializeField] Animator skillAnim;
        /// <summary>チュートリアル画面に表示する項目リスト</summary>
        List<Subject> targetTutorialsList = new List<Subject>{
            Subject.AboutTeleportSkill,
            Subject.AboutTeleportSkill_2,
            Subject.AboutTeleportSkill_3,
            };

        /// <summary>BattleManager</summary>
        BattleManager battleManager;
        /// <summary>PanelCounterの参照</summary>
        PanelCounter panelCounter;

        [Header("Parameters")]
        [SerializeField] float panelRotateTime = 1f;
        [SerializeField] float intervalForEachRotation = .1f;
        [SerializeField] Axis rotateAxis;





        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            panelCounter = PanelCounter.Instance; // PannelCounterの参照取得
            battleManager = BattleManager.Instance;
        }

        /// <summary>
        /// registration events
        /// </summary>
        private void Start()
        {
            GameObject hitResult;

            TouchGestureDetector.Instance.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.Click
                && battleManager.m_StateMachine.m_State == BattleManager.StateMachine.State.PlayerChoice
                && touchInfo.HitDetection(out hitResult)
                && IsActivatable
                && !PanelManager.Instance.IsOpenedAllPanelsExceptEnemyPanels)
                {
                    if (hitResult.tag != "ShufflePanels")
                    {
                        return;
                    }
                    battleManager.SetStateMachine(BattleManager.StateMachine.State.Pause);
                    popupSystem.Popup();
                    popupSystem.SubscribeButton(new PopupSystemMaterial(Activate, PositiveButton.gameObject.name, true));
                    popupSystem.SubscribeButton(new PopupSystemMaterial(Cancel, NegativeButton.gameObject.name, true));
                }
            });

            // animationが終わった時にスキルを発動するイベントを登録する
            var animBehaviour = skillAnim.GetBehaviour<AnimBehaviour>();
            animBehaviour.SetStateExitEventWithState(PanelShuffle);
        }

        /// <summary>
        /// starting animation of skill activate.
        /// </summary>
        void Activate()
        {
            skillAnim.SetTrigger("Activate");
            battleManager.SetStateMachine(battleManager.m_StateMachine.PreviousStateWithoutPause);
        }

        /// <summary>
        /// close popup window.
        /// </summary>
        void Cancel()
        {
            battleManager.SetStateMachine(battleManager.m_StateMachine.PreviousStateWithoutPause);
        }

        public void OnCompleteConditions()
        {
            // Inspectorで指定したフラグをここで代入する
            var targetTutorials = new Subject();
            targetTutorialsList.ForEach(item =>
            {
                targetTutorials = targetTutorials | item;
            });

            // Tutorialのフラグが立っていた時のみチュートリアルを再生しフラグを下げ二度と呼ばれないようにする
            var progress = Progress.Instance;
            var tutorialFlag = progress.TutorialProgressInBattleScene;
            if (targetTutorials == (tutorialFlag & targetTutorials))
            {
                BattleSceneTutorialsPopper.Instance.Popup(targetTutorials);
                progress.SetTutorialProgress(targetTutorials, false);
            }

        }


        /// <summary>
        /// 現在画面に表示されている3*3のパネルを全て非表示の状態に戻して、その3*3のパネル内でまたシャッフルさせる
        /// </summary>
        public void PanelShuffle(AnimatorStateInfo stateInfo)
        {
            var targetHash = Animator.StringToHash("TransitionSkill");
            if (stateInfo.shortNameHash == targetHash)
            {
                Debug.Log("called");
                StartCoroutine(SkillActivate());
            }
        }

        /// <summary>
        /// Skill animation
        /// </summary>
        /// <returns></returns>
        IEnumerator SkillActivate()
        {
            m_sensor.enabled = true; // colliderをactiveにする
            var results = new Collider2D[9]; // 結果を受け取るための配列
            m_sensor.OverlapCollider(new ContactFilter2D(), results); // 設定したcolliderと重なっているcolliderを検出し配列に代入する

            var panelList = new List<Panel>(); // colliderに検出されたオブジェクトのPanelの参照リスト
            var panelTypes = new List<PanelType>(); // colliderに検出されたパネルのPanelTypeをリストで格納
            foreach (var panel in results) // colliderに検出されたパネルとそのPanelTypeを全てリストに格納する
            {
                var panelObject = panel.gameObject.GetComponent<Panel>(); // Panelの参照取得
                panelList.Add(panelObject); // Panelをリストに追加
                panelTypes.Add(panelObject.MyPanelType); // PanelTypeをリストに追加
            }

            var result = panelTypes.OrderBy((panelType) => Guid.NewGuid()).ToArray(); // Guid配列に変換、OrderByでアルファベット順に並び替える
            var count = 0; // ForEachに使うresult配列の要素指定用のカウンター

            foreach (var panel in panelList)
            {
                panel.MyPanelType = result[count]; // PanelTypeの代入
                panel.ResetPanel(); // パネルを引いていない状態に戻す
                panel.Rotate(rotateAxis.ToString(), panelRotateTime);
                count++; // カウントアップ
                yield return new WaitForSeconds(intervalForEachRotation);
            }

            m_sensor.enabled = false; // colliderをdisableにする
            panelCounter.ResetShuffleSkillCounter(); // カウンターをリセット
            battleManager.SetStateMachine(battleManager.m_StateMachine.PreviousStateWithoutPause); // stateを元に戻す       
        }

        /// <summary>
        /// スキル発動時パネルを回転させる軸
        /// </summary>
        enum Axis
        {
            x,
            y,
            z,
        }
    }
}

