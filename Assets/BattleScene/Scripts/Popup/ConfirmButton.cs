using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DemonicCity.BattleScene
{
    public class ConfirmButton : PopupObject
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            popupSystem.Confirm += Agree;
        }

        protected override void OnDisable()
        {
            popupSystem.Confirm -= Agree;
        }

        bool Agree()
        {
            return true;
        }
    }
}
