using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.HomeScene
{
    public class CharaContent : MonoBehaviour
    {

        [SerializeField] Image cImage;
        [SerializeField] Text[] texts = new Text[2];
        

        Item item;
        Person person;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Init(Item data)
        {
            person = data as Person;
            if (person == null)
            {

            }
        }

    }
}