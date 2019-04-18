
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Player choice.
    /// </summary>
    public class PlayerChoiceState : StatesBehaviour
    {
        /// <summary>チュートリアル画面に表示する項目リスト</summary>
         List<Subject> targetTutorialsList = new List<Subject>
        {
            Subject.AboutPanels,
            Subject.AboutAttack,
            Subject.AboutPause,
            Subject.CompletePanels,
            Subject.FirstPanelOpen,
        };

        /// <summary>
        /// Start this instance.a
        /// </summary>
        void Start()
        {
            // Inspectorで指定したフラグをここで代入する
            var targetTutorials = new Subject();
            targetTutorialsList.ForEach(item =>
            {
                targetTutorials = targetTutorials | item;
            });

            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.PlayerChoice || m_battleManager.m_StateMachine.PreviousStateIsPause) // StateがPlayerChoice以外の時は処理終了
                {
                    return;
                }

                m_panelCounter.InitializeCounter();
                m_panelManager.InitPanels();
                m_panelFrameManager.MovingCenter();

                // Tutorialのフラグが立っていた時のみチュートリアルを再生しフラグを下げ二度と呼ばれないようにする
                var progress = Progress.Instance;
                var tutorialFlag = progress.TutorialProgressInBattleScene;
                if (targetTutorials == (tutorialFlag & targetTutorials))
                {
                    BattleSceneTutorialsPopper.Instance.Popup(targetTutorials);
                    progress.SetTutorialProgress(targetTutorials, false);
                }


                // ==============================
                // ここにプレイヤーターンが始まった時の処理を書く
                // PlayerCoiceStateから遷移する処理はPanelCounterが敵パネルを認識してState遷移処理をさせている
                // PlayerChoiceの時のinvokeはPanelCounter.PanelJudgerが行っている
                // ==============================
                StartCoroutine(Activate());
            });
        }

        IEnumerator Activate()
        {
            yield return null;
        }
    }
}