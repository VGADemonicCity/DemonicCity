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
    public class PopupSystem : MonoBehaviour
    {
        public GameObject popupedObject { get; set; }

        public Canvas canvas { get; set; }
        GameObject canvasObject;

        [SerializeField] GameObject popupObject;
        [SerializeField] float scalingTime = 1f;
        [SerializeField] int sortingOrder = 100;

        readonly Vector2 targetResolution = new Vector2(1080, 1920);

        /// <summary>
        /// UIに配置されているボタンに対してイベントハンドラを登録する
        /// </summary>
        /// <param name="popupMaterial"></param>
        public void SubscribeButton(PopupSystemMaterial popupSystemMaterial)
        {
            var button = popupedObject.GetComponent<Button>();
            if (button == null)
            {
                var buttons = popupedObject.GetComponentsInChildren<Button>().ToList();
                button = buttons.Find(obj => obj.gameObject.name == popupSystemMaterial.ObjectName);
            }

            button.onClick.AddListener(() =>
            {
                popupSystemMaterial.EventHandler();
                if (popupSystemMaterial.IsPushAfterClose)
                {
                    Close();
                }
            });
        }

        /// <summary>
        /// UIに配置されているトグルに対してイベントハンドラを登録する
        /// </summary>
        /// <param name="popupMaterial"></param>
        public void SubscribeToggle(PopupSystemMaterial popupSystemMaterial)
        {
            var toggle = popupedObject.GetComponent<Toggle>();
            var toggles = popupedObject.GetComponentsInChildren<Toggle>().ToList();
            toggle = toggles.Find(obj => obj.gameObject.name == popupSystemMaterial.ObjectName);

            toggle.onValueChanged.AddListener(chagedValue =>
            {
                popupSystemMaterial.BoolEventHandler(chagedValue);
            });
        }


        /// <summary>
        /// UIに配置されているスライダーに対してイベントハンドラを登録する
        /// </summary>
        /// <param name="popupMaterial"></param>
        public void SubscribeSlider(PopupSystemMaterial popupSystemMaterial)
        {
            var slider = popupedObject.GetComponent<Slider>();
            var sliders = popupedObject.GetComponentsInChildren<Slider>().ToList();
            slider = sliders.Find(obj => obj.gameObject.name == popupSystemMaterial.ObjectName);

            slider.onValueChanged.AddListener(changedvalue =>
            {
                popupSystemMaterial.FloatEventHandler(changedvalue);
            });
        }

        /// <summary>
        /// Scene上にCanvasを作成
        /// </summary>
        void CreateCanvas()
        {
            // Create canvas
            canvasObject = new GameObject("PopupCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.AddComponent<CanvasScaler>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var canvasScaler = canvas.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = targetResolution;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            // 最前面に来る様適当なSortingOderに設定する
            canvas.sortingOrder = sortingOrder;
        }

        /// <summary>
        /// 引数のUIをポップアップさせる
        /// Popupさせるポジションは予めCanvasに配置してTransform情報を保持したPrefabから取得する
        /// </summary>
        public void Popup()
        {
            if (canvas == null)
            {
                CreateCanvas();
            }
            popupedObject = Instantiate(popupObject, canvas.transform);
            popupedObject.transform.localScale = new Vector3(1f, 0f, 1f);
            iTween.ScaleTo(popupedObject, iTween.Hash("scale", Vector3.one, "time", scalingTime,"ignoretimescale",true));
        }

        /// <summary>
        /// popupしたオブジェクトを閉じた後削除
        /// </summary>
        public void Close()
        {
            Debug.Log("CloseButtonがpushされた");
            Destroy(canvasObject);
        }
    }
}