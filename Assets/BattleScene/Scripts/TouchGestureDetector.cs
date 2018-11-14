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

    /// <summary>
    /// Gesture.
    /// </summary>
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

    public Camera shootingCamera; // MainCamera
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
        foreach (var touch in Input.touches) // 毎フレームタッチされた本数分判定する
        {
            switch (touch.phase)
            {
                case TouchPhase.Began: // タッチされた瞬間のフレーム呼ばれる
                    OnTouchBegin(touch.fingerId, touch.position);
                    break;
                case TouchPhase.Moved: // タッチされている間のフレームずっと呼ばれる
                    OnTouchMove(touch.fingerId, touch.position);
                    break;
                case TouchPhase.Stationary: // タッチされて動いていない間のフレームずっと呼ばれる
                    OnTouchStationary(touch.fingerId);
                    break;
                case TouchPhase.Canceled: // タッチが認識出来ない時(eg.本数多すぎ、手のひらで触る等)呼ばれる
                case TouchPhase.Ended: // 離す時最後に触れていたフレームで呼ばれる
                    OnTouchEnd(touch.fingerId, touch.position);
                    break;
            }
        }
        if (Input.touchCount == 0) // タッチが0の時
        {
            if (Input.GetMouseButtonDown(0)) //左クリック押したら
            {
                OnTouchBegin(Int32.MaxValue, Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0)) // 左クリックを離したら
            {
                OnTouchEnd(Int32.MaxValue, Input.mousePosition);
            }
            else //左クリックを押している間
            {
                OnTouchMove(Int32.MaxValue, Input.mousePosition);
            }
        }
    }

    /// <summary>
    /// Ons the touch begin.
    /// </summary>
    /// <param name="fingerId">Finger identifier.</param>
    /// <param name="position">Position.</param>
    private void OnTouchBegin(int fingerId, Vector2 position)
    {
        var touchInfo = new TouchInfo(fingerId, position);
        if (!hitCheck || touchInfo.IsHit(gameObject, shootingCamera))
        {
            touchInfos.Add(touchInfo);
            OnGestureDetected(Gesture.TouchBegin, touchInfo);
        }
    }

    /// <summary>
    /// Ons the touch move.
    /// </summary>
    /// <param name="fingerId">Finger identifier.</param>
    /// <param name="position">Position.</param>
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

    /// <summary>
    /// Ons the touch stationary.
    /// </summary>
    /// <param name="fingerId">Finger identifier.</param>
    private void OnTouchStationary(int fingerId)
    {
        var touchInfo = touchInfos.FirstOrDefault(x => x.fingerId == fingerId);
        if (null == touchInfo)
        {
            return;
        }
        OnGestureDetected(Gesture.TouchStationary, touchInfo);
    }

    /// <summary>
    /// Ons the touch end.
    /// </summary>
    /// <param name="fingerId">Finger identifier.</param>
    /// <param name="position">Position.</param>
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

    /// <summary>
    /// Ons the gesture detected.
    /// </summary>
    /// <param name="gesture">Gesture.</param>
    /// <param name="touchInfo">Touch info.</param>
    private void OnGestureDetected(Gesture gesture, TouchInfo touchInfo)
    {
        onGestureDetected.Invoke(gesture, touchInfo);
    }

    /// <summary>
    /// Touch info.
    /// </summary>
    public class TouchInfo
    {

        /// <summary>
        /// Gets the difference.
        /// 二次元座標の差分
        /// </summary>
        /// <value>The diff.</value>
        public Vector2 Diff
        {
            get
            {
                return new Vector2(positions.Last().x - positions.First().x, positions.Last().y - positions.First().y); // 最初にタップした位置から最後にタップしていた位置の差分ベクトルをとる
            }
        }

        /// <summary>
        /// Gets the elapsed time.
        /// 経過時間
        /// </summary>
        /// <value>The elapsed time.</value>
        public float ElapsedTime
        {
            get
            {
                return Time.time - startTime; // 
            }
        }


        public readonly int fingerId;
        private float startTime;
        private List<Vector2> positions = new List<Vector2>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TouchGestureDetector.TouchInfo"/> class.
        /// </summary>
        /// <param name="fingerId">Finger identifier.</param>
        /// <param name="position">Position.</param>
        public TouchInfo(int fingerId, Vector2 position)
        {
            this.fingerId = fingerId;
            startTime = Time.time;
            AddPosition(position);
        }

        /// <summary>
        /// Adds the position.
        /// </summary>
        /// <param name="position">Position.</param>
        public void AddPosition(Vector2 position)
        {
            positions.Add(position);
        }

        /// <summary>
        /// Ises the hit.
        /// </summary>
        /// <returns><c>true</c>, if hit was ised, <c>false</c> otherwise.</returns>
        /// <param name="targetGameObject">Target game object.</param>
        /// <param name="camera">Camera.</param>
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

    /// <summary>
    /// Gesture detector event.
    /// </summary>
    public class GestureDetectorEvent : UnityEvent<Gesture, TouchInfo>
    {
        public GestureDetectorEvent()
        {
        }
    }
}