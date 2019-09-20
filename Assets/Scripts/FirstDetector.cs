using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class FirstDetector : MonoBehaviour
    {
        string keyName = "init";
        void Awake()
        {
            if (PlayerPrefs.HasKey(keyName) && PlayerPrefs.GetInt(keyName) == 1) return;
            DataReset();
            PlayerPrefs.SetInt(keyName, 1);
            PlayerPrefs.Save();
        }
        void DataReset()
        {
            Progress.Instance.Clear();
            Magia.Instance.Clear();
            ChapterManager.Instance.ProgressReset();
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Title);
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}