using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "MagiaSkillAudioMaterial", menuName = "BattleScene/MagiaSkillAudioMaterials")]
    public class MagiaSkillAudioMaterials : ScriptableObject
    {
        public List<MagiaSkillAudioMaterial> Materials;

        [System.Serializable]
        public class MagiaSkillAudioMaterial
        {
            public AudioClip VoiceClip;
            public AudioClip SEClip;
            public Magia.PassiveSkill skillTag;
        }
    }
}
