using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownLayer : MonoBehaviour {

    
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas)
        {
            canvas.sortingLayerName = "TouchEffect";
            canvas.sortingOrder = 10000;
        }
    }
}
