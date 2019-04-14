using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    using Skill = Magia.PassiveSkill;
    [System.Serializable]
    [CreateAssetMenu(fileName = "MagiaSkillSE", menuName = "Sounds/MagiaSkillSE")]
    public class SkillSEAsset : ScriptableObject
    {

        [System.Serializable]
        public class SkillSE
        {
            public Skill id;
            public AudioClip clip;
        }
        [SerializeField] List<SkillSE> magiaSkillSE = new List<SkillSE>();

        public Dictionary<Skill, AudioClip> MagiaSkillSE
        {
            get
            {
                Dictionary<Skill, AudioClip> tmp = new Dictionary<Skill, AudioClip>();
                foreach (var item in magiaSkillSE)
                {
                    tmp.Add(item.id, item.clip);
                }
                return tmp;
            }
        }

    }
}