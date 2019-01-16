using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonicCity
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        /// <summary>現在のチャプター</summary>
        public Chapter CurrentChapter
        {
            get { return m_currentChapter; }
            set { m_currentChapter = value; }
        }

        /// <summary></summary>
        SceneFader m_sceneFader;
        /// <summary></summary>
        ChapterManager m_chapterManager;
        Chapter m_currentChapter;

        private void Awake()
        {
            m_sceneFader = SceneFader.Instance;

            SceneManager.sceneLoaded += ((scene, mode) =>
            {
                m_sceneFader.FadeIn(); // SceneLoad時の画面演出
            });
        }



        /// <summary>
        /// 作ったゲームオブジェクトをシーン遷移しても壊されない様にする
        /// </summary>
        public override void OnInitialize()
        {
            base.OnInitialize();
            DontDestroyOnLoad(Instance);
        }
    }
}
