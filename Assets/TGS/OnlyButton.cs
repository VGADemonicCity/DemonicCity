using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity
{
    public class OnlyButton : MonoBehaviour
    {
        [SerializeField] Button target;
        public void OnlyInteractable()
        {
            target.interactable = false;
        }
        public void OnlyActive()
        {
            target.gameObject.SetActive(false);
        }
    }
}