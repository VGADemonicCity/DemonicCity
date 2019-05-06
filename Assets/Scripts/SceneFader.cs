using System.Collections;
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
        static private Canvas m_fadeCanvas;
        /// <summary>フェーディング演出に使うImage</summary>
        private Image m_fadeImage;
        private Color fadingColor = Color.black;

        /// <summary>m_fadeImadeのアルファ値</summary>
        private float m_alpha;
        /// <summary>フェーディング演出に掛ける時間</summary>
        private float m_fadeTime = 1f;
        /// <summary>遷移先のシーンタイトル</summary>
        private string m_nextSceneTitle;

        private void Awake()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                TouchGestureDetector.Instance.onGestureDetected.RemoveAllListeners();
            };
        }

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
            m_fadeCanvas.sortingOrder = 1000; // 適当な値(最前面,つまりこの値がSceneにあるどのUIレイヤーのsortingOrderよりも高ければなんでもよい)

            // fade用のImage生成
            m_fadeImage = new GameObject("ImageFade").AddComponent<Image>();
            m_fadeImage.color = Color.white;
            m_fadeImage.transform.SetParent(m_fadeCanvas.transform, false);
            m_fadeImage.rectTransform.anchoredPosition = Vector2.zero;

            // 画面のサイズに合わせてImageのサイズ設定
            m_fadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            Debug.Log(m_fadeImage.rectTransform.sizeDelta);
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        public void FadeIn(float fadeTime = 1f)
        {
            if (fadeTime != 1f)
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
        public void FadeOut(SceneTitle sceneTitle, float fadeTime = 1f, FadeColor color = FadeColor.Black)
        {
            TouchGestureDetector.Instance.onGestureDetected.RemoveAllListeners();

            if (fadeTime != 1f)
            {
                m_fadeTime = fadeTime;
            }

            switch (color)
            {
                case FadeColor.White:
                    fadingColor = Color.white;
                    break;
                case FadeColor.Black:
                    fadingColor = Color.black;
                    break;
                default:
                    break;
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
            m_fadeImage.color = Color.white;
            m_alpha = 1f;
            while (m_alpha > 0f)
            {
                m_alpha -= Time.unscaledDeltaTime * m_fadeTime;
                m_fadeImage.color = new Color(fadingColor.r, fadingColor.g, fadingColor.b, m_alpha);
                yield return null;
            }
            m_fadeCanvas.enabled = false;
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
            m_fadeCanvas.enabled = true;
            m_alpha = 0f;

            while (m_alpha < 1f)
            {
                m_alpha += Time.unscaledDeltaTime * m_fadeTime;
                m_fadeImage.color = new Color(fadingColor.r, fadingColor.g, fadingColor.b, m_alpha);
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
            EndCredit,
        }

        /// <summary>
        ///  Background color during transition
        /// </summary>
        public enum FadeColor
        {
            White,
            Black,
        }
    }
}