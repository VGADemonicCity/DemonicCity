using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
namespace DemonicCity
{
    public class EyeAnimator : MonoBehaviour
    {
        [SerializeField] List<Image> eyes = new List<Image>();
        [SerializeField] Image flasher;
        BlurOptimized blur;
        BlurOptimized Blur
        {
            get
            {
                if (blur == null)
                {
                    blur = Camera.main.GetComponent<BlurOptimized>();
                }
                return blur;
            }
        }
        public void Init()
        {
            flasher.color = Color.clear;

        }
        // Use this for initialization
        void Start()
        {
            //Time.timeScale = 0f;
            //StartCoroutine(TestEye());
        }

        // Update is called once per frame
        void Update()
        {
            //if (isEnableInspector)
            //{
            //    FillChange(openCodition);
            //    blur.enabled = isEnableBlur;
            //    blur.downsample = downsample;
            //    blur.blurSize = blurSize;
            //    blur.blurIterations = blurIterations;
            //}
        }
        void FillChange(float amount)
        {
            eyes.ForEach(x => x.fillAmount = amount);
        }
        void BlurSizeChange(float size)
        {
            int iteration = size < 1f ? 0
                : size < 2f ? 1
                : size < 3f ? 2
                : size < 4f ? 3
                : 4;
            Blur.blurIterations = iteration;
            Blur.blurSize = size;
        }
        void AlphaChange(float alpha)
        {
            //Debug.Log(alpha);
            //flasher.color = new Color(flasher.color.r, flasher.color.g, flasher.color.b, alpha);
            flasher.color = new Color(1f, 1f, 1f, alpha);
        }

        IEnumerator TestEye()
        {
            yield return new WaitForSecondsRealtime(1f);

            //0.4から閉じ始める。
            //IEnumerator e = Close();
            //while (e.MoveNext()) yield return e.Current;
            yield return StartCoroutine(Close());

            //WaitTime
            yield return new WaitForSecondsRealtime(2f);

            StartCoroutine(Open());
        }

        public IEnumerator Open()
        {
            BlurSizeChange(5f);

            //ImageFade
            StartCoroutine(Translate(0f, 0.5f, 1f, AlphaChange));

            //FillTrans
            //StartCoroutine(Translate(1f, 0.9f, 1f, FillChange));

            yield return StartCoroutine(Translate(1f, 0.9f, 1f, FillChange));
            //IEnumerator e = Translate(1f, 0.9f, 1f, FillChange);
            //while (e.MoveNext())
            //{
            //    yield return e.Current;
            //}
            //yield return new WaitForSecondsRealtime(0.3f);
            StartCoroutine(Translate(0.5f, 1f, 0.3f, AlphaChange));
            StartCoroutine(Translate(0.9f, 0.4f, 0.3f, FillChange));
            yield return new WaitForSecondsRealtime(0.3f);

            //BlurTrans
            Blur.enabled = false;
            //StartCoroutine(Translate(0.5f, 0f, 0f, BlurSizeChange));

            //ImageFade
            //Debug.Log("End");
            yield return StartCoroutine(Translate(1f, 0f, 0.1f, AlphaChange));
        }
        public IEnumerator Close()
        {
            //BlurTrans
            Blur.enabled = true;
            StartCoroutine(Translate(0f, 5f, 0.9f, BlurSizeChange));

            //FillTrans
            //var e = Translate(0.4f, 0.8f, 1f, FillChange);
            //while (e.MoveNext()) yield return e.Current;
            yield return StartCoroutine(Translate(0.4f, 0.8f, 1f, FillChange));

            yield return new WaitForSecondsRealtime(0.5f);

            yield return StartCoroutine(Translate(0.8f, 1f, 0.8f, FillChange));
            BlurSizeChange(0f);
        }
        public IEnumerator Translate(float ori, float target, float time, Action<float> action)
        {
            //time *= 3;
            float diff = target - ori;
            float current = time != 0f ? ori : target;
            if (ori < target)
            {
                Debug.Log($"{time}:{action.Method.Name}");
                //加算
                while (current < target)
                {
                    action?.Invoke(current);
                    yield return null;
                    current += diff * Time.unscaledDeltaTime / time;
                }
            }
            else
            {
                //減算
                while (target < current)
                {
                    action?.Invoke(current);
                    yield return null;
                    current += diff * Time.unscaledDeltaTime / time;
                }
            }
            action?.Invoke(target);
        }
        IEnumerator FillTrans(float ori, float target, float time)
        {
            if (time == 0f)
            {
                FillChange(target);
                yield break;
            }
            float current = ori;
            if (ori < target)
            {
                //加算
                while (current < target)
                {
                    FillChange(current);
                    yield return null;
                    current += Time.unscaledDeltaTime;
                }
            }
            else
            {
                //減算
                while (target < current)
                {
                    FillChange(current);
                    yield return null;
                    current -= Time.unscaledDeltaTime;
                }
            }
            FillChange(target);

        }
    }
}