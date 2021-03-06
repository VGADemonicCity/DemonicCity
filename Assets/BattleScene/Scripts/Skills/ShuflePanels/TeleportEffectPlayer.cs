﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    public class TeleportEffectPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip clip;
        public void PlayEffectSE()
        {
            SoundManager.Instance.PlayWithFade(SoundManager.SoundTag.SE, clip);
        }
    }
}
