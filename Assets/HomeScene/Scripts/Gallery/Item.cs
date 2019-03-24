using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    [System.Serializable]
    [CreateAssetMenu(fileName ="ItemData",menuName ="Gallery/Item")]
    public class Item : ScriptableObject
    {
        public GalleryContent.ItemTag tag;
        public string id;
        public string name;
        [Multiline(5)] public string text;
        public Progress.StoryProgress UnLockStory;
    }
}