using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonVRKeyboardController : MonoBehaviour {

	float degree = 0;
	float angle = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.RightArrow))
         {
             degree += 1f;
         }
         if (Input.GetKey(KeyCode.LeftArrow))
         {
             degree -= 1f;
         }
 
         angle = Mathf.LerpAngle(transform.rotation.y, degree, Time.time * 0.5f);
         transform.eulerAngles = new Vector3(0, angle, 0);
	}
}
