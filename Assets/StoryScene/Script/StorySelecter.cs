using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StorySelectScene
{
    using Story = Progress.StoryProgress;
    public class StorySelecter : MonoBehaviour
    {
        Progress progress;
        Story MyStory;
        [SerializeField] ChapterManager chapterManager;
        [SerializeField] RectTransform parent;
        [SerializeField] GameObject SelectButton;
        [SerializeField] List<SelectButton> selectButtons = new List<SelectButton>();

        void Awake()
        {
            progress = Progress.Instance;
            chapterManager = ChapterManager.Instance;
        }
        void Start()
        {
            List<Story> tgsStory = new List<Story>() { Story.Nafla, Story.Amon, Story.Faulus };

            int testCount = EnumCommon.GetLength<Story>() - 1;
            for (int i = testCount - 1; 0 <= i; i--)
            {
                int c = selectButtons.Count - i - 1;
                SelectButton currentButton = selectButtons[c];
                Story progressIndex = (Story)(1 << i);
                currentButton.Initialize(chapterManager.GetChapter(progressIndex), tgsStory.Contains(progressIndex));
            }

            return;



            MyStory = progress.MyStoryProgress;

            //progress.MyStoryProgress= Progress.StoryProgress.All;
            int progressCount = EnumCommon.GetLength<Story>() - 1;
            for (int i = progressCount - 1; 0 <= i; i--)
            {
                int c = selectButtons.Count - i - 1;
                Debug.Log($"{selectButtons.Count} : {c}");
                SelectButton currentButton = selectButtons[c];
                Story progressIndex = (Story)(1 << i);
                Story previewIndex = (Story)((int)progressIndex / 2);
                currentButton.Initialize(chapterManager.GetChapter(progressIndex), (previewIndex & MyStory) == previewIndex);
            }

            //for (int i = (int)MyStory; i >= 1; i /= 2)
            //{
            //    GameObject newSelectButton = Instantiate(SelectButton, parent);
            //    newSelectButton.GetComponent<SelectButton>().Initialize((Progress.StoryProgress)i, chapterManager.GetTitle((Progress.StoryProgress)i), this);
            //}

        }




    }
    public class ToStories
    {
        List<TGSStatusUint> tmp = new List<TGSStatusUint>()
        {
            new TGSStatusUint(Story.Nafla,4000,400,400, (Magia.PassiveSkill)63),
            new TGSStatusUint(Story.Amon,8000,800,800, (Magia.PassiveSkill)511),
            new TGSStatusUint(Story.Faulus,15000,1500,1500, (Magia.PassiveSkill)8191),
        };
        public void ToStory(Story chapter)
        {
            ///ForTGS
            TGSStatusUint unit = new TGSStatusUint();
            foreach (var item in tmp)
            {
                if (item.Story == chapter) unit = item;
            }
            Magia.Instance.Stats = new Status(unit.HP, unit.ATK, unit.DEF, 999, 0, 0, 0, 0, 0, 0);
            Magia.Instance.MyPassiveSkill = unit.Skill;
            Progress.Instance.TutorialProgressInBattleScene = BattleScene.Subject.AllFlag;

            Progress progress = Progress.Instance;
            Debug.Log(chapter.ToString());
            progress.ThisStoryProgress = chapter;
            Chapter thisChapter = ChapterManager.Instance.GetChapter();
            if (thisChapter.isStory)
            {
                progress.ThisQuestProgress = Progress.QuestProgress.Prologue;
                SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Story);
            }
            else
            {
                progress.ThisQuestProgress = Progress.QuestProgress.Battle;
                SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Battle);
            }

        }
    }



    ///ForTGS
    public class TGSStatusUint
    {
        public Story Story { get; }
        public int HP { get; }
        public int ATK { get; }
        public int DEF { get; }
        public Magia.PassiveSkill Skill { get; }
        public TGSStatusUint() { }
        public TGSStatusUint(Story story, int hp, int atk, int def, Magia.PassiveSkill skill)
        {
            this.Story = story;
            HP = hp;
            ATK = atk;
            DEF = def;
            Skill = skill;
        }

    }
}
