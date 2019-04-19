using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DemonicCity
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SoundData", menuName = "CreSounds")]
    public class SoundAsset : ScriptableObject
    {
        public enum BGMTag
        {
            Title,
            Home,
            BattleAStd,
            BattleABoss,
            BattleBStd,
            BattleBBoss,
            BattleCStd,
            BattleCBoss,
            ThemaInst,
        }

        public enum SETag
        {
            BeforeOpenPanel,
            AfterOpenedPanel,
            AfterOpenedEnemeyPanel,
            PositiveButton,
            NegativeButton,
        }

        public enum TutorialTag
        {
            Home,
            Battle,
            Growth,
        }
        public enum VoiceTag
        {
            Before,
            After,
        }

        public SoundManager.SoundTag tag;
        public AudioSource[] audioSource;

    }



    //[System.Serializable]
    //public class Sounds<T> : ScriptableObject
    //{
    //    [System.Serializable]
    //    public class Source
    //    {
    //        public T tag;
    //        public List<AudioClip> clips = new List<AudioClip>();
    //    }

    //    public List<Source> sources = new List<Source>();

    //    public List<AudioClip> GetClips(T tag)
    //    {
    //        Source tmp = sources.FirstOrDefault(x => x.tag.ToString() == tag.ToString());
    //        if (tmp == null)
    //        {
    //            return null;
    //        }
    //        return tmp.clips;
    //    }

    //}


    //[System.Serializable]
    //[CreateAssetMenu(fileName = "BGMSources", menuName = "SoundAssets/BGMSources")]
    //public class BGMs : Sounds<SoundAsset.BGMTag>
    //{

    //}

    //[System.Serializable]
    //[CreateAssetMenu(fileName = "SESources", menuName = "SoundAssets/SESources")]
    //public class SEs : Sounds<SoundAsset.SETag>
    //{

    //}



    //[System.Serializable]
    //[CreateAssetMenu(fileName = "BGMSources", menuName = "SoundAssets/BGMSources")]
    //public class BGMAssets : ScriptableObject
    //{

    //    public class BGMSouce
    //    {
    //        public BGMTag tag;
    //        public List<AudioClip> clips = new List<AudioClip>();
    //    }

    //    public List<BGMSouce> BGMs = new List<BGMSouce>();
    //}

    //[System.Serializable]
    //[CreateAssetMenu(fileName = "BGMSources", menuName = "SoundAssets/SESources")]
    //public class SEAssets : ScriptableObject
    //{
    //    public enum SETag
    //    {
    //        A,
    //        B,
    //        C,
    //    }
    //    public class SESouce
    //    {
    //        public SETag tag;
    //        public List<AudioClip> clips = new List<AudioClip>();
    //    }

    //    public List<SESouce> SEs = new List<SESouce>();
    //}

}