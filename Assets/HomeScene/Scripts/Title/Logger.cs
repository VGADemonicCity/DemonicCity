using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DemonicCity
{
    public class Logger : MonoBehaviour
    {
        [SerializeField] LogText texts;
        [SerializeField] Transform parent;
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += OnLogged;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Reset()
        {
            foreach (Transform item in parent)
            {
                Destroy(item.gameObject);
            }
        }
        void OnLogged(string text, string trace, LogType type)
        {
            Instantiate(texts.gameObject, parent).GetComponent<LogText>().Initialize(text, trace, type);
        }
    }
}