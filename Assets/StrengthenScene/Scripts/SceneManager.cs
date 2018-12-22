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

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
            
        }

        void Start()
        {
            popUpWindows[0].SetActive(false);
            popUpWindows[1].SetActive(false);

//            int mainLayer = LayerMask.NameToLayer("MainLayer");

           
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
                            case ("SelectAttributeButton"):
                                
                                popUpWindows[0].SetActive(true);
                                break;
                            case ("SelectSkillButton"):
                                popUpWindows[1].SetActive(true);
                                break;
                            case ("BackButton"):
                                popUpButton.gameObject.SetActive(false);
                                popUpWindows[0].SetActive(false);
                                popUpWindows[1].SetActive(false);
                                break;
                        }
                    }

                }
            });
        }


       
    }
}
