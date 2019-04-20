using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    public class TeleportEffectPlayer : MonoBehaviour
    {
        public void PlayEffectSE()
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
