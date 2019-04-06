using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SEAsset", menuName = "Sounds/SE")]
    public class SEAsset : ScriptableObject
    {
        [System.Serializable]
        public class Source
        {
            public SoundAsset.SETag tag;
            public List<AudioClip> clips = new List<AudioClip>();
        }

        public List<Source> sources = new List<Source>();

        public List<AudioClip> GetClips(SoundAsset.SETag tag)
        {
            Source tmp = sources.FirstOrDefault(x => x.tag.ToString() == tag.ToString());
            if (tmp == null)
            {
                return null;
            }
            return tmp.clips;
        }
        public AudioClip GetClip(SoundAsset.SETag tag)
        {
            List<AudioClip> tmp = GetClips(tag);
            return tmp[Random.Range(0, tmp.Count)];
        }
    }
}