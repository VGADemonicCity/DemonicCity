using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.HomeScene
{
    public class GalleryManager: MonoBehaviour
    {
        public enum ItemTag
        {
            TestItem,



            TestPerson,
        }

        List<Item> items = new List<Item>();
        List<Person> people = new List<Person>();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        #region Import

        string itemPath = "Sources/GalleryData/Items";
        string peoplePath = "Sources/GalleryData/People";

        void ImportItems()
        {
            items = Resources.LoadAll<Item>(itemPath).ToList();
            people = Resources.LoadAll<Person>(peoplePath).ToList();

            items.Sort((x, y) => x.tag - y.tag);
            people.Sort((x, y) => x.tag - y.tag);

        }

        #endregion

    }

}