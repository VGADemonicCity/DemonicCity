using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.HomeScene
{
    public class ItemContent : MonoBehaviour
    {
        [SerializeField] Person testData;
        [SerializeField] GalleryManager gallery;


        [SerializeField] Image[] cImage = new Image[3];
        [SerializeField] Text nameLabel;
        [SerializeField] Text infoText;
        [SerializeField] GameObject[] charaObj = new GameObject[3];

        [SerializeField] PageSwitch swicher;

        GalleryManager galleryM;
        TouchGestureDetector touchGestureDetector;

        bool IsItem
        {
            get
            {
                return person == null;

            }
        }

        //Item currentContent;

        float scrollLim = 300f;

        enum ObjectTag
        {
            Character,
            Name,
            Info,
        }

        Dictionary<ObjectTag, Text> TextObjects
        {
            get
            {
                return new Dictionary<ObjectTag, Text>()
                {
                    {ObjectTag.Name,nameLabel},
                    {ObjectTag.Info,infoText},
                };
            }
        }

        Item currentContent = null;
        List<Item> contents = new List<Item>();

        Item item;
        Item[] items = new Item[3];
        Person person;
        Person[] people = new Person[3];

        // Use this for initialization
        void Start()
        {
            Init(testData, gallery);



            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.FlickLeftToRight)
                {
                    Scroll(true, 1f);
                }
                else if (gesture == TouchGestureDetector.Gesture.FlickRightToLeft)
                {
                    Scroll(true, -1f);
                }
                else if (gesture == TouchGestureDetector.Gesture.Click)
                {
                    Debug.Log(touchInfo.Diff);
                    if (touchInfo.Diff.x < -scrollLim)
                    {
                        Scroll(true, -1f);
                    }
                    else if (touchInfo.Diff.x > scrollLim)
                    {
                        Scroll(true, 1f);
                    }
                    else if (Mathf.Abs(touchInfo.Diff.x) > 1)
                    {
                        Scroll(false);
                    }
                }
            });

        }

        // Update is called once per frame
        void Update()
        {

        }




        public void Scroll(bool isScroll, float sign = 0f)
        {
            swicher.Scroll(isScroll, sign);
            int i = 0;
            if (sign > 0)           //ToLeft
            {
                i = 0;
                if (IsItem)
                {

                }
                else
                {

                }
            }
            else if (sign < 0)      //ToRight
            {
                i = 2;
                if (IsItem)
                {

                }
                else
                {

                }
            }
            cImage[1].sprite = cImage[i].sprite;
            swicher.targetObj.transform.localPosition = new Vector3(0, swicher.targetObj.transform.localPosition.y, swicher.targetObj.transform.localPosition.z);

            Reflect(contents[i]);
        }

        void SpriteReflect(List<Sprite> sprites)
        {
            cImage[0].sprite = sprites[0];
            cImage[1].sprite = sprites[1];
            cImage[2].sprite = sprites[2];
        }

        /// <summary>ItemならTrue</summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Reflect(Item data)
        {
            TextObjects[ObjectTag.Name].text = data.name;
            TextObjects[ObjectTag.Info].text = data.text;


            person = data as Person;
            if (person != null)
            {
                List<Person> tmp = galleryM.GetSide(false, person);
                contents = tmp.Cast<Item>().ToList();
                SpriteReflect(tmp.Select(x => x.illust).ToList());
                //cImage[1].sprite = person.illust;
                //GetSideしてそれぞれ反映

                currentContent = person;
                return false;
            }
            else
            {
                item = data;
                contents = galleryM.GetSide(true, item);

                currentContent = item;
                return true;
            }
        }


        public void Init(Item data, GalleryManager gM)
        {
            touchGestureDetector = TouchGestureDetector.Instance;
            galleryM = gM;


            if (!Reflect(data))
            {
                touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
                {
                    GameObject beginObject = null;
                    if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                    {
                        if (!touchInfo.HitDetection(out beginObject, charaObj[2]))
                        {
                            beginObject = null;
                        }
                    }
                    if (gesture == TouchGestureDetector.Gesture.Click)
                    {
                        GameObject hit;
                        if (beginObject != null
                        && touchInfo.HitDetection(out hit, beginObject))
                        {
                            galleryM.CharacterSpeak(person.voice);
                        }
                    }
                });
            }
        }

    }
}