using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StrengthenScene
{
    public class SceneManager : MonoBehaviour
    {
        public GameObject ShikiryoWindow;
        public GameObject SkillWindow;
 
        void Start()
        {
            ShikiryoWindow.SetActive(false);
            SkillWindow.SetActive(false);
        }

        void Update()
        {

        }

        public void OpenShikiryoWindow()
        {
            ShikiryoWindow.SetActive(true);
        }
        public void CloseShiryoWindow()
        {
            ShikiryoWindow.SetActive(false);
            
        }
        public void OpenSkillWindow()
        {
            SkillWindow.SetActive(true);
        }
        public void CloseSkillWindow()
        {
            SkillWindow.SetActive(false);
        }
    }
}
