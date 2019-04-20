using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DemonicCity.BattleScene
{
    public class MagiaSkillPlayer : MonoBehaviour
    {
        Animator animator;
        AudioSource audioSource;
        // Scriptable object
        [SerializeField] List<AudioClip> magiaSkillClips;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        public void Play(Magia.PassiveSkill skill)
        {
            var targetClip = magiaSkillClips.Find(clip => clip.name == skill.ToString());
            // skillのenumに応じてclipを切り替え,再生する
            audioSource.PlayOneShot(targetClip);
        }
    }
}