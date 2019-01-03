using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DemonicCity
{
    /// <summary>
    /// Scene fader.
    /// </summary>
    public class SceneFader : MonoSingleton<SceneFader>
    {
        /// <summary>フェード時に使用するCanvas</summary>
        private Canvas m_fadeCanvas;
        /// <summary>フェーディング演出に使うImage</summary>
        private Image m_fadeImage;
        /// <summary>m_fadeImadeのアルファ値</summary>
        private float m_alpha;
        /// <summary>フェーディング演出に掛ける時間</summary>
        private float m_fadeTime = 3f;
        /// <summary>遷移先のシーンタイトル</summary>
        private string m_nextSceneTitle;

        /// <summary>
        /// 初期化
        /// </summary>
        private void Init()
        {
            // fade用のCanvas生成  ※(gameObjectとSceneFader.cs自体はMonoSingletonのInstanceプロパティ呼び出し時に生成,アタッチしている)
            m_fadeCanvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
            m_fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // 最前面になるようLayer設定
            m_fadeCanvas.sortingLayerName = "UI";
            m_fadeCanvas.sortingOrder = 10000; // 適当な値(最前面,つまりこの値がSceneにあるどのUIレイヤーのsortingOrderよりも高ければなんでもよい)

            // fade用のImage生成
            m_fadeImage = new GameObject("ImageFade").AddComponent<Image>();
            m_fadeImage.color = Color.black;
            m_fadeImage.transform.SetParent(m_fadeCanvas.transform, false);
            m_fadeImage.rectTransform.anchoredPosition = Vector2.zero;

            // 画面のサイズに合わせてImageのサイズ設定
            m_fadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        public void FadeIn(float fadeTime = 0f)
        {
            if (fadeTime != 0f)
            {
                m_fadeTime = fadeTime;
            }

            StartCoroutine(FadingIn());
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="sceneTitle">遷移先のシーンタイトル</param>
        /// <param name="fadeTime">フェーディング処理に掛ける時間</param>
        public void FadeOut(SceneTitle sceneTitle, float fadeTime = 0f)
        {
            if (fadeTime != 0f)
            {
                m_fadeTime = fadeTime;
            }

            m_nextSceneTitle = sceneTitle.ToString();
            StartCoroutine(FadingOut());
        }



        /// <summary>
        /// フェードインのコルーチン
        /// </summary>
        IEnumerator FadingIn()
        {
            if (m_fadeImage == null)
            {
                Init();
            }
            m_fadeImage.color = Color.black;
            m_alpha = 1f;
            while (m_alpha <= 1f)
            {
                m_alpha -= Time.deltaTime / m_fadeTime;
                m_fadeImage.color = new Color(0f, 0f, 0f, m_alpha);
                yield return null;
            }
        }

        /// <summary>
        /// フェードアウトのコルーチン
        /// </summary>
        IEnumerator FadingOut()
        {
            if (m_fadeImage == null)
            {
                Init();
            }
            while (m_alpha < 1f)
            {
                m_alpha += Time.deltaTime / m_fadeTime;
                m_fadeImage.color = new Color(0f, 0f, 0f, m_alpha);
                yield return null;
            }
            SceneManager.LoadScene(m_nextSceneTitle);
        }

        /// <summary>
        /// インスタンス生成時に行う任意の処理
        /// </summary>
        public override void OnInitialize()
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Sceneのタイトル一覧
        /// </summary>
        public enum SceneTitle
        {
            Battle,
            Title,
            Story,
            Home,
            StorySelect,
            Strengthen,
        }
    }
}