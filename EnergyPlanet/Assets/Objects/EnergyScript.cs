﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyScript : MonoBehaviour {
    private float rotateSpeed = 40.0f;
    private ParticleSystem particle;
    public Color myColor = Color.yellow;


    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(-Vector3.up * rotateSpeed * Time.deltaTime);

    }

    
}
