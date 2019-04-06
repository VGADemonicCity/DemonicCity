using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class PanelCompleteCutIn : MonoBehaviour
    {
        [SerializeField] AnimationClip clip;
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Debug.Log(clip.ToString());
        }

        public float PlayAnimation()
        {
            animator.CrossFadeInFixedTime(clip.ToString(), 0f);
            return clip.length;
        }
    }
}