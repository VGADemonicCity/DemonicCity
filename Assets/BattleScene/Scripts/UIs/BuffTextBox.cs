using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.Battle
{
    public class BuffTextBox : MonoBehaviour
    {
        [SerializeField] float fadingTime = .25f;

        TypefaceAnimator typeFaceAnimator;
        Outline outline;
        Text buffTextBox;

        private void Awake()
        {
            typeFaceAnimator = GetComponent<TypefaceAnimator>();
            outline = GetComponent<Outline>();
            buffTextBox = GetComponent<Text>();
            buffTextBox.color = Color.clear;
        }

        private void Start()
        {
            typeFaceAnimator.onComplete.AddListener(() =>
            {
                //buffTextBox.color = Color.clear;
                iTween.ValueTo(gameObject, iTween.Hash("from", Color.white, "to", Color.clear, "time", fadingTime, "onupdate", "NextColorChange", "onupdatetarget", gameObject));
            });
        }
        
        void NextColorChange(Color nextColor)
        {
            buffTextBox.color = nextColor;
        }

        public void PlayText(string text)
        {
            buffTextBox.text = text;
            buffTextBox.color = Color.white;
            typeFaceAnimator.Play();
        }

        public void SyncSettings(Skill.EnhanceType enhanceType)
        {
            switch (enhanceType)
            {
                case Skill.EnhanceType.AttackBuff:
                    outline.effectColor = Color.red;
                    break;
                case Skill.EnhanceType.DefenseBuff:
                    outline.effectColor = Color.blue;
                    break;
                case Skill.EnhanceType.HpBuff:
                    outline.effectColor = Color.green;
                    break;
                case Skill.EnhanceType.AttackAndDefenseBuff:
                    outline.effectColor = Color.yellow;
                    break;
                case Skill.EnhanceType.Invalid:
                default:
                    Debug.Log("Invalid type.");
                    break;
            }
        }
    }
}
