using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(250, 0);
    }

    void Update()
    {
        rectTransform.sizeDelta += new Vector2(0, 1);

        if (rectTransform.sizeDelta.y >= 150)
        {
            rectTransform.sizeDelta = new Vector2(250, 150);
        }
    }
}
