using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity.HomeScene
{
    public class GalleryButton : MonoBehaviour
    {

        TouchGestureDetector tGD;
        GalleryManager galleryM;

        [SerializeField] Text text;
        //[SerializeField] Button button;


        GalleryManager.ItemTag tag;
        bool unLocked = true;

        void Awake()
        {
            tGD = TouchGestureDetector.Instance;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        GameObject beginObj = null;

        public void Init(GalleryManager gallery, GalleryManager.ItemTag item)
        {
            galleryM = gallery;
            tag = item;
            unLocked = galleryM.CheckContent(item);
            //button.interactable = unLocked;
            if (unLocked)
            {
                text.text = "？？？";
                if (galleryM.GetItem(tag) != null)
                {
                    text.text = galleryM.GetItem(tag).name;
                }
                if (galleryM.GetPerson(tag) != null)
                {
                    text.text = galleryM.GetPerson(tag).name;
                }

                tGD.onGestureDetected.AddListener((gesture, touchInfo) =>
                {
                    if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                    {
                        beginObj = null;
                        touchInfo.HitDetection(out beginObj, gameObject);
                    }
                    if (gesture == TouchGestureDetector.Gesture.Click)
                    {
                        GameObject hit = null;
                        if (touchInfo.HitDetection(out hit, gameObject))
                        {
                            Debug.Log(beginObj.GetHashCode());
                            Debug.Log(hit.GetHashCode());
                            if (hit != null && hit == beginObj)
                            {
                                Debug.Log("Open");
                                galleryM.ContentOpen(tag);
                            }
                        }

                    }

                });
            }
            else
            {
                text.text = "？？？";
            }
        }

    }
}