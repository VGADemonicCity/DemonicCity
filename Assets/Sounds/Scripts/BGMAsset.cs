using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "BGMAsset", menuName = "Sounds/BGM")]
    public class BGMAsset : ScriptableObject
    {
        [System.Serializable]
        public class Source
        {
            public SoundAsset.BGMTag tag;
            public List<AudioClip> clips = new List<AudioClip>();
        }

        public List<Source> sources = new List<Source>();

        public List<AudioClip> GetClips(SoundAsset.BGMTag tag)
        {
            Source tmp = sources.FirstOrDefault(x => x.tag.ToString() == tag.ToString());
            if (tmp == null)
            {
                return null;
            }
            return tmp.clips;
        }
        public AudioClip GetClip(SoundAsset.BGMTag tag)
        {
            List<AudioClip> tmp = GetClips(tag);
            return tmp[Random.Range(0, tmp.Count)];
        }
    }
}