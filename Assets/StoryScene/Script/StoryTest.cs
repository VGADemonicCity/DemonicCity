using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DemonicCity.StoryScene
{
    public class StoryTest : MonoBehaviour
    {

        [SerializeField] Dropdown[] dropdowns;
        Progress progress;
        List<string> quest = new List<string>();
        List<string> story = new List<string>();

        private void Awake()
        {
            progress = Progress.Instance;


            string[] questA = System.Enum.GetNames(typeof(Progress.QuestProgress));
            string[] storyA = System.Enum.GetNames(typeof(Progress.StoryProgress));

            quest = new List<string>(questA);
            story = new List<string>(storyA);
        }

        // Use this for initialization
        void Start()
        {
            dropdowns[0].AddOptions(quest);
            dropdowns[1].AddOptions(story);
        }

        public void SelectScene()
        {
            Progress.QuestProgress questProgress;
            Progress.StoryProgress storyProgress;
            if (EnumCommon.TryParse(quest[dropdowns[0].value], out questProgress))
            {
                progress.ThisQuestProgress = questProgress;
            }
            if (EnumCommon.TryParse(story[dropdowns[1].value], out storyProgress))
            {
                progress.ThisStoryProgress = storyProgress;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}