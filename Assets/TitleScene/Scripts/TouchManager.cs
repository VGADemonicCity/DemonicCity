using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class TouchManager : MonoBehaviour
    {

        GameObject effectObj=null;
        GameObject newObj = null;
        
        // Use this for initialization
        void Start()
        {
            effectObj = ResourcesLoad.Load<GameObject>("Effects/TouchEffect");
            
            Debug.Log(effectObj.name);
            if (effectObj)
            {
                newObj = Instantiate(effectObj, transform);
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Down");
                newObj.GetComponent<ParticleSystem>().loop=true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Up");
                newObj.GetComponent<ParticleSystem>().loop = false;
            }
        }

    }
}