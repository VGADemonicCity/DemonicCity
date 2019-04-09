using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity.HomeScene
{
    public class HomeLevelDraw : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text level;

        // Use this for initialization
        void Start()
        {
            level.text = "Lv. " +Magia.Instance.Stats.Level.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}