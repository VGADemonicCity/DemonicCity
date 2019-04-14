using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StoryScene
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ScenarioData", menuName = "StoryAssets")]
    public class Scenario : ScriptableObject
    {
        public List<TextStorage> texts = new List<TextStorage>();
        public List<CharName> characters = new List<CharName>();

        public Scenario()
        {
        }
        public Scenario(Scenario parent)
        {
            texts = parent.texts;
            characters = parent.characters;
        }
    }
}