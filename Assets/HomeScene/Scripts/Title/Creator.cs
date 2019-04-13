using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DemonicCity
{
    [System.Serializable]
    [CreateAssetMenu(fileName ="Creators",menuName ="Credit/Creator")]
    public class Creators : ScriptableObject
    {
        [System.Serializable]
        public struct Creator
        {
            public string role;
            public string name;
        }

        public List<Creator> creators = new List<Creator>();
    }
}