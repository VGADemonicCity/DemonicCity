using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class ForRay : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public GameObject HitRay(Vector3 rayPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(rayPos)  ;
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            return hit.collider.gameObject;
        }
    }

}
