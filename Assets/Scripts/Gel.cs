﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gel : MonoBehaviour {

	private Transform tr;
	public float speed;
	Vector3 pos;
	private int health;
	private bool isMoving;
	private int dir;
	public float timeDelay;
	public float spriteDelay;
	private float timer;
	private float spriteTimer;
	public Sprite[] array;
	private int here;

	// Use this for initialization
	void Start () {
		pos = transform.position;
    	tr = transform;
		health = 1;
		isMoving = false;
		dir = Random.Range(0,3); //pick a random starting direction
		timer = Time.time + timeDelay;
		spriteTimer = Time.time + spriteDelay;
		here = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time >= spriteTimer) {
			GetComponent<SpriteRenderer>().sprite = array[here];
			if (here == 0) here = 1;
			else if (here == 1) here = 0;
			spriteTimer = Time.time + spriteDelay;
		}

		if (tr.position == pos) {
			isMoving = false;
			timer = Time.time + timeDelay;
		}
		else
			isMoving = true;

		if (!isMoving) {
			int num = Random.Range (0, 15);

			Vector3 rayUp = transform.TransformDirection (Vector3.up);
			Vector3 rayDown = transform.TransformDirection (Vector3.down);
			Vector3 rayLeft = transform.TransformDirection (Vector3.left);
			Vector3 rayRight = transform.TransformDirection (Vector3.right);
			RaycastHit hit;

			//Equal percentages each direction...
			if (num == 0 || num == 1) {
				if (Physics.Raycast (transform.position, rayUp, out hit, 1) && hit.transform.tag == "Wall") {
					return;
				}
				pos += Vector3.up;
				dir = 0;
			} 
			else if ((num == 2 || num == 3) && !Physics.Raycast (transform.position, rayLeft, 1)) {
				pos += Vector3.left;
				dir = 1;
			} 
			else if ((num == 4 || num == 5) && !Physics.Raycast (transform.position, rayRight, 1)) {
				pos += Vector3.right;
				dir = 2;
			} 
			else if ((num == 6 || num == 7) && !Physics.Raycast (transform.position, rayDown, 1)) {
				pos += Vector3.down;
				dir = 3;
			}
			//...added likelihood to continue moving in same direction
			//0=UP, 1=DOWN, 2=LEFT, 3=RIGHT
			else if (num >= 8 && num <= 14) {
				if (!Physics.Raycast (transform.position, rayUp, 1) && dir == 0) pos += Vector3.up;
				else if (!Physics.Raycast (transform.position, rayDown, 1) && dir == 1) pos += Vector3.down;
				else if (!Physics.Raycast (transform.position, rayLeft, 1) && dir == 2) pos += Vector3.left;
				else if (!Physics.Raycast (transform.position, rayRight, 1) && dir == 3) pos += Vector3.right;
			}
		}
		if (Time.time >= timer) transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Sword") {
			Destroy(col.gameObject);
			health--;
			if (health <= 0) Destroy(this.gameObject);
		}
	}
}