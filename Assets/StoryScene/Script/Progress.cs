using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    [Serializable]
    /// <summary>
    ///ストーリーの進行度、進捗
    ///</summary>
    public class Progress : SavableSingletonBase<Progress>
    {

        /// <summary>ストーリーの進行度</summary>
        [Flags]
        public enum StoryProgress
        {
            /// <summary>序章</summary>
            Prologue = 1,
            /// <summary>1章</summary>
            Phoenix = 2,
            /// <summary>2章</summary>
            Nafla = 4,
            /// <summary>3章</summary>
            ZAKO1 = 8,
            /// <summary>4章</summary>
            Amon = 16,
            /// <summary>5章</summary>
            ZAKO2 = 32,
            /// <summary>6章</summary>
            Ashmedy = 64,
            /// <summary>7章</summary>
            ZAKO3 = 128,
            /// <summary>8章</summary>
            Faulus = 256,
            /// <summary>9章</summary>
            ZAKO4 = 512,
            /// <summary>10章</summary>
            Barl = 1024,
            /// <summary>11章</summary>
            InvigoratedPhoenix = 2048,
            /// <summary>12章</summary>
            Ixmagina = 4096,
            All = 8191,
            Test = 8192,
        }
        /// <summary>1クエスト内での進行度</summary>

        public enum QuestProgress
        {
            /// <summary>戦闘前のストーリー</summary>
            Prologue = 0,
            /// <summary>戦闘</summary>
            Battle,
            /// <summary>戦闘後のストーリー</summary>
            Epilogue,
            /// <summary>ストーリー外</summary>
            None,

            All,
            Test,
        }

        /// <summary>ストーリーの進行度</summary>
        [SerializeField] StoryProgress storyProgress = StoryProgress.Ixmagina;

        /// <summary>現在進行しているクエスト</summary>
        [SerializeField] StoryProgress thisStoryProgress = StoryProgress.Prologue;
        /// <summary>現在進行しているクエストの進行度</summary>
        [SerializeField] QuestProgress questProgress = QuestProgress.Prologue;

        /// <summary>ストーリーの進行度のプロパティ</summary>
        public StoryProgress MyStoryProgress
        {
            get { return storyProgress; }
            set { storyProgress = value; Save(); }
        }

        /// <summary>現在進行しているクエストのプロパティ</summary>
        public StoryProgress ThisStoryProgress
        {
            get { return thisStoryProgress; }
            set { thisStoryProgress = value; Save(); }
        }

        /// <summary>現在進行しているクエストの進行度のプロパティ</summary>
        public QuestProgress ThisQuestProgress
        {
            get { return questProgress; }
            set { questProgress = value; Save(); }
        }

        public StoryProgress NextStory(StoryProgress nowStory)
        {
            int tmpStory = 0;
            foreach (StoryProgress story in Enum.GetValues(typeof(StoryProgress)))
            {
                if ((nowStory & story) == story)
                {
                    tmpStory = (int)story;
                }
            }
            tmpStory = tmpStory << 1;
            return (StoryProgress)tmpStory;
        }


        public bool IsClear
        {
            get
            {
                if ((storyProgress & StoryProgress.Ixmagina) == StoryProgress.Ixmagina)
                {
                    return true;
                }
                return false;
            }
        }
    }

}
