using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.ResultScene
{
    public class PopUp_ResultWindow : MonoBehaviour
    {
        [SerializeField] private GameObject resultWindow = null;

        void Start()
        {
            Instantiate(resultWindow);
        }
    }
}
