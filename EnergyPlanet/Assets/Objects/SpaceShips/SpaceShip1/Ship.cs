using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    private float rotateSpeed = 40.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	void Update () {
        transform.Rotate(-Vector3.back * rotateSpeed * Time.deltaTime);
    }
}
