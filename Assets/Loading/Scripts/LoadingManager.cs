using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DemonicCity.Loading
{
    public class LoadingManager : MonoBehaviour
    {
        [SerializeField] LoadAnimation loadAnimation;
        //[SerializeField] Text loadingText;
        [SerializeField] Image gauge;
        [SerializeField] Image back;
        [SerializeField] Image half;
        [SerializeField] Image full;
        float fadeTime = 0.5f;
        public static SceneName NextSceneName { get; set; } = SceneName.Title;
        // Use this for initialization
        void Start()
        {
            half.color = Color.clear;
            full.color = Color.clear;
            half.gameObject.SetActive(true);
            full.gameObject.SetActive(true);
            StartCoroutine(LoadProcess());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator LoadProcess()
        {
            yield return StartCoroutine(FadeOut());
            yield return StartCoroutine(Loading(NextSceneName));
            //yield return FadeIn
        }

        IEnumerator FadeIn()
        {
            float a = 1f;
            Color origin = Color.black;
            while (0 < a)
            {
                a -= Time.unscaledDeltaTime / fadeTime;
                origin.a = a;
                back.color = origin;
                yield return null;
            }
            back.color = Color.clear;
        }
        IEnumerator FadeOut()
        {
            float a = 0f;
            Color origin = Color.clear;
            while (a < 1)
            {
                a += Time.unscaledDeltaTime / fadeTime;
                origin.a = a;
                back.color = origin;
                yield return null;
            }
            back.color = Color.black;
        }
        IEnumerator Loading(SceneName nextScene)
        {
            //loadingText.text = $"読込中";
            var asyncOperation = SceneManager.LoadSceneAsync(nextScene.ToString());
            asyncOperation.allowSceneActivation = false;
            loadAnimation.StartLoadingAnimation(ref asyncOperation);


            while (asyncOperation.progress < 0.9f)
            {
                float p = asyncOperation.progress;
                if (p < 0.5f)
                {
                    half.color = new Color(1, 1, 1, p * 2);
                }
                else
                {
                    half.color = Color.white;
                    full.color = new Color(1, 1, 1, (p - 0.5f) * 2);
                }
                gauge.fillAmount = p;
                //Debug.Log(asyncOperation.progress);
                //loadingText.text = $"{AsyncProgressToPercent(asyncOperation.progress)}% is loaded...";
                yield return null;
            }
            float a = asyncOperation.progress;
            while (a < 1f)
            {
                gauge.fillAmount = a;
                full.color = new Color(1, 1, 1, (a - 0.5f) * 2);
                a += Time.deltaTime;
                yield return null;
            }
            full.color = Color.white;
            half.gameObject.SetActive(false);
            asyncOperation.allowSceneActivation = true;

            SceneManager.UnloadSceneAsync("Loading");
        }
        int AsyncProgressToPercent(float progress)
        {
            if (progress == 0f) return 0;
            return (int)(progress / 0.009f);
        }
    }
}