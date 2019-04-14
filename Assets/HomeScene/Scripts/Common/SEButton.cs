using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class SEButton : MonoBehaviour
    {
        public bool isPositive;
        public void OnTouched()
        {
            CheckManager();
            if (isPositive)
            {
                soundM.PlayWithFade(SoundAsset.SETag.PositiveButton);
            }
            else
            {
                soundM.PlayWithFade(SoundAsset.SETag.NegativeButton);
            }
        }
        SoundManager soundM = null;
        void CheckManager()
        {
            if (soundM == null)
            {
                soundM = SoundManager.Instance;
            }
        }

    }
}