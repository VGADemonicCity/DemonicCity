using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DemonicCity.BattleScene
{
    public class CancelButton : PopupObject
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            popupSystem.Confirm += Cancel;
        }

        protected override void OnDisable()
        {
            popupSystem.Confirm -= Cancel;
        }

        bool Cancel()
        {
            return false;
         }
    }
}