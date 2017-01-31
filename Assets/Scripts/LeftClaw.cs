﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftClaw : MonoBehaviour {

	private SpriteRenderer sr;
	Vector3 origin;
	Vector3 target;
	private float extendTimer;
	public float speed;
	private bool movingout;
	private bool movingin;
	private bool delayset = false;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		origin = sr.transform.position;
		target = new Vector3(sr.transform.position.x - 3, sr.transform.position.y, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (sr.transform.position == origin && !delayset) {
			extendTimer = Time.time + Random.Range(0f,5f);
			delayset = true;
			movingout = true;
			movingin = false;
		}
		else if (sr.transform.position == target) {
			movingout = false;
			movingin = true;
			delayset = false;
		}

		if(movingout && Time.time >= extendTimer) sr.transform.position = Vector3.MoveTowards(sr.transform.position, target, Time.deltaTime * speed);
		else if (movingin) sr.transform.position = Vector3.MoveTowards(sr.transform.position, origin, Time.deltaTime * speed);
	}
}