using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity
{
    public class LogText : MonoBehaviour
    {
        [SerializeField] Text text;


        public void Initialize(string _text, string _trace, LogType _type)
        {
            text.text = _type.ToString() + " : " + _text + '\n' + _trace;
        }

    }
}