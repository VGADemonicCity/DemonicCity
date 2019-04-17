using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class WinState : StatesBehaviour
    {
        [SerializeField] GameObject resultWindow;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {

                if (state != BattleManager.StateMachine.State.Win || m_battleManager.m_StateMachine.PreviousStateIsPause) // StateがWin以外の時は処理終了
                {
                    return;
                }
                Debug.Log("Win state called.");

                //=======================
                //Resultのポップアップ等の処理を書く予定
                //=======================

                // TODO: 適正レベルに応じて総街破壊数に倍率処理を入れる


                m_panelCounter.TotalDestructionCount = GetExpForRatio(m_panelCounter.TotalDestructionCount);


                // りょうくんのリザルト画面呼び出し
                resultWindow.SetActive(true);
            });
        }


        /// <summary>
        /// 倍率をかけた経験値を返す。
        /// </summary>
        /// <param name="panelCount">街破壊数</param>
        /// <param name="level">現在のレベル</param>
        /// <returns></returns>
        int GetExpForRatio(int panelCount)
        {
            int level = m_magia.Stats.Level;
            float ratio = 1f;
            int[] range = ChapterManager.Instance.GetChapter().levelRange;
            if (level < range[0])
            {
                ratio = 1.5f;
            }
            if (range[1] < level)
            {
                ratio = 0.5f;
            }
            var result = panelCount * ratio;
            result = Mathf.RoundToInt(result);
            return (int)(result);
        }
    }
}