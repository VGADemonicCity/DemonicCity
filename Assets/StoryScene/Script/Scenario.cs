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

    }
}