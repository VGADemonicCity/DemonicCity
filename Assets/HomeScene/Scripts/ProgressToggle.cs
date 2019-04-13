using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity
{
    public class ProgressToggle : MonoBehaviour
    {
        [SerializeField] Toggle toggle;
        [SerializeField] Text text;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(ProgressPanel parent, Progress.StoryProgress story, bool state, string title)
        {
            toggle.isOn = state;
            text.text = title;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                parent.ToggleChanged(story, isOn);
            });
        }
    }
}