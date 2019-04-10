using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity
{
    public class PopupSystemMaterial
    {
        public Action EventHandler { get; set; }
        public Action<bool> ToggleEventHandler { get; set; }
        public bool IsPushAfterClose { get; set; }
        public string ObjectName { get; set; }

        public PopupSystemMaterial(Action eventHandler, string buttonName, bool isPushAfterClose)
        {
            EventHandler = eventHandler;
            IsPushAfterClose = isPushAfterClose;
            ObjectName = buttonName;
        }

        public PopupSystemMaterial(Action<bool> eventHandler,string toggleName)
        {
            ToggleEventHandler = eventHandler;
            ObjectName = toggleName;
        }
    }
}
