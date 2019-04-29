using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonicCity
{
    public class ToTitle : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Button btn;


        void Awake()
        {
            if (SceneFader.SceneTitle.Title.ToString() == SceneManager.GetActiveScene().name)
            {
                gameObject.SetActive(false);
            }
        }

        public void ToTitleScene()
        {
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Title);
            btn.interactable = false;
        }
    }
}