using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public static class TouchPosition
    {

        public const int width = 1080;
        public const int height = 1920;
        public const double scale = 0.06014065f;

        public static Vector2 TouchToCanvas()
        {
            var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition + Camera.main.transform.forward * 10);
            pos = new Vector3(width * (float)(pos.x - 0.5), height * (float)(pos.y - 0.5), 0);

            return pos;
        }
        public static Vector2 TouchToRay()
        {
            var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            pos = new Vector3(width*(float)(pos.x - 0.5), height*(float)(pos.y - 0.5), 0);
            pos.x = (float)(pos.x * scale);
            pos.y = (float)(pos.y * scale);
            
            return pos;
        }
    }
    
}