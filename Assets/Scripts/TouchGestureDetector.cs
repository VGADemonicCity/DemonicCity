using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DemonicCity
{
    /// <summary>
    /// Touch gesture detector for Unity.
    /// </summary>
    /// <author>KAKO Akihito</author>
    /// <email>kako@qnote.co.jp</email>
    /// <license>MIT License</license>
    public class TouchGestureDetector : MonoBehaviour
    {
        const float FLICK_TIME_LIMIT = 0.3f;

        /// <summary>
        /// Gesture.
        /// </summary>
        public enum Gesture
        {
            /// <summary>The touch begin.</summary>
            TouchBegin,
            /// <summary>The touch move.</summary>
            TouchMove,
            /// <summary>The touch stationary.</summary>
            TouchStationary,
            /// <summary>The touch end.</summary>
            TouchEnd,
            /// <summary>The click.</summary>
            Click,
            /// <summary>The flick top to bottom.</summary>
            FlickTopToBottom,
            /// <summary>The flick bottom to top.</summary>
            FlickBottomToTop,
            /// <summary>The flick left to right.</summary>
            FlickLeftToRight,
            /// <summary> flick right to left.</summary>
            FlickRightToLeft,
        }

        /// <summary>メインカメラ</summary>
        public Camera shootingCamera;
        /// <summary>isHitを使うか使わないかを切り替えられるフラグ。On状態の時は使う</summary>
        public bool hitCheck = true;
        /// <summary>フリック検知を有効にするかどうかを切り替えるフラグ</summary>
        public bool detectFlick = true;
        /// <summary>UnityEventクラスを引数を持てる様に継承したクラス</summary>
        public GestureDetectorEvent onGestureDetected = new GestureDetectorEvent();
        /// <summary>TouchInfo:タッチの情報を格納するリスト</summary>
        List<TouchInfo> touchInfos = new List<TouchInfo>();
        /// <summary>割る数</summary>
        public float m_divisor = 10f;


        void Awake()
        {
            if (null == shootingCamera) // カメラオブジェクトがnullの場合メインカメラを代入する
            {
                shootingCamera = Camera.main;
            }
        }

        void Update()
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
        void OnTouchBegin(int fingerId, Vector2 position)
        {
            var touchInfo = new TouchInfo(fingerId, position); // touch情報が入っている変数
            if (!hitCheck || touchInfo.IsHit(gameObject, shootingCamera))
            {
                touchInfos.Add(touchInfo);
                OnGestureDetected(Gesture.TouchBegin, touchInfo); // 呼び出し元でAddListenerで登録されたmethodに引数を渡してInvokeで呼び出す
            }
        }

        /// <summary>
        /// Ons the touch move.
        /// </summary>
        /// <param name="fingerId">Finger identifier.</param>
        /// <param name="position">Position.</param>
        void OnTouchMove(int fingerId, Vector2 position)
        {
            var touchInfo = touchInfos.FirstOrDefault(x => x.fingerId == fingerId); // touchInfosリストの先頭要素が引数のtouchInfoのfingerIdと同値だった場合その要素を返す,でなければnullを返す。
            if (null == touchInfo) // touchInfoがnullならmethod終了
            {
                return;
            }
            touchInfo.AddPosition(position); // touchInfoをリストに追加する
            OnGestureDetected(Gesture.TouchMove, touchInfo); // 呼び出し元でAddListenerで登録されたmethodに引数を渡してInvokeで呼び出す
        }

        /// <summary>
        /// Ons the touch stationary.
        /// </summary>
        /// <param name="fingerId">Finger identifier.</param>
        void OnTouchStationary(int fingerId)
        {
            var touchInfo = touchInfos.FirstOrDefault(x => x.fingerId == fingerId); // touchInfosリストの先頭要素が引数のtouchInfoのfingerIdと同値だった場合その要素を返す,でなければnullを返す。
            if (null == touchInfo) // touchInfoがnullならmethod終了
            {
                return;
            }
            OnGestureDetected(Gesture.TouchStationary, touchInfo); // 呼び出し元でAddListenerで登録されたmethodに引数を渡してInvokeで呼び出す
        }

        /// <summary>
        /// Ons the touch end.
        /// </summary>
        /// <param name="fingerId">Finger identifier.</param>
        /// <param name="position">Position.</param>
        void OnTouchEnd(int fingerId, Vector2 position)
        {
            var touchInfo = touchInfos.FirstOrDefault(x => x.fingerId == fingerId); // touchInfosリストの先頭要素が引数のtouchInfoのfingerIdと同値だった場合その要素を返す,でなければnullを返す。
            if (null == touchInfo) // touchInfoがnullならmethod終了
            {
                return;
            }
            touchInfo.AddPosition(position); // touchInfoをリストに追加する
            OnGestureDetected(Gesture.TouchEnd, touchInfo);  // 呼び出し元でAddListenerで登録されたmethodに引数を渡してInvokeで呼び出す

            var diff = touchInfo.Diff; // タッチ始めのフレームと離す瞬間のフレーム時の差分ベクトルを代入する
            var flickDistanceLimit = (float)Math.Min(Screen.width, Screen.height) / m_divisor; //画面の縦横で短い方を任意の値で割った数値を代入する

            // フリック検知フラグがON,且つタッチを始めてからの経過時刻がタイムリミット以内,且つxかyのタッチ差分が任意の閾値を超えた時
            if (detectFlick && touchInfo.ElapsedTime < FLICK_TIME_LIMIT && (Mathf.Abs(diff.x) > flickDistanceLimit || Mathf.Abs(diff.y) > flickDistanceLimit))
            {
                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y)) // タッチ差分がy軸よりx軸方が大きい時
                {
                    if (diff.x < 0f) // x軸がマイナス方向だったら
                    {
                        OnGestureDetected(Gesture.FlickRightToLeft, touchInfo); // 引数を左フリックにする
                    }
                    else
                    {
                        OnGestureDetected(Gesture.FlickLeftToRight, touchInfo);// でなければ引数を右フリックにする
                    }
                }
                else // y軸の方が大きい時
                {
                    if (diff.y < 0f) // y軸がマイナス方向だったら
                    {
                        OnGestureDetected(Gesture.FlickTopToBottom, touchInfo); // 引数を下フリックにする
                    }
                    else
                    {
                        OnGestureDetected(Gesture.FlickBottomToTop, touchInfo); // でなければ引数を上フリックにする
                    }
                }
            }
            else if (hitCheck && touchInfo.IsHit(gameObject, shootingCamera)) // フラグがOn,且つゲームオブジェクトをタッチしたら
            {
                OnGestureDetected(Gesture.Click, touchInfo); // 引数をクリックにする
            }

            touchInfos.RemoveAll(x => x.fingerId == fingerId); // 引数のfingerIdと同値だったらリストから削除する
        }

        /// <summary>
        /// Ons the gesture detected.
        /// </summary>
        /// <param name="gesture">Gesture.</param>
        /// <param name="touchInfo">Touch info.</param>
        void OnGestureDetected(Gesture gesture, TouchInfo touchInfo)
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
            ///  </summary>
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
                    return Time.time - startTime;
                }
            }

            /// <summary>The finger identifier.</summary>
            public readonly int fingerId; // どのタッチかを識別するユニーク値
                                          /// <summary>The start time.</summary>
            float startTime; // インスタンス生成した瞬間のゲーム経過時刻
                             /// <summary>The positions.</summary>
            List<Vector2> positions = new List<Vector2>();

            /// <summary>
            ///Touch情報を入れておくクラス
            ///  </summary>
            /// <param name="fingerId">Finger identifier.</param>
            /// <param name="position">Touch position.</param>
            public TouchInfo(int fingerId, Vector2 position)
            {
                this.fingerId = fingerId; // 
                startTime = Time.time; // タッチ開始時間を記録する
                AddPosition(position); //TouchInfoリストに追加する
            }

            /// <summary>
            /// Adds the position to List.
            /// positionsリストにタッチ座標を追加する
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
                if (null == camera) // 引数のカメラがnullなら
                {
                    camera = Camera.main; // 最初に検出したメインカメラタグがついているカメラオブジェクトを代入する
                }
                var lastTouchPosition = positions.Last(); // タッチを離す瞬間のフレームの座標
                if (null == targetGameObject.GetComponent<RectTransform>()) // 引数のゲームオブジェクトにRectTransformがなければ。つまりUIじゃなければ？
                {
                    var ray = camera.ScreenPointToRay(lastTouchPosition); // 最後にタッチした座標をRay型に変換する
                    var hit = new RaycastHit();
                    return Physics.Raycast(ray, out hit) && hit.collider.gameObject == targetGameObject; // Raycastにオブジェクトが検出される、且つそのオブジェクトが引数のオブジェクトと一緒ならtrueを返す
                }
                else // 引数のゲームオブジェクトにRectTransformがあったら。つまりUIならば？
                {
                    var pointerEventData = new PointerEventData(EventSystem.current)
                    {
                        position = lastTouchPosition // 指を離す瞬間のフレーム時の座標を代入する
                    };
                    var raycastResults = new List<RaycastResult>(); // UI用レイキャストの結果をリストで保持する変数
                    EventSystem.current.RaycastAll(pointerEventData, raycastResults); // ここでレイキャストを飛ばして当たったオブジェクトをリストに格納する
                    if (raycastResults.Count == 0) // UIを1個も検出出来なかったらfalseを返す
                    {
                        return false;
                    }
                    var resultGameObject = raycastResults.First().gameObject; // 最初に検出したオブジェクトを代入する
                    while (null != resultGameObject) // 検出したオブジェクトがあれば
                    {
                        if (resultGameObject == targetGameObject) // 検出したオブジェクトが引数と一緒ならばtrueを返す
                        {
                            return true;
                        }
                        var parent = resultGameObject.transform.parent; // 検出したオブジェクトの親が存在するならば、代入
                        resultGameObject = null != parent ? parent.gameObject : null;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Gesture detector event.
        /// methodを登録するクラス
        /// </summary>
        public class GestureDetectorEvent : UnityEvent<Gesture, TouchInfo>
        {
            public GestureDetectorEvent()
            {
            }
        }
    }
}
