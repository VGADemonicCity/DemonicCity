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

            MagiaHome,
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
            //Setulus,
            Satan = 19,
            Single,
            Double,
            HalfMask,
            SowrdPrincess,
            BlackKing,
            Devil
        }

        List<Item> items = new List<Item>();
        List<Person> people = new List<Person>();

        List<GalleryButton> itemInstance = new List<GalleryButton>();
        List<GalleryButton> personInstance = new List<GalleryButton>();

        [SerializeField] GameObject leftObj;
        [SerializeField] GameObject rightObj;
        [SerializeField] GameObject returnObj;
        [SerializeField] GameObject homeObj;
        [SerializeField] Button[] tabs;

        [SerializeField] Transform listParent;
        [SerializeField] GameObject galleryButton;

        [SerializeField] Button LeftArrow;
        [SerializeField] Button RightArrow;
        TouchGestureDetector tGD;
        Progress progress;

        [SerializeField] ItemContent drawer;

        enum ObjectTag
        {
            Return,
            ToHome,
            LeftArrow,
            RightArrow,
            Person,
            Item,
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
            progress = Progress.Instance;
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
                    //Debug.Log(hitObj.name);
                    if (hitTag != ObjectTag.None
                    && touchInfo.HitDetection(out hitObj, TouchObjects[hitTag]))
                    {
                        //Debug.Log(hitObj.name);
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

            ListReset();
        }


        public void TabSelect(bool isItem)
        {
            tabs[0].interactable = isItem;
            tabs[1].interactable = !isItem;
            ListCheck(isItem);
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
            Debug.Log("Speak!");
        }


        public void ContentOpen(ItemTag tag)
        {
            drawer.transform.parent.gameObject.SetActive(true);
            if (GetItem(tag) != null)
            {
                drawer.Init(GetItem(tag), this);
            }
            if (GetPerson(tag) != null)
            {
                drawer.Init(GetPerson(tag), this);
            }
        }


        public void ContentClose()
        {
            drawer.gameObject.SetActive(false);
        }


        public Item GetItem(ItemTag tag)
        {
            return items.FirstOrDefault(x => x.tag == tag);
        }
        public Person GetPerson(ItemTag tag)
        {
            return people.FirstOrDefault(x => x.tag == tag);

        }


        bool CheckContent<T>(T item) where T : Item
        {
            if ((progress.MyStoryProgress & item.UnLockStory) == item.UnLockStory)
            {
                return true;
            }
            return false;
        }

        public bool CheckContent(ItemTag item)
        {
            Item tmpItem = items.FirstOrDefault(x => x.tag == item);
            if (tmpItem != null)
            {
                return (progress.MyStoryProgress & tmpItem.UnLockStory) == tmpItem.UnLockStory;
            }
            Person tmpPerson = people.FirstOrDefault(x => x.tag == item);
            if (tmpPerson != null)
            {
                return (progress.MyStoryProgress & tmpPerson.UnLockStory) == tmpPerson.UnLockStory;
            }
            return false;
        }

        Item NextContent(Item item)
        {
            int index = items.FindIndex(x => x.tag == item.tag);
            if (index == -1)
            {
                return null;
            }
            int currentIndex = index;
            currentIndex += 1;
            while (currentIndex != index)
            {
                if (currentIndex == items.Count)
                {
                    currentIndex = 0;
                }
                if (CheckContent(items[currentIndex]))
                {
                    return items[currentIndex];
                }
                currentIndex += 1;
            }
            return null;
        }
        Person NextContent(Person item)
        {
            int index = people.FindIndex(x => x.tag == item.tag);
            if (index == -1)
            {
                return null;
            }
            int currentIndex = index;
            currentIndex += 1;
            while (currentIndex != index)
            {
                if (currentIndex == people.Count)
                {
                    currentIndex = 0;
                }
                if (CheckContent(people[currentIndex]))
                {
                    return people[currentIndex];
                }
                currentIndex += 1;
            }
            return null;
        }

        Person PreviewContent(Person item)
        {
            int index = people.FindIndex(x => x.tag == item.tag);
            if (index == -1)
            {
                return null;
            }
            int currentIndex = index;
            currentIndex -= 1;
            while (currentIndex != index)
            {
                if (CheckContent(people[currentIndex]))
                {
                    return people[currentIndex];
                }
                if (currentIndex == 0)
                {
                    currentIndex = people.Count;
                }
                currentIndex -= 1;
            }
            return null;
        }
        Item PreviewContent(Item item)
        {
            int index = items.FindIndex(x => x.tag == item.tag);
            if (index == -1)
            {
                return null;
            }
            int currentIndex = index;
            currentIndex -= 1;
            while (currentIndex != index)
            {
                if (CheckContent(items[currentIndex]))
                {
                    return items[currentIndex];
                }
                if (currentIndex == 0)
                {
                    currentIndex = items.Count;
                }
                currentIndex -= 1;
            }
            return null;
        }

        public List<Item> GetSide(Item item)
        {
            return new List<Item>()
            {
                PreviewContent(item),
                item,
                NextContent(item)
            };
        }
        public List<Person> GetSide(Person item)
        {
            return new List<Person>()
            {
                PreviewContent(item),
                item,
                NextContent(item)
            };
        }


        void ListCheck(bool isItem)
        {
            if (itemInstance.Count == 0)
            {
                ListGenerate(items);
            }
            if (personInstance.Count == 0)
            {
                ListGenerate(people);
            }

            itemInstance.ForEach(x => x.gameObject.SetActive(isItem));
            personInstance.ForEach(x => x.gameObject.SetActive(!isItem));
        }
        void ListReset()
        {
            itemInstance.Clear();
            personInstance.Clear();
            foreach (Transform item in listParent)
            {
                Destroy(item.gameObject);
            }

            ListGenerate(items);
            ListGenerate(people);

            TabSelect(false);
        }




        void ListGenerate(List<Item> contents)
        {
            foreach (Item item in contents)
            {
                GalleryButton newBtn = Instantiate(galleryButton.gameObject, listParent).GetComponent<GalleryButton>();
                newBtn.Init(this, item.tag);

                itemInstance.Add(newBtn);
            }
        }
        void ListGenerate(List<Person> contents)
        {
            foreach (Person item in contents)
            {
                GalleryButton newBtn = Instantiate(galleryButton.gameObject, listParent).GetComponent<GalleryButton>();
                newBtn.Init(this, item.tag);

                personInstance.Add(newBtn);
            }
        }

        //void ListGenerate<T>(List<T> contents) where T : Item
        //{
        //    List<Person> tmp = contents.Cast<Person>().ToList();
        //    bool isPerson = tmp[0] as Person;
        //    foreach (T item in contents)
        //    {
        //        GalleryButton newBtn = Instantiate(galleryButton.gameObject, listParent).GetComponent<GalleryButton>();
        //        newBtn.Init(this, item.tag);

        //        if (isPerson)
        //        {
        //            people.Add(item as Person);
        //        }
        //        else
        //        {
        //            items.Add(item);
        //        }
        //    }
        //}


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