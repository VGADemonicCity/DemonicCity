using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    [Serializable]
    public class ChapterManager : SingletonBase<ChapterManager>
    {

        /// <summary>進行度と敵のIDを登録するList</summary>
        public List<Chapter> chapters = new List<Chapter>()
        {
            new Chapter("プロローグ",Progress.StoryProgress.Prologue),
            new Chapter("はじまりと出逢い",Progress.StoryProgress.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix),
            new Chapter("道なき道へ",Progress.StoryProgress.Nafla, EnemiesDataBase.EnemiesId.Nafla, EnemiesDataBase.EnemiesId.Nafla, EnemiesDataBase.EnemiesId.Nafla),
            new Chapter("迫る鎮圧部隊",Progress.StoryProgress.ZAKO1),
            new Chapter("消えぬ炎",Progress.StoryProgress.Amon),
            new Chapter("怒り狂う軍隊",Progress.StoryProgress.ZAKO2),
            new Chapter("理の誘い",Progress.StoryProgress.Ashmedy),
            new Chapter("立ちふさがる防衛隊",Progress.StoryProgress.ZAKO3),
            new Chapter("恐怖への扉",Progress.StoryProgress.Faulus),
            new Chapter("魔王親衛隊",Progress.StoryProgress.ZAKO4),
            new Chapter("決戦の覚悟",Progress.StoryProgress.Barl),
            new Chapter("偽りの仮面",Progress.StoryProgress.InvigoratedPhoenix),
            new Chapter("終末の王",Progress.StoryProgress.Ixmagina),
        };



        /// <summary>
        /// 引数のストーリーの進行度に対応した敵のIDを取得する
        /// </summary>
        /// <param name="progress">敵のIDを知りたいストーリーの進行度</param>
        /// <returns>敵のIDのList</returns>
        public List<EnemiesDataBase.EnemiesId> GetEnemies(Progress.StoryProgress progress)
        {
            Chapter chapter = chapters.Find(item => item.storyProgress == progress);
            return chapter.enemiesIds;
        }
        /// <summary>
        /// 現在進行している章に対応した敵のIDを取得する
        /// </summary>
        public List<EnemiesDataBase.EnemiesId> GetEnemies()
        {
            Chapter chapter = chapters.Find(item => item.storyProgress == Progress.Instance.ThisStoryProgress);
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
            Chapter chapter = chapters.Find(item => item.storyProgress == Progress.Instance.ThisStoryProgress);
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
            return chapters.Find(item => item.storyProgress == Progress.Instance.ThisStoryProgress);
        }


    }
}