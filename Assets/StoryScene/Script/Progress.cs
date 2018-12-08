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
            Aamon = 1 << 2,
            Naphula = 1 << 3,
            Ashmedai = 1 << 4,
            Foras = 1 << 5,
            Baal = 1 << 6,
            ExMagia = 1 << 7,
            All = (1 << 8) - 1
        }

        [Flags]
        public enum QuestProgress
        {
            Prologue = 1 << 0,
            Battle = 1 << 1,
            Epilogue = 1 << 2,
            All = (1 << 3) - 1,
        }

        StoryProgress storyProgress = StoryProgress.Prologue;

        StoryProgress thisStoryProgress = StoryProgress.Prologue;
        QuestProgress questProgress = QuestProgress.Prologue;

        public StoryProgress MyStoryProgress {
            get { return storyProgress; }
            set { storyProgress = value; }
        }

        public StoryProgress ThisStoryProgress
        {
            get { return thisStoryProgress; }
            set { thisStoryProgress = value; }
        }
        public QuestProgress MyQuestProgress {
            get { return questProgress; }
            set { questProgress = value; }
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
