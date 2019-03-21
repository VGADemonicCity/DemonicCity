using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity
{
    public class PopupSystemMaterial
    {
        public Action EventHandler { get; set; }
        public bool IsPushAfterClose { get; set; }
        public string ButtonName { get; set; }

        public PopupSystemMaterial(Action eventHandler, string buttonName, bool isPushAfterClose)
        {
            EventHandler = eventHandler;
            IsPushAfterClose = isPushAfterClose;
            ButtonName = buttonName;
        }
    }
}
