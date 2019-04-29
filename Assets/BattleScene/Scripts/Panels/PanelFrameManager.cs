﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{

    /// <summary>
    /// パネル枠の位置
    /// </summary>
    public enum FramePosition
    {
        /// <summary>右にいる時</summary>
        Right = 1,
        /// <summary>真ん中にいる時</summary>
        Center = 2,
        /// <summary>左にいる時</summary>
        Left = 4,
    }


    /// <summary>
    /// PanelFrameManager
    /// </summary>
    public class PanelFrameManager : MonoSingleton<PanelFrameManager>
    {
        /// <summary>PanelFrameがどこにいるかの状態を取得する</summary>
        public FramePosition GetPanelFramePosition
        {
            get
            {
                return m_framePosition;
            }
        }

        public bool isSkillActivating { get; set; }

        /// <summary>枠移動の待ち時間</summary>
        [SerializeField] float m_waitTime = 1f;
        /// <summary>フレームの位置を表すenum</summary>
        [SerializeField] FramePosition m_framePosition = FramePosition.Center;
        /// <summary>PanelFrameがフリックで動ける座標</summary>
        float[] m_framePositions = new float[]
        {
            -730, // 左
            0, // 真ん中
            730, // 右
        };
        /// <summary>TouchGestureDetectorの参照</summary>
        TouchGestureDetector m_touchGestureDetector;
        BattleManager battleManager;
        /// <summary>パネル枠が動いている最中はフラグ</summary>
        bool isMoving;

        public void Start()
        {
            m_touchGestureDetector = TouchGestureDetector.Instance; // shingleton,TouchGestureDetectorインスタンスの取得
            battleManager = BattleManager.Instance;
            isSkillActivating = true;

            // UnityEvent機能を使ってメソッドを登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {

                if (isMoving || battleManager.m_StateMachine.m_State != BattleManager.StateMachine.State.PlayerChoice ) // 枠移動中の時は終了
                {
                    return;
                }

                switch (gesture) // タッチ情報が左右のフリックだったら
                {
                    case TouchGestureDetector.Gesture.FlickLeftToRight: // 右フリックの時,枠が左にいないときはひとつ左に動かす
                        switch (m_framePosition)
                        {
                            case FramePosition.Center:
                                StartCoroutine(MovingFrame(FramePosition.Left));
                                break;
                            case FramePosition.Right:
                                StartCoroutine(MovingFrame(FramePosition.Center));
                                break;
                        }
                        break;
                    case TouchGestureDetector.Gesture.FlickRightToLeft: // 左フリックの時,枠が右にいないときはひとつ右に動かす
                        switch (m_framePosition)
                        {
                            case FramePosition.Center:
                                StartCoroutine(MovingFrame(FramePosition.Right));
                                break;
                            case FramePosition.Left:
                                StartCoroutine(MovingFrame(FramePosition.Center));
                                break;
                        }
                        break;
                }
            });
        }

        /// <summary>
        /// PanelFrameを左へ動かす
        /// </summary>
        public void MovingLeft()
        {
            StartCoroutine(MovingFrame(FramePosition.Left));
        }

        /// <summary>
        /// PanelFrameを真ん中へ動かす
        /// </summary> 
        public void MovingCenter()
        {
            StartCoroutine(MovingFrame(FramePosition.Center));
        }

        /// <summary>
        /// PanelFrameを右へ動かす
        /// </summary>
        public void MovingRight()
        {
            StartCoroutine(MovingFrame(FramePosition.Right));
        }

        /// <summary>
        /// パネルフレームを引数に応じたフレームポジションへなめらかに移動させる
        /// </summary>
        /// <param name="framePosition">移動したい先のポジション</param>
        /// <returns></returns>
        public IEnumerator MovingFrame(FramePosition framePosition)
        {
            if (isMoving)
            {
                yield break;
            }

            isMoving = true;
            switch (framePosition)
            {
                case FramePosition.Right:
                    iTween.MoveTo(gameObject, iTween.Hash(("x"), m_framePositions[0]));
                    break;
                case FramePosition.Center:
                    iTween.MoveTo(gameObject, iTween.Hash(("x"), m_framePositions[1]));
                    break;
                case FramePosition.Left:
                    iTween.MoveTo(gameObject, iTween.Hash(("x"), m_framePositions[2]));
                    break;
            }
            yield return new WaitForSeconds(m_waitTime); // 移動してる間重複呼び出しを止める
            m_framePosition = framePosition; // panelFrameの位置情報を更新する
            isMoving = false;
            OnMovingFrame();
        }

        void OnMovingFrame()
        {
            foreach (var panel in PanelManager.Instance.PanelsInTheScene)
            {
                panel.CheckActivatableCollider();
            }
        }
    }
}
