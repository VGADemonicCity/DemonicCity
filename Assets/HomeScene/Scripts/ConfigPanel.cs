using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity
{
    using SoundTag = SoundManager.SoundTag;
    public class ConfigPanel : MonoBehaviour
    {
        [SerializeField] PopupSystem popupSystem;
        [SerializeField] Button positiveButton;
        [SerializeField] Button negativeButton;
        [SerializeField] Button Btn;

        void Start()
        {
            if (Btn == null)
            {
                Btn = GetComponent<Button>();
            }
        }
        public void OnPush()
        {
            Btn.interactable = false;
            popupSystem.Popup();
            popupSystem.SubscribeButton(new PopupSystemMaterial(DataReset, "PositiveButton", true));
            popupSystem.SubscribeButton(new PopupSystemMaterial(Cancel, "NegativeButton", true));
        }

        void DataReset()
        {
            Debug.Log("DataReset");
            Progress.Instance.Clear();
            Magia.Instance.Clear();
            ChapterManager.Instance.ProgressReset();
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Title);
        }

        void Cancel()
        {
            Btn.interactable = true;
        }
    }
}