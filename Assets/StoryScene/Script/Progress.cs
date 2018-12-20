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

        //[Flags]
        //public enum StoryProgress
        //{
        //    Prologue = 1 << 0,
        //    Phoenix = 1 << 1,
        //    Naphula = 1 << 2,
        //    ZAKO1 = 1 << 3,
        //    Aamon = 1 << 4,
        //    ZAKO2 = 1 << 5,
        //    Ashmedai = 1 << 6,
        //    ZAKO3 = 1 << 7,
        //    Foras = 1 << 8,
        //    ZAKO4 = 1 << 9,
        //    Baal = 1 << 10,
        //    ExMagia = 1 << 12,
        //    All = (1 << 13) - 1
        //}

        /// <summary>ストーリーの進行度</summary>
        public enum StoryProgress
        {
            /// <summary>序章</summary>
            Prologue = 1,
            /// <summary>1章</summary>
            Phoenix,
            /// <summary>2章</summary>
            Nafla,
            /// <summary>3章</summary>
            ZAKO1,
            /// <summary>4章</summary>
            Amon,
            /// <summary>5章</summary>
            ZAKO2,
            /// <summary>6章</summary>
            Ashmedy,
            /// <summary>7章</summary>
            ZAKO3,
            /// <summary>8章</summary>
            Faulus,
            /// <summary>9章</summary>
            ZAKO4,
            /// <summary>10章</summary>
            Barl,
            /// <summary>11章</summary>
            Ixmagina,
            All,
        }
        /// <summary>1クエスト内での進行度</summary>
        [Flags]
        public enum QuestProgress
        {
            /// <summary>戦闘前のストーリー</summary>
            Prologue = 1,
            /// <summary>戦闘</summary>
            Battle = 2,
            /// <summary>戦闘後のストーリー</summary>
            Epilogue = 4,
            All = 7,
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
        public QuestProgress MyQuestProgress
        {
            get { return questProgress; }
            set { questProgress = value; Save(); }
        }

    }

}
