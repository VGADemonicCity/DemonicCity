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

        const float widthRatio = 9f;
        const float heightRatio = 16f;

        /// <summary></summary>
        SceneFader m_sceneFader;

        private void Awake()
        {
            m_sceneFader = SceneFader.Instance;

            SceneManager.sceneLoaded += ((scene, mode) =>
            {
                m_sceneFader.FadeIn(); // SceneLoad時の画面演出
                Time.timeScale = 1f;
                // 解像度を調整する
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