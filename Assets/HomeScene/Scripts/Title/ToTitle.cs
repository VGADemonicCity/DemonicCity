using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class ToTitle : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Button btn;
        public void ToTitleScene()
        {
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Title);
            btn.interactable = false;
        }
    }
}