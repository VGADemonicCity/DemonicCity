
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
        readonly List<Subject> targetTutorialsList = new List<Subject>
        {
            Subject.AboutPanels,
            Subject.AboutPanelFrame,
            Subject.AboutAttack,
            Subject.AboutPause,
            Subject.CompletePanels,
            Subject.FirstPanelOpen,
        };

        Progress progress;
        Magia magia;

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
                if (state != BattleManager.StateMachine.State.PlayerChoice
                || m_battleManager.m_StateMachine.PreviousStateIsDebugging
                || m_battleManager.m_StateMachine.PreviousStateIsPause
                || m_battleManager.m_StateMachine.PreviousStateIsTutorial) // StateがPlayerChoice以外の時は処理終了
                {
                    return;
                }

                m_panelCounter.InitializeCounter();
                m_panelManager.InitPanels();
                m_panelFrameManager.MovingCenter();

                // Tutorialのフラグが立っていた時のみチュートリアルを再生しフラグを下げ二度と呼ばれないようにする
                progress = Progress.Instance;
                var tutorialFlag = progress.TutorialProgressInBattleScene;
                if (targetTutorials == (tutorialFlag & targetTutorials))
                {
                    BattleSceneTutorialsPopper.Instance.Popup(targetTutorials);
                    progress.SetTutorialProgress(targetTutorials, false);
                }

                progress = Progress.Instance;
                magia = Magia.Instance;
            });
        }
    }
}