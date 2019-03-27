using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTextAnimation : MonoBehaviour
{
    Animator textAnimator;

    public enum AnimationClip
    {
        TopToBottom
    }

    void Start()
    {
        textAnimator = GetComponent<Animator>();
    }

    /// <summary>レベルアップ時のテキストアニメーション</summary>
    /// <param name="animationClip"></param>
    public void TopToBottomAnimation(AnimationClip animationClip)
    {
        textAnimator.CrossFadeInFixedTime(animationClip.ToString(), 0, 0);
    }
}
