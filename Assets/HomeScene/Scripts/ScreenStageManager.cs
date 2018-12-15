using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenStageManager : MonoBehaviour {




    IEnumerator ScreenFade()
    {
        GameObject fadePanel = null;
        fadePanel.AddComponent<RectTransform>();
        fadePanel.AddComponent<Image>();

        GameObject newPanel = Instantiate(fadePanel, transform.parent);

        return null;
    }
}
