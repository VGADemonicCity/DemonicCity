using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    private Animator animator;
    

    void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    void Start ()
    {
        
	}
	
	void Update ()
    {
		
	}

    public void NextAnimation()
    {
        animator.SetBool("isPlay", true);
    }
}
