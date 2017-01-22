using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaster : MonoBehaviour {

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
	
	// Main/Camera will handle spawning/instantiating them!!! ********************
	void Update ()
	{

		if (Time.time >= spriteTimer) {
			GetComponent<SpriteRenderer> ().sprite = array [here];
			if (here == 0)
				here = 1;
			else if (here == 1)
				here = 0;
			spriteTimer = Time.time + spriteDelay;
		}

		if (tr.position == pos) {
			isMoving = false;
			timer = Time.time + timeDelay;
		} else
			isMoving = true;

		if (!isMoving) {

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
