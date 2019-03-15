using System.Collections;
using UnityEngine;
using TMPro;
using DemonicCity.BattleScene;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;


namespace DemonicCity.ResultScene
{
    public class ResultManager : MonoBehaviour
    {
        Magia magia;
        PanelCounter panelCounter;
        Status beforeStatus;

        List<Status> StatusDifference = new List<Status>();

        


        private void Start()
        {
            magia = Magia.Instance;
            panelCounter = PanelCounter.Instance;
            beforeStatus = magia.Stats;
        }

        void ResultCalclation()
        {
            var totalExperience = magia.TotalExperience + panelCounter.TotalDestructionCount;
            var requiredTotalExperience = magia.GetRequiredExpToNextLevel(magia.Stats.m_level);
            StatusDifference.Add(magia.Stats);

            // 総経験値がレベルアップに必要な経験値よりも高かった場合条件が満たさなくなる迄レベルアップ処理を行う
            while(totalExperience >= requiredTotalExperience)
            {
                magia.LevelUp();
                var diff = new Status()
                StatusDifference.Add();
                requiredTotalExperience = magia.GetRequiredExpToNextLevel(magia.Stats.m_level);
                
            }

            var diff = requiredTotalExperience - totalExperience;

        }
    }
}
