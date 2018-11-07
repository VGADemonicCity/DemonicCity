using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity.HomeScene
{
    public class TouchEffect : MonoBehaviour
    {
        Vector3 position;
        ParticleSystem.MainModule pMain;
        private void Update()
        {

            pMain= GetComponent<ParticleSystem>().main;
            if (Input.GetMouseButton(0))
            {
                pMain.loop = true;
            }
            else
            {
                pMain.loop = false;
            }
        }

        
    }
}