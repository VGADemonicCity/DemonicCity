using UnityEngine;

namespace DemonicCity.StrengthenScene
{
    public class SceneManager : MonoBehaviour
    {
        Magia magia;
        TouchGestureDetector touchGestureDetector;

        /// <summary>ポップアップウィンドウ</summary>
        [SerializeField]
        GameObject[] popUpWindows = new GameObject[2];

        public enum BUTTON
        {
            ATTRIBITE_BUTTON,
            SKILL_BUTTON,
            BACK_BUTTON,
        };

        private BUTTON button = BUTTON.ATTRIBITE_BUTTON;

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            
        }

        void Start()
        {
            popUpWindows[0].SetActive(false);
            popUpWindows[1].SetActive(false);

            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    GameObject popUpButton;
                    touchInfo.HitDetection(out popUpButton);

                    if(popUpButton != null)
                    {
                        switch (popUpButton.name)
                        {
                            case ("AttributeButton"):
                                popUpWindows[0].SetActive(true);
                                break;
                            case ("MasterdSkillButton"):
                                popUpWindows[1].SetActive(true);
                                break;
                            case ("BackButton"):
                                popUpWindows[0].SetActive(false);
                                popUpWindows[1].SetActive(false);
                                break;
                        }
                        //switch (button)
                        //{
                        //    case BUTTON.ATTRIBITE_BUTTON:
                        //        popUpWindows[0].SetActive(true);
                        //        break;
                        //    case BUTTON.SKILL_BUTTON:
                        //        popUpWindows[1].SetActive(true);
                        //        break;
                        //    case BUTTON.BACK_BUTTON:
                        //        popUpWindows[0].SetActive(false);
                        //        popUpWindows[1].SetActive(false);
                        //        break;

                        //}
                    }

                }
            });
        }


       
    }
}
