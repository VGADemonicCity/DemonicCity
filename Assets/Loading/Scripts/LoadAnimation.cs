using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DemonicCity.StoryScene;
using System.Linq;

namespace DemonicCity.Loading
{
    public class LoadAnimation : MonoBehaviour
    {
        [SerializeField] SpriteRenderer magia;
        [SerializeField] List<Sprite> magiaAnims = new List<Sprite>();
        [SerializeField] TextActor magiaSource;
        [SerializeField] Text loadingText;
        [SerializeField] string incString;
        int listCount;
        [SerializeField]
        float fadeTime = 0.5f;
        Coroutine textCoroutine = null;
        // Use this for initialization
        void Start()
        {
            magia.enabled = false;
            magiaAnims = magiaSource.faces.ToList();
            listCount = magiaAnims.Count;
            //StartCoroutine(AsyncLoad(SceneFader.SceneTitle.Title));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Coroutine StartLoadingAnimation(ref AsyncOperation asyncOperation)
        {
            textCoroutine = StartCoroutine(Period());
            return StartCoroutine(AsyncLoad(asyncOperation));
        }

        IEnumerator AsyncLoad(AsyncOperation asyncOperation)
        {
            magia.enabled = true;
            //var asyncOperation = SceneManager.LoadSceneAsync(title.ToString());
            //asyncOperation.allowSceneActivation = false;
            int i = 0;
            int c = 0;
            while (asyncOperation.progress < 0.9f || i != listCount)
            {
                Debug.Log(asyncOperation.progress);
                if (listCount <= i) { i = 0; c = 1; }
                //現在のロード進捗が取得できます。
                //text.text = asyncOperation.progress.ToString();
                magia.sprite = magiaAnims[i];
                i = c++ / 8;
                yield return null;
            }
            float a = 1f;
            while (0 < a)
            {
                magia.color = new Color(1, 1, 1, a);
                a -= Time.deltaTime * fadeTime;
                yield return null;
            }
            magia.color = Color.clear;
            //asyncOperation.allowSceneActivation = true;
        }
        IEnumerator Period()
        {
            int i = 0;
            while (true)
            {
                loadingText.text = $"読込中{GetPeriod(i++)}";
                yield return new WaitForSecondsRealtime(0.5f);
                if (8 <= i) i = 0;
            }
        }
        string GetPeriod(int num)
        {
            string ret = incString;
            for (int i = 1; i < num; i++)
            {
                if (i % 2 == 0) ret += incString;
            }
            return ret;
        }
        void OnDestroy()
        {
            Debug.Log("Destroy");
            if (textCoroutine != null)
            {
                StopCoroutine(textCoroutine);
            }
        }
    }
}