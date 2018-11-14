using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity.HomeScene
{
    public class WindowManager : MonoBehaviour
    {

        public enum Window
        {
            Growth, Config, Calender, PresentBox
        }

        public GameObject[] parents;
        public GameObject[] windowObjects;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void WindowOpen(Window window)
        {
            int i = (int)window;
            GameObject newPanel = Instantiate(windowObjects[i], parents[i].transform);
            newPanel.GetComponent<WindowState>().ChangeState("IsOpen", true);
        }

    }
}