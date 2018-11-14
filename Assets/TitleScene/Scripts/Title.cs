using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class Title : MonoBehaviour
    {

        public void ToHome()
        {
            SceneChange.SceneChanger(SceneName.Home);
        }



        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                ToHome();
            }
        }
    }
}
