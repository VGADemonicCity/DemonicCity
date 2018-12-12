using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    [Serializable]
    /// <summary>ストーリーの進行度、進捗</summary>
    public class Progress : SavableSingletonBase<Progress>
    {
        [Flags]
        public enum StoryProgress
        {
            Prologue = 1 << 0,
            Phoenix = 1 << 1,
            Naphula = 1 << 2,
            ZAKO1 = 1 << 3,
            Aamon = 1 << 4,
            ZAKO2 = 1 << 5,
            Ashmedai = 1 << 6,
            ZAKO3 = 1 << 7,
            Foras = 1 << 8,
            ZAKO4 = 1 << 9,
            Baal = 1 << 10,
            ExMagia = 1 << 12,
            All = (1 << 13) - 1
        }

        [Flags]
        public enum QuestProgress
        {
            Prologue = 1 << 0,
            Battle = 1 << 1,
            Epilogue = 1 << 2,
            All = (1 << 3) - 1,
        }

        [SerializeField] StoryProgress storyProgress = StoryProgress.Prologue;

        [SerializeField] StoryProgress thisStoryProgress = StoryProgress.Prologue;
        [SerializeField] QuestProgress questProgress = QuestProgress.Prologue;

        public StoryProgress MyStoryProgress
        {
            get { return storyProgress; }
            set { storyProgress = value; Save(); }
        }

        public StoryProgress ThisStoryProgress
        {
            get { return thisStoryProgress; }
            set { thisStoryProgress = value; Save(); }
        }

        public QuestProgress MyQuestProgress
        {
            get { return questProgress; }
            set { questProgress = value; Save(); }
        }






        public void Test()
        {
            for (int i = 1; i <= (int)StoryProgress.Naphula; i++)
            {
                Debug.Log((StoryProgress)i);
                Debug.Log((int)StoryProgress.All);
            }
        }



    }

}
