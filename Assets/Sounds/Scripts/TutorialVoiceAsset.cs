using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "TutorialAsset", menuName = "Sounds/Tutorial")]
    public class TutorialVoiceAsset : ScriptableObject
    {
        [System.Serializable]
        public class Source
        {
            public SoundAsset.TutorialTag tag;
            public List<AudioClip> clips = new List<AudioClip>();
        }

        public List<Source> sources = new List<Source>();

        public List<AudioClip> GetClips(SoundAsset.TutorialTag tag)
        {
            Source tmp = sources.FirstOrDefault(x => x.tag.ToString() == tag.ToString());
            if (tmp == null)
            {
                return null;
            }
            return tmp.clips;
        }
        public AudioClip GetClip(SoundAsset.TutorialTag tag, int index)
        {
            List<AudioClip> tmp = GetClips(tag);
            return tmp[index];
        }
    }
}