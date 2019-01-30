using System.Collections;
using UnityEngine;

public class Rotation : MonoBehaviour {
	[SerializeField] float xRotation = 0F;
	[SerializeField]float yRotation = 0F;
    [SerializeField] float zRotation = 0F;
	void OnEnable(){
		InvokeRepeating("rotate", 0f, 0.0167f);
	}
	void OnDisable(){
		CancelInvoke();
	}
	void Rotate(){
		this.transform.localEulerAngles += new Vector3(xRotation,yRotation,zRotation);
	}
}
