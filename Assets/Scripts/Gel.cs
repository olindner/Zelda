using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gel : MonoBehaviour {

	public float timeDelay;
	private float timer;

	// Use this for initialization
	void Start () {
		timer = Time.time + timeDelay;
	}
	
	// Update is called once per frame
	void Update () {
		timer = Time.time + timeDelay;
		//if (Time.time >= timer) move
	}
}
