using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.HomeScene
{
    public class GalleryManager : MonoBehaviour
    {
        public enum ItemTag
        {
            TestItem,

            Getia,
            Demons,
            GehennaClown,
            Baptisma,
            ConvertDvilaity,
            TransPort,


            TestPerson,

            Magia,
            Ixmagina,
            Phoenix,
            InPhoenix,
            Nafla,
            Amon,
            Ashmedy,
            Faulus,
            Barl,
            Setulus,
            Satan,
            Single,
            Double,
            HalfMask,
            SowrdPrincess,
            BlackKing,
            Devil
        }

        List<Item> items = new List<Item>();
        List<Person> people = new List<Person>();

        [SerializeField] GameObject leftObj;
        [SerializeField] GameObject rightObj;
        [SerializeField] GameObject returnObj;
        [SerializeField] GameObject homeObj;

        [SerializeField] Button LeftArrow;
        [SerializeField] Button RightArrow;
        TouchGestureDetector tGD;

        [SerializeField] ItemContent drawer;

        enum ObjectTag
        {
            Return,
            ToHome,
            LeftArrow,
            RightArrow,
            None,
        }

        Dictionary<ObjectTag, GameObject> TouchObjects
        {
            get
            {
                return new Dictionary<ObjectTag, GameObject>()
                {
                    {ObjectTag.LeftArrow,leftObj },
                    {ObjectTag.RightArrow,rightObj},
                    {ObjectTag.Return,returnObj },
                    {ObjectTag.ToHome,homeObj},
                };
            }
        }

        void Awake()
        {
            tGD = TouchGestureDetector.Instance;
            ImportItems();
        }
        // Use this for initialization
        void Start()
        {
            ObjectTag hitTag = ObjectTag.None;
            GameObject hitObj = null;
            tGD.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    foreach (ObjectTag item in TouchObjects.Keys)
                    {
                        if (touchInfo.HitDetection(out hitObj, TouchObjects[item]))
                        {
                            hitTag = item;
                            break;
                        }
                        hitTag = ObjectTag.None;
                    }
                }
                if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    Debug.Log(hitObj.name);
                    if (hitTag != ObjectTag.None
                    && touchInfo.HitDetection(out hitObj, TouchObjects[hitTag]))
                    {
                        Debug.Log(hitObj.name);
                        switch (hitTag)
                        {
                            case ObjectTag.Return:
                                break;
                            case ObjectTag.ToHome:
                                break;
                            //case ObjectTag.LeftArrow:
                            //    ToLeft();
                            //    break;
                            //case ObjectTag.RightArrow:
                            //    ToRight();
                            //    break;
                            default:
                                break;
                        }
                    }

                }
            });
            LeftArrow.onClick.AddListener(() => { ToLeft(); });
            RightArrow.onClick.AddListener(() => { ToRight(); });
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ToLeft()
        {
            drawer.Scroll(true, 1f);
            
        }
        void ToRight()
        {
            drawer.Scroll(true, -1f);
        }
        public void CharacterSpeak(List<AudioClip> clips)
        {

        }

        public List<T> GetSide<T>(bool isItem, T item) where T : Item
        {
            List<T> tmp = new List<T>();

            List<int> indexs = new List<int>();
            int center = 0;

            if (isItem)
            {
                center = items.FindIndex(x => x.tag == item.tag);

                if (center == 0)
                {
                    indexs.Add(items.Count - 1);
                    indexs.Add(center);
                    indexs.Add(center + 1);
                }
                else if (center == items.Count - 1)
                {
                    indexs.Add(center - 1);
                    indexs.Add(center);
                    indexs.Add(0);
                }

                foreach (int i in indexs)
                {
                    tmp.Add(items[i] as T);
                }
            }
            else
            {
                center = people.FindIndex(x => x.tag == item.tag);
                if (center == 0)
                {
                    indexs.Add(people.Count - 1);
                    indexs.Add(center);
                    indexs.Add(center + 1);
                }
                else if (center == people.Count - 1)
                {
                    indexs.Add(center - 1);
                    indexs.Add(center);
                    indexs.Add(0);
                }

                foreach (int i in indexs)
                {
                    tmp.Add(people[i] as T);
                }
            }
            return tmp;

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