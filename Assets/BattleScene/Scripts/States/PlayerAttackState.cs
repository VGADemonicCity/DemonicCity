using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonicCity.BattleScene.Skill;
using System.Linq;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Player attack state.
    /// 攻撃処理を行うクラス
    /// </summary>
    public class PlayerAttackState : StatesBehaviour
    {
        /// <summary>Animator of magia</summary>
        [SerializeField] Animator magiaAnimator;
        /// <summary>攻撃アニメーションの途中からゲージの減少処理を挟む為の調整係数</summary>
        [SerializeField] float adjustmentCoefficent = .5f;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.PlayerAttack) // StateがPlayerAttack以外の時は処理終了
                {
                    return;
                }

                // ==============================
                // イベント呼び出し : SkillJudger,強化
                // ==============================
                m_skillManager.m_skillJudger.Invoke(m_magia.MyPassiveSkill, m_panelCounter.DestructionCount); // SkillManagerのイベントを呼び出してPassiveSkillをステータスに反映させる
                StartCoroutine(ActivateSkill()); // 強化の演出開始
            });
        }

        /// <summary>
        /// 強化時の演出
        /// </summary>
        /// <returns>The enhancement.</returns>
        IEnumerator ActivateSkill()
        {
            // 各スキルコンポーネントを取得して,コンポーネントがスキル発動可能フラグを建てている時,enumの数値が少ない順から発動アニメーションを行う
            var passiveSkills = m_battleManager.GetComponentsInChildren<PassiveSkill>().ToList();
            var sortedSkills = passiveSkills.OrderBy((skill) => skill.GetPassiveSkill);
            foreach (var skill in sortedSkills)
            {
                if (skill.IsActivatable)
                {

                    Debug.Log(skill.GetType());
                    magiaAnimator.CrossFadeInFixedTime(skill.GetPassiveSkill.ToString(), 0, 0);
                    var clipInfos = magiaAnimator.GetCurrentAnimatorClipInfo(0);
                    while (clipInfos.Length == 0)
                    {
                        yield return null;
                        clipInfos = magiaAnimator.GetCurrentAnimatorClipInfo(0);
                    }
                    yield return new WaitForSeconds(clipInfos[0].clip.length);
                }
            }
            // スキル発動の演出を終えたら攻撃アニメーション再生
            magiaAnimator.CrossFadeInFixedTime("Attack", 0);
            var clips = magiaAnimator.GetCurrentAnimatorClipInfo(0);
            while (clips.Length == 0)
            {
                yield return null;
                clips = magiaAnimator.GetCurrentAnimatorClipInfo(0);
            }
            yield return new WaitForSeconds(clips[0].clip.length * adjustmentCoefficent);

            // 攻撃の演出開始
            StartCoroutine(AttackProcess());
        }


        /// <summary>
        /// スキル効果反映後、攻撃処理と演出を行う
        /// </summary>
        /// <returns>The process.</returns>
        IEnumerator AttackProcess()
        {
            // ==============================
            // ここに攻撃の演出処理を入れる予定
            // ==============================
            Debug.Log("攻撃する前の[" + m_battleManager.CurrentEnemy.Id + "]の体力 : " + m_battleManager.CurrentEnemy.Stats.Temp.HitPoint);
            var damage = m_battleManager.m_MagiaStats.Attack - m_battleManager.CurrentEnemy.Stats.Temp.Defense;
            if (damage > 0)
            {
                m_battleManager.CurrentEnemy.Stats.Temp.HitPoint -= damage; // プレイヤーの攻撃力から敵防御力を引いた値分ダメージ
                m_enemyHPGauge.Sync(m_battleManager.CurrentEnemy.Stats.Temp.HitPoint); // HPGaugeと同期
            }
            Debug.Log("Damage is " + damage);
            Debug.Log("攻撃した後の[" + m_battleManager.CurrentEnemy.Id + "]の体力 : " + m_battleManager.CurrentEnemy.Stats.Temp.HitPoint);


            // ==================================
            // イベント呼び出し : StateMachine.
            // ==================================
            if (m_battleManager.CurrentEnemy.Stats.Temp.HitPoint > 0) // 敵のHPが1以上だったら敵の攻撃ステートに遷移
            {
                if (m_battleManager.m_StateMachine.m_PreviousState == BattleManager.StateMachine.State.EnemyAttack)
                {
                    // ==================================
                    // イベント呼び出し : StateMachine.PlayerChoice
                    // ==================================
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
                }
                else
                {
                    // ==================================
                    // イベント呼び出し : StateMachine.EnemyAttack
                    // ==================================
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.EnemyAttack);
                }
            }
            else // 敵のHPが0以下だったら
            {
                m_battleManager.CurrentEnemy.Destroy(); // 敵の破壊処理
                yield return new WaitWhile(() => m_battleManager.CurrentEnemy != null); // 敵オブジェクトが破壊される迄待機

                if (m_battleManager.m_StateMachine.m_Wave != BattleManager.StateMachine.Wave.LastWave) // 現在のウェーブが最後のウェーブではなかったら
                {
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.NextWave);
                }
                else
                {
                    // ==================================
                    // イベント呼び出し : StateMachine.Win
                    // もし次のWaveが存在すれば、次のWaveへ遷移する処理を書く
                    // ==================================
                    m_battleManager.SetStateMachine(BattleManager.StateMachine.State.Win);
                }
            }
        }
    }
}
