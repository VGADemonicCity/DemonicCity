using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StoryScene
{
    public class SSTest : MonoBehaviour
    {

        Progress progress = null;

        void Awake()
        {
            progress = Progress.Instance;
        }
        // Use this for initialization
        void Start()
        {
            progress.Test();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
