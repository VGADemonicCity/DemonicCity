using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DemonicCity
{
    /// <summary>
    /// popupさせたいオブジェクトの呼び出し側にこのscriptをアタッチ
    /// inspectorにpopupさせたいオブジェクト(UI)とその親となるcanvas,
    /// </summary>
    public class PopupWindowSystem : MonoBehaviour
    {
        [SerializeField] GameObject popupWindow;
        [SerializeField] Canvas canvas;
        [SerializeField] float scalingTime;

        GameObject targetObject;

        /// <summary>
        /// Popup windowの中にあるボタンを登録する
        /// </summary>
        /// <param name="button"></param>
        /// <param name="action"></param>
        /// <param name="closable"></param>
        public void SubscribeButtons(UnityAction action, bool closable, Specification specification)
        {
            var buttons = targetObject.GetComponentsInChildren<Button>().ToList();
            var button = buttons.Find(obj => obj.gameObject.name == specification.ToString());

            button.onClick.AddListener(() =>
            {
                action();
                if (closable)
                {
                    Close();
                }
            });
        }

        public void SubscribeButtons(PopupMaterial popupMaterial)
        {
            var buttons = targetObject.GetComponentsInChildren<Button>().ToList();
            var button = buttons.Find(obj => obj.gameObject.name == popupMaterial.Specification.ToString());

        }

        void Initialize()
        {
            canvas = gameObject.AddComponent<Canvas>();
            
        }

        public void Popup()
        {
            if (canvas == null)
            {
                Initialize();
            }
            targetObject = Instantiate(popupWindow, canvas.transform);
            targetObject.transform.localScale = new Vector3(1f,0f,1f);
            iTween.ScaleTo(targetObject, iTween.Hash("scale", Vector3.one, "time", scalingTime));
        }

        public void Close()
        {
            Destroy(targetObject);
        }
    }
    public enum Specification
    {
        PositiveButton,
        NegativeButton,
    }
}