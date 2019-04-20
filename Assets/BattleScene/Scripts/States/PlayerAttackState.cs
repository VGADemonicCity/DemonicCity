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
        /// <summary>バフの上昇値を表示するUI</summary>
        [SerializeField] BuffTextBox buffTextBox;
        [SerializeField] float waitTime = .5f;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.PlayerAttack || m_battleManager.m_StateMachine.PreviousStateIsPause) // StateがPlayerAttack以外の時は処理終了
                {
                    return;
                }

                if (m_panelManager.IsOpenedAllPanelsExceptEnemyPanels)
                {
                    OnPanelCompleted();
                    return;
                }

                // 破壊数が0の時攻撃処理を行わずステート遷移する
                if (m_panelCounter.DestructionCount == 0)
                {
                    StartCoroutine(TransitionState());
                    return;
                }

                // =====================
                // イベント呼び出し : SkillJudger,強化
                // =====================
                m_skillManager.m_skillJudger.Invoke(m_magia.MyPassiveSkill, SkillManager.Timing.Enhancement, m_panelCounter.DestructionCount); // SkillManagerのイベントを呼び出してPassiveSkillをステータスに反映させる
                StartCoroutine(ActivateSkill(false)); // 強化の演出開始
            });
        }

        /// <summary>
        /// 敵パネル以外の全て引いた時敵を一撃で倒す
        /// </summary>
        public void OnPanelCompleted()
        {
            // 敵のHPをそのまま攻撃力に転換してダメージを与える
            var damage = m_battleManager.CurrentEnemy.Stats.HitPoint;
            StartCoroutine(AttackProcess(damage));
        }

        /// <summary>
        /// 発動可能なスキルのアニメーションを行う<see langword="true"/>is Attack animation, and <see langword="false"/>is Enhance animation.
        /// </summary>
        /// <param name="isAttack">攻撃演出かどうか</param>
        /// <returns></returns>
        IEnumerator ActivateSkill(bool isAttack)
        {
            var passiveSkills = m_battleManager.GetComponentsInChildren<PassiveSkill>().ToList();
            var isSkip = Debugger.BattleDebugger.Instance.EffectSkip;

            // 各スキルコンポーネントを取得して,コンポーネントがスキル発動可能フラグを建てている,SkipFlagが建っていない場合,発動アニメーションを行う
            foreach (var skill in passiveSkills)
            {
                if (skill.IsActivatable && !isSkip)
                {
                    if (skill.GetPassiveSkill == Magia.PassiveSkill.Invalid)
                    {
                        continue;
                    }
                    Debug.Log(skill.GetPassiveSkill.ToString());
                    magiaAnimator.CrossFadeInFixedTime(skill.GetPassiveSkill.ToString(), 0, 0);
                    var clipInfos = magiaAnimator.GetCurrentAnimatorClipInfo(0);

                    // 対象スキルと同じクリップが取得出来る迄処理を遅延させる
                    while (clipInfos.Length == 0)
                    {
                        yield return null;
                        clipInfos = magiaAnimator.GetCurrentAnimatorClipInfo(0);
                    }
                    while (clipInfos[0].clip.name != skill.GetPassiveSkill.ToString())
                    {
                        yield return null;
                        clipInfos = magiaAnimator.GetCurrentAnimatorClipInfo(0);
                    }

                    // バフUIに再生するテキストが存在する場合,対応したアウトラインの色に変えて,UIアニメーションを再生させる
                    if (!string.IsNullOrEmpty(skill.BuffText))
                    {
                        buffTextBox.SyncSettings(skill.GetEnhanceType);
                        buffTextBox.PlayText(skill.BuffText);
                    }
                    yield return new WaitForSeconds(clipInfos[0].clip.length);
                }
                // スキルが発動された時のコールバック.
                skill.OnSkillActivated();
            }

            // ダメージ計算を行い攻撃の演出開始
            if (isAttack)
            {
                var damage = m_battleManager.m_MagiaStats.Attack - m_battleManager.CurrentEnemy.Stats.Defense;
                StartCoroutine(AttackProcess(damage));
            }
            else
            {
                // =====================
                // イベント呼び出し : SkillJudger,攻撃
                // =====================
                m_skillManager.m_skillJudger.Invoke(m_magia.MyPassiveSkill, SkillManager.Timing.Attack, m_panelCounter.DestructionCount); // SkillManagerのイベントを呼び出してPassiveSkillをステータスに反映させる
                StartCoroutine(ActivateSkill(true)); // 攻撃の演出開始
            }
        }

        /// <summary>
        /// 引数のダメージを元に攻撃処理を行う
        /// </summary>
        /// <param name="damage"></param>
        IEnumerator AttackProcess(int damage)
        {
            if (damage > 0)
            {
                m_battleManager.CurrentEnemy.Stats.HitPoint -= damage; // プレイヤーの攻撃力から敵防御力を引いた値分ダメージ
                m_enemyHPGauge.Sync(m_battleManager.CurrentEnemy.Stats.HitPoint); // HPGaugeと同期
                yield return new WaitForSeconds(waitTime);
            }
            StartCoroutine(TransitionState());
        }


        /// <summary>
        /// 敵のHPに応じてステート遷移を行う
        /// </summary>
        /// <returns>The process.</returns>
        IEnumerator TransitionState()
        {
            // =====================
            // イベント呼び出し : StateMachine.
            // =====================
            if (m_battleManager.CurrentEnemy.Stats.HitPoint > 0) // 敵のHPが1以上だったら敵の攻撃ステートに遷移
            {
                if (m_battleManager.m_StateMachine.PreviousStateWithoutPause == BattleManager.StateMachine.State.EnemyAttack)
                {
                    // =============================
                    // イベント呼び出し : StateMachine.PlayerChoice
                    // =============================
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
