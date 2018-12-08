using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StrengthenScene
{
    public class SceneManager : MonoBehaviour
    {
        public GameObject ShikiryoWindow;
        public GameObject SkillWindow;
        Animation animation;

        void Start()
        {
            ShikiryoWindow.SetActive(false);
            SkillWindow.SetActive(false);

            animation = ShikiryoWindow.GetComponent<Animation>();
            animation = SkillWindow.GetComponent<Animation>();
        }

        void Update()
        {

        }

        public void OpenShikiryoWindow()
        {
            animation.Play("OpenShikiryoWindow");
            ShikiryoWindow.SetActive(true);
        }
        public void CloseShiryoWindow()
        {
            //animation.Play("CloseShiryoWindow");
            ShikiryoWindow.SetActive(false);
            
        }
        public void OpenSkillWindow()
        {
            animation.Play("OpenSkillWindow");
            SkillWindow.SetActive(true);
        }
        public void CloseSkillWindow()
        {
            SkillWindow.SetActive(false);
        }
    }
}
