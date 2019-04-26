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
                //AdjustmentRatio();
            });
        }

        //void AdjustmentRatio()
        //{
        //    var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //    // 理想の画面の比率
        //    float targetRatio = widthRatio / heightRatio;
        //    // 現在の画面の比率
        //    float currentRatio = Screen.width * 1f / Screen.height;
        //    // 理想と現在の比率
        //    float ratio = targetRatio / currentRatio;

        //    //カメラの描画開始位置をX座標にどのくらいずらすか
        //    float rectX = (1.0f - ratio) / 2f;
        //    //カメラの描画開始位置と表示領域の設定
        //    camera.rect = new Rect(rectX, 0f, ratio, 1f);
        //}

        // Use this for initialization
        void Start()
        {

            // set the desired aspect ratio (the values in this example are
            // hard-coded for 16:9, but you could make them into public
            // variables instead so you can set them at design time)
            float targetaspect = 16.0f / 9.0f;

            // determine the game window's current aspect ratio

            float windowaspect = (float)Screen.width / (float)Screen.height;

            // current viewport height should be scaled by this amount

            float scaleheight = windowaspect / targetaspect;

            // obtain camera component so we can modify its viewport

            Camera camera = GetComponent<Camera>();

            // if scaled height is less than current height, add letterbox

            if (scaleheight < 1.0f)
            {
                Rect rect = camera.rect;

                rect.width = 1.0f;

                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;

                camera.rect = rect;

            }
            else // add pillarbox
            {
                float scalewidth = 1.0f / scaleheight;

                Rect rect = camera.rect;

                rect.width = scalewidth;

                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;

            }
        }

        // Update is called once per frame
        void Update()
        {

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