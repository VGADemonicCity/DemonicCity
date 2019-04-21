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
                AdjustmentRatio();
            });
        }

        void AdjustmentRatio()
        {
            var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            // 理想の画面の比率
            float targetRatio = widthRatio / heightRatio;
            // 現在の画面の比率
            float currentRatio = Screen.width * 1f / Screen.height;
            // 理想と現在の比率
            float ratio = targetRatio / currentRatio;

            //カメラの描画開始位置をX座標にどのくらいずらすか
            float rectX = (1.0f - ratio) / 2f;
            //カメラの描画開始位置と表示領域の設定
            camera.rect = new Rect(rectX, 0f, ratio, 1f);
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
