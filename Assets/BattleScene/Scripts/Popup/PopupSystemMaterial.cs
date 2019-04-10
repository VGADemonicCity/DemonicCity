using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity
{
    public class PopupSystemMaterial
    {
        public Action EventHandler { get; set; }
        public Action<bool> BoolEventHandler { get; set; }
        public Action<float> FloatEventHandler { get; set; }
        public bool IsPushAfterClose { get; set; }
        public string ObjectName { get; set; }

        public PopupSystemMaterial(Action eventHandler, string name, bool isProcessingAfterClose)
        {
            EventHandler = eventHandler;
            IsPushAfterClose = isProcessingAfterClose;
            ObjectName = name;
        }

        public PopupSystemMaterial(Action<bool> eventHandler,string name)
        {
            BoolEventHandler = eventHandler;
            ObjectName = name;
        }

        public PopupSystemMaterial(Action<float> eventHandler,string name)
        {
            FloatEventHandler = eventHandler;
            ObjectName = name;
        }
    }
}
