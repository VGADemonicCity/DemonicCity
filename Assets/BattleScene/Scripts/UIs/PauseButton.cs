using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 
    /// </summary>
    public class PauseButton : MonoBehaviour
    {
        /// <summary>popup system</summary>
        [SerializeField] PopupSystem popupSystem;
        /// <summary>BattleManager</summary>
        BattleManager battleManager;
        /// <summary>Button for back to home </summary>
        [SerializeField] Button backToHomeButton;
        /// <summary>Button for restarting</summary>
        [SerializeField] Button restartingButton;
        /// <summary>Button for resumes</summary>
        [SerializeField] Button resumesButton;

        List<PopupSystemMaterial> ButtonEvents;

        private void Awake()
        {
            battleManager = BattleManager.Instance;
            // Buttonのイベント登録に使う素材を作成し,全て登録する
            ButtonEvents = new List<PopupSystemMaterial> {
                    new PopupSystemMaterial(BackToHomeScene,backToHomeButton.gameObject.name,true),
                    new PopupSystemMaterial(RestartingBattleScene,restartingButton.gameObject.name,true),
                    new PopupSystemMaterial(Resumes,resumesButton.gameObject.name,true),
            };
        }

        /// <summary>
        /// ポース画面を出す
        /// </summary>
        public void InningThePause()
        {
            if (battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Pause
                && battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Init
                && battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.Win)
            {
                battleManager.SetStateMachine(BattleManager.StateMachine.State.Pause);
                popupSystem.Popup();
                ButtonEvents.ForEach(item => popupSystem.SubscribeButton(item));
            }
        }

        /// <summary>
        /// ホーム画面へ戻る
        /// </summary>
        void BackToHomeScene()
        {
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Home, 1f);
        }
        /// <summary>
        /// バトルをリスタートする
        /// </summary>
        void RestartingBattleScene()
        {
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Battle, 1f);
        }
        /// <summary>
        /// バトルを再開する
        /// </summary>
        void Resumes()
        {
            battleManager.SetStateMachine(battleManager.m_StateMachine.m_PreviousState);
        }
    }
}