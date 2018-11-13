using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Touch gesture detector for Unity.
/// </summary>
/// <author>KAKO Akihito</author>
/// <email>kako@qnote.co.jp</email>
/// <license>MIT License</license>
public class TouchGestureDetector : MonoBehaviour
{
    private const float FLICK_TIME_LIMIT = 0.3f;

    public enum Gesture
    {
        TouchBegin,
        TouchMove,
        TouchStationary,
        TouchEnd,
        Click,
        FlickTopToBottom,
        FlickBottomToTop,
        FlickLeftToRight,
        FlickRightToLeft,
    }

    public Camera shootingCamera;
    public bool hitCheck = true;
    public bool detectFlick = true;
    public GestureDetectorEvent onGestureDetected = new GestureDetectorEvent();
    private List<TouchInfo> touchInfos = new List<TouchInfo>();

    private void Awake()
    {
        if (null == shootingCamera)
        {
            shootingCamera = Camera.main;
        }
    }

    private void Update()
    {
        foreach (var touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegin(touch.fingerId, touch.position);
                    break;
                case TouchPhase.Moved:
                    OnTouchMove(touch.fingerId, touch.position);
                    break;
                case TouchPhase.Stationary:
                    OnTouchStationary(touch.fingerId);
                    break;
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    OnTouchEnd(touch.fingerId, touch.position);
                    break;
            }
        }
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchBegin(Int32.MaxValue, Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnTouchEnd(Int32.MaxValue, Input.mousePosition);
            }
            else
            {
                OnTouchMove(Int32.MaxValue, Input.mousePosition);
            }
        }
    }

    private void OnTouchBegin(int fingerId, Vector2 position)
    {
        var touchInfo = new TouchInfo(fingerId, position);
        if (!hitCheck || touchInfo.IsHit(gameObject, shootingCamera))
        {
            touchInfos.Add(touchInfo);
            OnGestureDetected(Gesture.TouchBegin, touchInfo);
        }
    }

    private void OnTouchMove(int fingerId, Vector2 position)
    {
        var touchInfo = touchInfos.FirstOrDefault(x => x.fingerId == fingerId);
        if (null == touchInfo)
        {
            return;
        }
        touchInfo.AddPosition(position);
        OnGestureDetected(Gesture.TouchMove, touchInfo);
    }

    private void OnTouchStationary(int fingerId)
    {
        var touchInfo = touchInfos.FirstOrDefault(x => x.fingerId == fingerId);
        if (null == touchInfo)
        {
            return;
        }
        OnGestureDetected(Gesture.TouchStationary, touchInfo);
    }

    private void OnTouchEnd(int fingerId, Vector2 position)
    {
        var touchInfo = touchInfos.FirstOrDefault(x => x.fingerId == fingerId);
        if (null == touchInfo)
        {
            return;
        }
        touchInfo.AddPosition(position);
        OnGestureDetected(Gesture.TouchEnd, touchInfo);

        var diff = touchInfo.Diff;
        var flickDistanceLimit = (float)Math.Min(Screen.width, Screen.height) / 10;
        if (detectFlick && touchInfo.ElapsedTime < FLICK_TIME_LIMIT && (Mathf.Abs(diff.x) > flickDistanceLimit || Mathf.Abs(diff.y) > flickDistanceLimit))
        {
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                if (diff.x < 0f)
                {
                    OnGestureDetected(Gesture.FlickRightToLeft, touchInfo);
                }
                else
                {
                    OnGestureDetected(Gesture.FlickLeftToRight, touchInfo);
                }
            }
            else
            {
                if (diff.y < 0f)
                {
                    OnGestureDetected(Gesture.FlickTopToBottom, touchInfo);
                }
                else
                {
                    OnGestureDetected(Gesture.FlickBottomToTop, touchInfo);
                }
            }
        }
        else if (hitCheck && touchInfo.IsHit(gameObject, shootingCamera))
        {
            OnGestureDetected(Gesture.Click, touchInfo);
        }

        touchInfos.RemoveAll(x => x.fingerId == fingerId);
    }

    private void OnGestureDetected(Gesture gesture, TouchInfo touchInfo)
    {
        onGestureDetected.Invoke(gesture, touchInfo);
    }

    public class TouchInfo
    {
        public Vector2 Diff
        {
            get
            {
                return new Vector2(positions.Last().x - positions.First().x, positions.Last().y - positions.First().y);
            }
        }

        public float ElapsedTime
        {
            get
            {
                return Time.realtimeSinceStartup - startTime;
            }
        }

        public readonly int fingerId;
        private float startTime;
        private List<Vector2> positions = new List<Vector2>();

        public TouchInfo(int fingerId, Vector2 position)
        {
            this.fingerId = fingerId;
            startTime = Time.realtimeSinceStartup;
            AddPosition(position);
        }

        public void AddPosition(Vector2 position)
        {
            positions.Add(position);
        }

        public bool IsHit(GameObject targetGameObject, Camera camera = null)
        {
            if (null == camera)
            {
                camera = Camera.main;
            }
            var lastTouchPosition = positions.Last();
            if (null == targetGameObject.GetComponent<RectTransform>())
            {
                var ray = camera.ScreenPointToRay(lastTouchPosition);
                var hit = new RaycastHit();
                return Physics.Raycast(ray, out hit) && hit.collider.gameObject == targetGameObject;
            }
            else
            {
                var pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = lastTouchPosition;
                var raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);
                if (raycastResults.Count == 0)
                {
                    return false;
                }
                var resultGameObject = raycastResults.First().gameObject;
                while (null != resultGameObject)
                {
                    if (resultGameObject == targetGameObject)
                    {
                        return true;
                    }
                    var parent = resultGameObject.transform.parent;
                    resultGameObject = null != parent ? parent.gameObject : null;
                }
                return false;
            }
        }
    }

    public class GestureDetectorEvent : UnityEvent<Gesture, TouchInfo>
    {
        public GestureDetectorEvent()
        {
        }
    }
}