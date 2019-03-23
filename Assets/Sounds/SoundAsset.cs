using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SoundData", menuName = "CreSounds")]
    public class SoundAsset : ScriptableObject
    {

        [System.Serializable]
        public class Sound
        {
            public AudioClip[] clips;

        }

        public SoundManager.SoundTag tag;
        public List<Sound> sounds = new List<Sound>();
        public AudioSource[] audioSource;

    }
}