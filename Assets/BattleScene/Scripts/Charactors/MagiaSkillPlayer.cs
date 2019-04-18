using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class MagiaSkillPlayer : MonoBehaviour
    {
        Animator animator;
        AudioSource audioSource;
        // Scriptable object

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        public void Play(Magia.PassiveSkill skill)
        {
            // skillのenumに応じてclipを切り替え,再生する
            //audioSource.PlayOneShot(clip);
        }
    }
}