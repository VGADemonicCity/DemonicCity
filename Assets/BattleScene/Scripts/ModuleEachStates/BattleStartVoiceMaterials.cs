using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "BattleStartVoiceMaterial", menuName = "BattleScene/BattleStartVoiceMaterials")]
    public class BattleStartVoiceMaterials : ScriptableObject
    {
        public List<BattleStartVoiceMaterial> Materials;

        [System.Serializable]
        public class BattleStartVoiceMaterial
        {
            public AudioClip VoiceClip;
            public Progress.StoryProgress StoryIdentifier;
        }
    }
}
