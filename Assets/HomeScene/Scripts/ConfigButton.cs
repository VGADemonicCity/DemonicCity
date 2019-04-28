using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity
{
    using SoundTag = SoundManager.SoundTag;
    public class ConfigButton : MonoBehaviour
    {

        [SerializeField] SoundTag tag;

        [SerializeField] Button[] btn;

        SoundManager soundM;

        SoundManager.Vols vols;
        GameObject beginObject;

        void Awake()
        {
            soundM = SoundManager.Instance;
        }
        // Use this for initialization
        void Start()
        {
            vols = soundM.CurrentVol;

            Select(vols.vol[(int)tag]);
        }

        // Update is called once per frame
        void Update()
        {

        }
        void Select(bool isOn)
        {
            btn[0].interactable = isOn;
            btn[1].interactable = !isOn;
        }
        public void Submit(bool isOn)
        {
            Select(isOn);
            //soundM.LoadVol();
            soundM.SwitchVol(tag, isOn);
            soundM.SaveVol();
        }

    }
}