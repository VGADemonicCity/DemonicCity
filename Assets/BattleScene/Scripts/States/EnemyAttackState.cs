using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Enemy attack state.
    /// </summary>
    public class EnemyAttackState : StatesBehaviour
    {
        /// <summary>敵の攻撃力上昇倍率</summary>
        [SerializeField] float penaltyIncrease;
        /// <summary>敵のバフアニメーション</summary>
        [SerializeField] Animator buffAnimator;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.EnemyAttack
                || m_battleManager.m_StateMachine.PreviousStateIsPause 
                || m_battleManager.m_StateMachine.PreviousStateIsDebugging) // StateがEnemyAttack以外の時は処理終了
                {
                    return;
                }

                // もし敵パネルを選択して遷移してきた時
                if (m_battleManager.m_StateMachine.PreviousStateWithoutSpecialStates == BattleManager.StateMachine.State.PlayerChoice)
                {
                    // 敵の攻撃力を上げて、エフェクトを再生
                    m_battleManager.CurrentEnemy.AttackBuffActivate((int)((m_battleManager.CurrentEnemy.Stats.Attack * penaltyIncrease) - m_battleManager.CurrentEnemy.Stats.Attack));
                    StartCoroutine(PenaltyEffect());
                }
                else
                {
                    // 攻撃処理と演出
                    StartCoroutine(AttackProcess());
                }
            });
        }

        /// <summary>
        /// 敵の攻撃力を上げる演出(敵パネルをひいてしまった時のペナルティ演出)
        /// </summary>
        /// <returns></returns>
        IEnumerator PenaltyEffect()
        {
            // EffectSkipが無効な場合
            if (!Debugger.BattleDebugger.Instance.EffectSkip)
            {
                // 透明情報を消す
                var spriteRenderer = buffAnimator.gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.white;
                buffAnimator.CrossFadeInFixedTime("AttackBuff", 0);
                var clips = buffAnimator.GetCurrentAnimatorClipInfo(0);
                while (clips.Length == 0)
                {
                    yield return null;
                    clips = buffAnimator.GetCurrentAnimatorClipInfo(0);
                }
                yield return new WaitForSeconds(clips[0].clip.length); // アニメーションの長さ分遅延させる
                // 透明情報を戻す
                spriteRenderer.color = Color.clear;
            }
            // 攻撃処理と演出
            StartCoroutine(AttackProcess());
            m_battleManager.CurrentEnemy.AttackBuffDeactivate(); // ペナルティによって上昇したステータスを元に戻す
        }

        /// <summary>
        /// 敵の攻撃処理と演出
        /// </summary>
        /// <returns></returns>
        IEnumerator AttackProcess()
        {
            // ==============================
            // ここに攻撃の演出処理を入れる予定
            // ==============================

            Debug.Log("敵から攻撃される前の[" + m_magia + "]の体力 : " + m_battleManager.m_MagiaStats.HitPoint);
            // エフェクトスキップ無効時
            if (!Debugger.BattleDebugger.Instance.EffectSkip)
            {
                // Enemyの攻撃アニメーションを再生する
                m_battleManager.CurrentEnemy.AnimCtrl.CrossFadeInFixedTime("Attack", 0);
                var clips = m_battleManager.CurrentEnemy.AnimCtrl.GetCurrentAnimatorClipInfo(0);
                while (clips.Length == 0)
                {
                    yield return null;
                    clips = m_battleManager.CurrentEnemy.AnimCtrl.GetCurrentAnimatorClipInfo(0);
                }
                yield return new WaitForSeconds(clips[0].clip.length); // アニメーションの長さ分遅延させる
            }

            if (m_enemySkillGauge.m_flag == true)
            {
                m_enemySkillGauge.SkillActivate();
            }

            var damage = m_battleManager.CurrentEnemy.Stats.Attack - m_battleManager.m_MagiaStats.Defense; // 敵の攻撃力からプレイヤーの防御力を引いた値
            var attack = m_battleManager.CurrentEnemy.Stats.Attack;
            if (damage > 0)
            {
                m_battleManager.m_MagiaStats.HitPoint -= damage; // ダメージ
                m_magiaHPGauge.Sync(m_battleManager.m_MagiaStats.HitPoint); // HPGaugeと同期
            }

            OnProcessEnded();

            // ゲージが減少している間待つ
            yield return new WaitForSeconds(1f);
            TransitionState();
        }

        void TransitionState()
        {
            // ペナルティ処理の場合はプレイヤーの攻撃が後攻になっているのでプレイヤーの攻撃ターンに遷移する
            if (m_battleManager.m_StateMachine.PreviousStateWithoutSpecialStates == BattleManager.StateMachine.State.PlayerChoice)
            {

                if (m_battleManager.m_MagiaStats.HitPoint > 0) // プレイヤーの体力が1以上だったら次のターンへ遷移する
                {
                    // ==============================
                    // イベント呼び出し : StateMachine.PlayerAttack
                    // ==============================
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerAttack);
                }
                else // PlayerのHPが0以下になったらLoseステートに遷移する
                {
                    // ==============================
                    // イベント呼び出し : StateMachine.Lose
                    // ==============================
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.Lose);
                }
            }
            else
            {
                if (m_battleManager.m_MagiaStats.HitPoint > 0) // プレイヤーの体力が1以上だったら次のターンへ遷移する
                {
                    // ==============================
                    // イベント呼び出し : StateMachine.PlayerChoice
                    // ==============================
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
                }
                else // PlayerのHPが0以下になったらLoseステートに遷移する
                {
                    // ==============================
                    // イベント呼び出し : StateMachine.Lose
                    // ==============================
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.Lose);
                }
            }
        }

        /// <summary>
        /// スキルの発動が済んだら
        /// </summary>
        private void OnProcessEnded()
        {

            if (m_enemySkillGauge.m_flag == true)
            {
                m_enemySkillGauge.SkillDeactivate();
            }
            m_enemySkillGauge.Sync();
        }
    }
}
