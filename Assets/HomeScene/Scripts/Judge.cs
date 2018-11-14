using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity.HomeScene
{
    public class Judge : MonoBehaviour
    {

        public static void JudgeObj(GameObject Obj)
        {
            if (Obj)
            {
                if (Obj.name.Remove(2) == "To")
                {
                    ToButton(Obj);
                    
                }
            }
            
        } 




        public static void ToButton(GameObject Button)
        {
            if (Button.GetComponent<Button>())
            {
                SceneChange.SceneChanger(Button.name.Substring(2));
            }
        }
        
    }

}
