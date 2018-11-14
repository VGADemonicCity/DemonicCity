using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowState : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (true)
        //    {

        //    }
        //    ChangeState("IsOpen");
        //}
	}
    
    public void ChangeState(string s)
    {
        GetComponent<Animator>().SetBool(s, !GetComponent<Animator>().GetBool(s));
        Debug.Log(GetComponent<Animator>().GetBool(s));
        Debug.Log("aaa");
    }
    public void ChangeState(string s,bool state)
    {
        GetComponent<Animator>().SetBool(s, state);
        Debug.Log(GetComponent<Animator>().GetBool(s));
        Debug.Log("aaa");
    }
}
