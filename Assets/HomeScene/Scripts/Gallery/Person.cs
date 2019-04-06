using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "PersonalData", menuName = "Gallery/Person")]
    public class Person : Item
    {
        public string actor;
        public List<AudioClip> voice = new List<AudioClip>();
    }
}