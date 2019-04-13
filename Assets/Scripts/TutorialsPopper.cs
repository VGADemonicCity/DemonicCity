using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity
{
    public class TutorialsPopper : MonoBehaviour
    {
        /// <summary>表示させるチュートリアルの素材群</summary>
        [SerializeField] List<TutorialItems> items;
        /// <summary>popup system</summary>
        [SerializeField] PopupSystem popupSystem;

        /// <summary>現在対象となっている素材</summary>
        TutorialItems currentItem;
        
        void Popup()
        {

        }



        void ToNextTexture()
        {

        }

        bool HasNextTexture()
        {



            return true;
        }
    }
}
