using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    /// <summary>
    /// 各章の情報が格納されるクラス
    /// </summary>
    [System.Serializable]
    public class Chapter
    {
        /// <summary>その章で使うEnemiesId</summary>
        public List<EnemiesFactory.EnemiesId> enemiesIds = new List<EnemiesFactory.EnemiesId>();
        /// <summary>その章で使うストーリー進行度</summary>
        public Progress.StoryProgress storyProgress;
        /// <summary>その章のタイトル</summary>
        public string chapterTitle;
        /// <summary>その章に戦闘があるか</summary>
        public bool isBattle;
        /// <summary>その章のソースフォルダのパス</summary>
        public string sourceFolderPath;


        /// <summary>ストーリーの進行度とタイトル、敵のIDを3Wave分登録する</summary>
        public Chapter(string title, Progress.StoryProgress progress, EnemiesFactory.EnemiesId enemy1, EnemiesFactory.EnemiesId enemy2, EnemiesFactory.EnemiesId enemy3, string folderPath)
        {
            chapterTitle = title;
            storyProgress = progress;
            sourceFolderPath = folderPath;
            enemiesIds.Add(enemy1);
            enemiesIds.Add(enemy2);
            enemiesIds.Add(enemy3);
            isBattle = true;
        }
        /// <summary>ストーリーの進行度とタイトルを登録する</summary>
        public Chapter(string title, Progress.StoryProgress progress,string folderPath)
        {
            chapterTitle = title;
            storyProgress = progress;
            sourceFolderPath = folderPath;
            isBattle = false;
        }
    }
}