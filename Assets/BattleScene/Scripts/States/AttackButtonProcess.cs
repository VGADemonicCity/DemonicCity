using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class AttackButtonProcess: MonoBehaviour
    {
        /// <summary>PopupSystem</summary>
        [SerializeField] PopupSystem popupSystem;
        /// <summary>AttackButton</summary>
        [SerializeField] Button attackButton;

        /// <summary>
        /// 攻撃ボタンをポップアップさせる
        /// </summary>
        public void DisplayAttackButton()
        {
                popupSystem.Popup();
                popupSystem.SubscribeButton(new PopupSystemMaterial(StartAttack, attackButton.gameObject.name, true));
        }

        /// <summary>
        /// ポップアップしている攻撃ボタンを閉じる
        /// </summary>
        public void ButtonClose()
        {
            popupSystem.Close();
        }

        /// <summary>
        /// プレイヤーの攻撃ターンに遷移する
        /// </summary>
        void StartAttack()
        {
            // set the StateMachine
            BattleManager.Instance.SetStateMachine(BattleManager.StateMachine.State.PlayerAttack);
        }
    }
}