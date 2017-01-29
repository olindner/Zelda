using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaster : MonoBehaviour {

	public float speed;
	private Vector3 target;
	private int health;
	public int checks;
	public float timeDelay;
	public float spriteDelay;
	private float timer;
	private float spriteTimer;
	public Sprite[] array;
	private int here;
	private bool set1;
	private bool set2;
	private bool set3;
	private bool isLeft = false;
	private bool isRight = false;
	private bool isUp = false;
	private bool isDown = false;

	public Room room;

	// Use this for initialization
	void Start () {
		health = 2;
		timer = Time.time + timeDelay;
		spriteTimer = Time.time + spriteDelay;
		here = 0;
		target = transform.position;
		checks = 0;
		set1 = set2 = set3 = false;
	}

	// Main/Camera will handle spawning/instantiating them!!! ********************
	void Update ()
	{
		isLeft = isRight = isUp = isDown = false;
		float playerx = PlayerController.instance.transform.position.x;
		float playery = PlayerController.instance.transform.position.y;
		float playerxFloor = Mathf.Floor (playerx);
		float playeryFloor = Mathf.Floor (playery);

		//sprite alternating code
		if (Time.time >= spriteTimer) {
			GetComponent<SpriteRenderer> ().sprite = array [here];
			if (here == 0)
				here = 1;
			else if (here == 1)
				here = 0;
			spriteTimer = Time.time + spriteDelay;
		}

		if (playerx >= 65.5f && playerx <= 66.5f)
			isLeft = true;
		if (playerx >= 76.5f && playerx <= 77.5f)
			isRight = true;
		if (playerx >= 40.5f && playerx <= 41.5f)
			isUp = true;
		if (playerx >= 34.5f && playerx <= 35.5f)
			isDown = true;

		if (isLeft) {	
			if (!set1) {
				target = new Vector3 (transform.position.x + 1f, transform.position.y, 0); //Target 1: right 1 space
				set1 = true;
			}

			if (checks == 1 && !set2) { //has reached right1, and is in top part of room
				if (transform.position.y < playery) { //player is above
					target = new Vector3 (transform.position.x, transform.position.y + 3f, 0); //Target 2a: up 3 spaces
				} else if (transform.position.y >= playery) { //player is below
					target = new Vector3 (transform.position.x, transform.position.y - 3f, 0); //Target 2a: down 3 spaces
				}
				set2 = true;
			}

			if (checks == 2 && !set3) { //has reached up3/down3
				target = new Vector3 (transform.position.x - 1f, transform.position.y, 0); //Target 3: left 1 space
				set3 = true;
			}

			//Check if at destination, increment checkpoints
			if (transform.position == target) {
				checks++;
				if (checks == 3) {
					Destroy (gameObject);
					//need proper delete lines of code (Lillian)
				}
			}
		}

		transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * speed);
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Sword") {
			Destroy(col.gameObject);
			health--;
			if (health <= 0) {
				room.num_enemies_left--;
				room.things_inside_room.Remove (this.gameObject);
				Destroy (this.gameObject);
			}
		}
	}

//	void OnCollisionEnter (Collision coll)
//	{
//		if (coll.gameObject.tag == "Player") {
//			coll.gameObject.GetComponent<Collider>().isTrigger = true; //Player will now be a TRIGGER
//			print("made player into trigger");
//		}
//	}

	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			col.gameObject.transform.position = transform.position; //Player will now follow path of hand
		}
	}
}