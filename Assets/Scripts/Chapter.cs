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
        public List<EnemiesDataBase.EnemiesId> enemiesIds = new List<EnemiesDataBase.EnemiesId>();
        /// <summary>その章で使うストーリー進行度</summary>
        public Progress.StoryProgress storyProgress;
        /// <summary>その章のタイトル</summary>
        public string chapterTitle;
        /// <summary>その章に戦闘があるか</summary>
        public bool isBattle;


        /// <summary>ストーリーの進行度と敵のIDを3Wave分登録する</summary>
        public Chapter(Progress.StoryProgress progress, EnemiesDataBase.EnemiesId enemy1, EnemiesDataBase.EnemiesId enemy2, EnemiesDataBase.EnemiesId enemy3)
        {
            storyProgress = progress;
            enemiesIds.Add(enemy1);
            enemiesIds.Add(enemy2);
            enemiesIds.Add(enemy3);
        }
        /// <summary>ストーリーの進行度とタイトル、敵のIDを3Wave分登録する</summary>
        public Chapter(string title, Progress.StoryProgress progress, EnemiesDataBase.EnemiesId enemy1, EnemiesDataBase.EnemiesId enemy2, EnemiesDataBase.EnemiesId enemy3)
        {
            chapterTitle = title;
            storyProgress = progress;
            enemiesIds.Add(enemy1);
            enemiesIds.Add(enemy2);
            enemiesIds.Add(enemy3);
            isBattle = true;
        }
        /// <summary>ストーリーの進行度とタイトルを登録する</summary>
        public Chapter(string title, Progress.StoryProgress progress)
        {
            chapterTitle = title;
            storyProgress = progress;
            isBattle = false;
        }
    }
}