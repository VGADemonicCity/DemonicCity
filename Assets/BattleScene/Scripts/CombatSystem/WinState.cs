﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class WinState : StatesBehaviour
    {
        [SerializeField] GameObject resultWindow;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.Win) // StateがWin以外の時は処理終了
                {
                    return;
                }
                Debug.Log("Win state called.");

                //=======================
                //Resultのポップアップ等の処理を書く予定
                //=======================

                // TODO: 適正レベルに応じて総街破壊数に倍率処理を入れる


                // りょうくんのリザルト画面呼び出し
                resultWindow.SetActive(true);
            });
        }
    }
}