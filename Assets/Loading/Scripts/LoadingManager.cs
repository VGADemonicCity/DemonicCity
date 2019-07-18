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
        [SerializeField] Text loadingText;
        [SerializeField] Image back;
        float fadeTime = 0.5f;
        public static SceneName NextSceneName { get; set; } = SceneName.Title;
        // Use this for initialization
        void Start()
        {

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
                a -= Time.unscaledDeltaTime / fadeTime:
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
                a += Time.unscaledDeltaTime / fadeTime:
                origin.a = a;
                back.color = origin;
                yield return null;
            }
            back.color = Color.black;
        }
        IEnumerator Loading(SceneName nextScene)
        {
            loadingText.text = $"0% is loaded...";
            var asyncOperation = SceneManager.LoadSceneAsync(nextScene.ToString());
            asyncOperation.allowSceneActivation = false;
            loadAnimation.StartLoadingAnimation();
            while (asyncOperation.progress < 0.9f)
            {
                //Debug.Log(asyncOperation.progress);
                loadingText.text = $"{asyncOperation.progress}% is loaded...";
                yield return null;
            }
            asyncOperation.allowSceneActivation = true;

            SceneManager.UnloadSceneAsync("LoadingScene");
        }

    }
}