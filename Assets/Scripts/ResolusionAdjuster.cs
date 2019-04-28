using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonicCity
{
    public class ResolusionAdjuster : MonoBehaviour
    {
        [SerializeField] float width = 1080;
        [SerializeField] float height = 1920;

        Camera targetCamera;
        float targetAspect;
        float currentAspect;
        float ratio;

        private void Awake()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                targetCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            };
        }

        private void Update()
        {
            targetAspect = width / height;
            currentAspect = Screen.width / Screen.height;
            ratio = currentAspect / targetAspect;

            if (1f > ratio)
            {
                targetCamera.rect = new Rect
                {
                    x = 0f,
                    y = (1 - ratio) / 2f,
                    width = 1f,
                    height = ratio,
                };
            }
            else
            {
                ratio = 1f / ratio;
                targetCamera.rect = new Rect
                {
                    x = (1f - ratio) / 2f,
                    width = ratio,
                    y = 0f,
                    height = 1f,
                };
            }
        }
    }
}
