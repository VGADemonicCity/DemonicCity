using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity
{
    public class PopupMaterial
    {
        public Button Button { get; set; }
        public Action Action { get; set; }
        public bool Closable { get; set; }
        public Specification Specification { get; set; }

        public PopupMaterial(Button button,Action action,bool closable,Specification specification)
        {
            Button = button;
            Action = action;
            Closable = closable;
            Specification = specification;
        }
    }
}
