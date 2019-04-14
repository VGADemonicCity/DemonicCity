using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.HomeScene
{
    public class ItemContent : MonoBehaviour
    {
        //[SerializeField] Person testData;
        //[SerializeField] GalleryManager gallery;


        [SerializeField] Image[] cImage = new Image[3];
        [SerializeField] Text nameLabel;
        [SerializeField] Text infoText;
        [SerializeField] Text creatorLabel;
        [SerializeField] GameObject creatorObj;
        [SerializeField] Text actorLabel;
        [SerializeField] GameObject actorObj;
        [SerializeField] GameObject returnObj;
        [SerializeField] GameObject soundIcon;

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
            CreLabel,
            CreObj,
            ActLabel,
            ActObj,
            ReturnObj,
            Sound,
            None,
        }

        Dictionary<ObjectTag, Text> TextObjects
        {
            get
            {
                return new Dictionary<ObjectTag, Text>()
                {
                    {ObjectTag.Name,nameLabel},
                    {ObjectTag.Info,infoText},
                    {ObjectTag.CreLabel,creatorLabel},
                    {ObjectTag.ActLabel,actorLabel},
                    //{ObjectTag.CreObj,creatorObj},
                    //{ObjectTag.ActObj,actorObj},
                };
            }
        }

        Item currentContent = null;
        List<Item> contents = new List<Item>();

        Item item;
        //Item[] items = new Item[3];
        Person person;
        //Person[] people = new Person[3];

        // Use this for initialization
        void Start()
        {
            //Init(testData, gallery);



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
                    Debug.Log(touchInfo.Diff + " : " + scrollLim);
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
            int i = 1;
            if (sign > 0)           //ToLeft
            {
                i = 0;
            }
            else if (sign < 0)      //ToRight
            {
                i = 2;
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

        void CreatorReflect(string creName)
        {
            creatorLabel.text =  creName;
            creatorObj.SetActive(!string.IsNullOrEmpty(creName));
        }

        void ActorReflect(string actName)
        {
            actorLabel.text = actName;
            actorObj.SetActive(!string.IsNullOrEmpty(actName));
        }

        /// <summary>ItemならTrue</summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Reflect(Item data)
        {
            TextObjects[ObjectTag.Name].text = data.name;
            TextObjects[ObjectTag.Info].text = data.text;

            CreatorReflect(data.creator);

            person = data as Person;
            if (person != null)
            {
                ActorReflect(person.actor);

                List<Person> tmp = galleryM.GetSide(person);
                contents = tmp.Cast<Item>().ToList();
                SpriteReflect(tmp.Select(x => x.illust).ToList());
                //cImage[1].sprite = person.illust;
                //GetSideしてそれぞれ反映

                currentContent = person;
                return false;
            }
            else
            {
                ActorReflect(null);

                item = data;
                contents = galleryM.GetSide(item);
                List<Sprite> tmp = contents.Select(x => x.illust).ToList();
                SpriteReflect(tmp);
                currentContent = item;
                return true;
            }
        }


        public void Init(Item data, GalleryManager gM)
        {
            touchGestureDetector = TouchGestureDetector.Instance;
            galleryM = gM;

            ObjectTag touchTag = ObjectTag.None;
            GameObject beginObject = null;
            if (!Reflect(data))
            {
                touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
                {
                    if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                    {
                        touchTag = ObjectTag.None;
                        beginObject = null;
                        if (touchInfo.HitDetection(out beginObject, charaObj[2]))
                        {
                            touchTag = ObjectTag.Character;
                        }
                        //if (touchInfo.HitDetection(out beginObject, returnObj))
                        //{
                        //    touchTag = ObjectTag.ReturnObj;
                        //}
                        if (touchInfo.HitDetection(out beginObject, soundIcon))
                        {
                            touchTag = ObjectTag.Sound;
                        }

                    }
                    if (gesture == TouchGestureDetector.Gesture.Click)
                    {
                        GameObject hit;
                        if (touchTag == ObjectTag.Character
                        && touchInfo.HitDetection(out hit, charaObj[2]))
                        {
                            galleryM.CharacterSpeak(person.voice);
                        }
                        if (touchTag == ObjectTag.Sound
                        && touchInfo.HitDetection(out hit, soundIcon))
                        {
                            galleryM.CharacterSpeak(person.voice);
                        }
                        //if (touchTag == ObjectTag.ReturnObj
                        //&& touchInfo.HitDetection(out hit, returnObj))
                        //{
                        //    galleryM.ContentClose();
                        //}
                    }
                });
            }
        }

    }
}