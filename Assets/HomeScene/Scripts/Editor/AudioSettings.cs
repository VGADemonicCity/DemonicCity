using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DemonicCity
{
    public class AudioSettings
    {

        [MenuItem("AudioSetting/InBackGround")]
        public static void LoadTypeToInBackGround()
        {
            foreach (var item in AssetDatabase.FindAssets($"t:{nameof(AudioClip)}"))
            {
                AudioImporter audioImporter = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(item)) as AudioImporter;
                if (audioImporter == null) return;
                audioImporter.loadInBackground = true;
            }
        }


    }
}