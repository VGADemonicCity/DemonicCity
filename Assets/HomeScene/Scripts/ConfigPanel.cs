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


        public void OnPush()
        {
            popupSystem.Popup();
            popupSystem.SubscribeButton(new PopupSystemMaterial(DataReset, "PositiveButton", true));
            popupSystem.SubscribeButton(new PopupSystemMaterial(Cancel, "NegativeButton", true));
        }

        void DataReset()
        {
            Debug.Log("DataReset");
            //データをリセットする処理
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Title);
        }

        void Cancel()
        {

        }
    }
}