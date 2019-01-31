using System;
using System.Linq;
//using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    //[Serializable]
    //[CreateAssetMenu(fileName = "ChapterData", menuName = "CreateChapterData")]
    //public class ChapterData : ScriptableObject
    //{
    //    [SerializeField] List<Chapter> chapterData;
    //    public List<Chapter> ChapterDatas
    //    {
    //        get { return chapterData; }
    //    }
    //}
    [Serializable]
    public class ChapterManager : SingletonBase<ChapterManager>
    {
        /// <summary>ストーリー進行度のインスタンスの参照</summary>
        Progress progress;

        /// <summary>進行度と敵のIDを登録するList</summary>
        public List<Chapter> chapters = new List<Chapter>()
        {
            new Chapter("プロローグ",Progress.StoryProgress.Prologue,sourceFolderPath+"1/"),
            new Chapter("はじまりと邂逅",Progress.StoryProgress.Phoenix, EnemiesFactory.EnemiesId.Phoenix, EnemiesFactory.EnemiesId.Phoenix, EnemiesFactory.EnemiesId.Phoenix,sourceFolderPath+"2/"),
            new Chapter("道なき道へ",Progress.StoryProgress.Nafla, EnemiesFactory.EnemiesId.SingleCorner, EnemiesFactory.EnemiesId.DoubleCorner, EnemiesFactory.EnemiesId.Nafla,sourceFolderPath+"3/"),
            new Chapter("迫る鎮圧部隊",Progress.StoryProgress.ZAKO1,sourceFolderPath+"4/",false),
            new Chapter("消えぬ炎",Progress.StoryProgress.Amon,sourceFolderPath+"5/"),
            new Chapter("怒り狂う軍隊",Progress.StoryProgress.ZAKO2,sourceFolderPath+"6/",false),
            new Chapter("理の誘い",Progress.StoryProgress.Ashmedy,sourceFolderPath+"7/"),
            new Chapter("立ちふさがる防衛隊",Progress.StoryProgress.ZAKO3,sourceFolderPath+"8/",false),
            new Chapter("恐怖への扉",Progress.StoryProgress.Faulus,sourceFolderPath+"9/"),
            new Chapter("魔王親衛隊",Progress.StoryProgress.ZAKO4,sourceFolderPath+"10/",false),
            new Chapter("決戦の覚悟",Progress.StoryProgress.Barl,sourceFolderPath+"11/"),
            new Chapter("偽りの仮面",Progress.StoryProgress.InvigoratedPhoenix,sourceFolderPath+"12/"),
            new Chapter("終末の王",Progress.StoryProgress.Ixmagina,sourceFolderPath+"13/"),
            new Chapter("Test",Progress.StoryProgress.Test,sourceFolderPath+""),
        };



        /// <summary>
        /// 引数のストーリーの進行度に対応した敵のIDを取得する
        /// </summary>
        /// <param name="progress">敵のIDを知りたいストーリーの進行度</param>
        /// <returns>敵のIDのList</returns>
        public List<EnemiesFactory.EnemiesId> GetEnemies(Progress.StoryProgress progress)
        {
            Chapter chapter = chapters.Find(item => item.storyProgress == progress);
            return chapter.enemiesIds;
        }
        /// <summary>
        /// 現在進行している章に対応した敵のIDを取得する
        /// </summary>
        public List<EnemiesFactory.EnemiesId> GetEnemies()
        {
            CheckProgress();
            Chapter chapter = chapters.Find(item => item.storyProgress == progress.ThisStoryProgress);
            return chapter.enemiesIds;
        }


        /// <summary>
        /// 引数のストーリーの進行度に対応した章のタイトルを取得する
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public string GetTitle(Progress.StoryProgress progress)
        {
            Chapter chapter = chapters.Find(item => item.storyProgress == progress);
            return chapter.chapterTitle;
        }
        /// <summary>
        /// 現在進行している章のタイトルを取得する
        /// </summary>
        public string GetTitle()
        {
            CheckProgress();
            Chapter chapter = chapters.Find(item => item.storyProgress == progress.ThisStoryProgress);
            return chapter.chapterTitle;
        }
        /// <summary>
        /// 引数のストーリー進行度に対応したChapterを取得する
        /// </summary>
        /// <returns></returns>
        public Chapter GetChapter(Progress.StoryProgress progress)
        {
            return chapters.Find(item => item.storyProgress == progress);
        }
        /// <summary>
        /// 現在進行している章のChapterを取得する
        /// </summary>
        /// <returns></returns>
        public Chapter GetChapter()
        {
            CheckProgress();
            return chapters.Find(item => item.storyProgress == progress.ThisStoryProgress);
        }





        void CheckProgress()
        {
            if (progress == null)
            {
                progress = Progress.Instance;
            }
        }


        const string sourceFolderPath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Sources/";
    }
}