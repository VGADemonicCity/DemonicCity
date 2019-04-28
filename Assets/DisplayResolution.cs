using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayResolution : MonoBehaviour
{
    private void Update()
    {
        var textBox = GetComponent<Text>();
        textBox.text = string.Format("x = {0}, y = {1}", Screen.width, Screen.height);
    }
}
