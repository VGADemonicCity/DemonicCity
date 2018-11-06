using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity.HomeScene
{
    public class TouchEffect : MonoBehaviour
    {
        Vector3 position;

        private void Update()
        {


            position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            position = new Vector3(position.x,position.y,-10);

            transform.position = position;

            Debug.Log(position);
        }

        
    }
}