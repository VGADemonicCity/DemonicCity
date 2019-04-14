using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity
{
    public class Credit : MonoBehaviour
    {

        [SerializeField] Transform parent;
        [SerializeField] GameObject creator;
        [SerializeField] Text role;
        [SerializeField] Text label;

        [SerializeField] Creators creators;


        // Use this for initialization
        void Start()
        {
            foreach (Transform child in parent)
            {
                DestroyImmediate(child.gameObject);
            }

            foreach (Creators.Creator item in creators.creators)
            {
                role.text = item.role;
                label.text = item.name;
                Instantiate(creator, parent);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }





    }
}