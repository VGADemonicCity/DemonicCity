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
        int listCount;
        float fadeTime = 0.5f;

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
                a -= Time.deltaTime / fadeTime;
                yield return null;
            }
            magia.color = Color.clear;
            //asyncOperation.allowSceneActivation = true;
        }
    }
}