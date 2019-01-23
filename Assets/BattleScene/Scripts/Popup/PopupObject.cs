using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DemonicCity.BattleScene
{
    public abstract class PopupObject : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] protected PopupSystem popupSystem;

        protected virtual void OnEnable()
        {
            if (!popupSystem)
            {
                popupSystem = GetComponentInParent<PopupSystem>();
                if (!popupSystem)
                {
                    Debug.LogError("Failed to get reference of PopupSystem Component.");
                }
            }
        }

        protected abstract void OnDisable();
        public abstract void OnPointerDown(PointerEventData eventData) ;
    }
}

