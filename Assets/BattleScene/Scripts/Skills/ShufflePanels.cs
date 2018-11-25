using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// Shuffle panels.
    /// スキル : シャッフルパネル
    /// 引いたパネル枚数が30枚以上の時、画面に表示されている3*3のパネルを引いていない状態に戻してシャッフルする
    /// </summary>
    public class ShufflePanels : MonoBehaviour
    {
        [SerializeField] BoxCollider2D m_sensor; // colliderを検出する為のcollider
        /// <summary>シャッフルスキル発動条件値</summary>
        [SerializeField] int m_shuffleSkillConditions = 30;
        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_panelCounter = PanelCounter.Instance; // PannelCounterの参照取得
        }

        /// <summary>
        /// シャッフルスキルを発動したら、現在画面に表示されている3*3のパネルを全て非表示の状態に戻して、その3*3のパネル内でまたシャッフルさせる
        /// </summary>
        public void PanelShuffle()
        {
            var shuffleCounter = m_panelCounter.m_CounterForShuffleSkill; // シャッフルスキル用のカウント数取得
            if (shuffleCounter >= m_shuffleSkillConditions) // 指定した回数以上パネルが引かれていたら
            {
                m_sensor.enabled = true; // colliderをactiveにする
                var results = new Collider2D[9]; // 結果を受け取るための配列
                m_sensor.OverlapCollider(new ContactFilter2D(), results); // 設定したcolliderと重なっているcolliderを検出し配列に代入する
                var panelList = new List<Panel>(); // colliderに検出されたオブジェクトのPanelの参照リスト
                var panelTypes = new List<PanelType>(); // colliderに検出されたパネルのPanelTypeをリストで格納

                foreach (var panel in results) // colliderに検出されたパネルとそのPanelTypeを全てリストに格納する
                {
                    var panelObject = panel.gameObject.GetComponent<Panel>(); // Panelの参照取得
                    panelList.Add(panelObject); // Panelをリストに追加
                    panelTypes.Add(panelObject.m_panelType); // PanelTypeをリストに追加
                }
                var result = panelTypes.OrderBy((arg1) => Guid.NewGuid()).ToArray(); // Guid配列に変換、OrderByでアルファベット順に並び替える
                var count = 0; // ForEachに使うresult配列の要素指定用のカウンター

                panelList.ForEach((panel) => // リストに格納した各パネルにGuidでランダム化したPanelTypeを順番に代入の後パネルを引いていない状態に戻す
                {
                    panel.m_panelType = result[count]; // PanelTypeの代入
                    panel.ResetPanel(); // パネルを引いていない状態に戻す
                    count++; // カウントアップ
                });

                m_sensor.enabled = false; // colliderをdisableにする
                m_panelCounter.ResetShuffleSkillCounter(); // カウンターをリセット
            }
        }
    }
}
