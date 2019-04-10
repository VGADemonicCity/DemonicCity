using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class BuffTextBox : MonoBehaviour
    {
        [SerializeField] float fadingTime = .5f;

        TypefaceAnimator typeFaceAnimator;
        Outline outline;
        Text buffTextBox;

        private void Awake()
        {
            typeFaceAnimator = GetComponent<TypefaceAnimator>();
            outline = GetComponent<Outline>();
            buffTextBox = GetComponent<Text>();
            buffTextBox.color = Color.clear;


            typeFaceAnimator.onComplete.AddListener(() =>
            {
                Debug.Log("called");
                iTween.ColorTo(gameObject, iTween.Hash("color", Color.clear, "time", fadingTime));
            });
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
                case Skill.EnhanceType.Invalid:
                default:
                    Debug.Log("Invalid type.");
                    break;
            }
        }
    }
}
