using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goriya : MonoBehaviour {

	public Sprite[] horizSprites;
	public Sprite[] upSprites;
	public Sprite[] downSprites;
	public float spriteDelay;
	private float spriteTimer;
	public int frames_between_throws;

	public Room room;

	public Direction current_direction;
	public int frames_before_change = 0;

	public GameObject boomerang;
	public bool has_boomerang = true;

	public GoriyaBoomerang current_gb;

	// Use this for initialization
	void Start () {
		spriteTimer = Time.time + spriteDelay;
		frames_between_throws = Random.Range (50, 100);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (frames_before_change == 0) {
			frames_before_change = Random.Range (50, 100);
			int temp = Random.Range (1, 4);
			if (temp == 1) { //go up
				GetComponent<SpriteRenderer> ().sprite = upSprites [0];
				current_direction = Direction.NORTH;
				GetComponent<Rigidbody> ().velocity = Vector3.up;
			} else if (temp == 2) { //go right
				GetComponent<SpriteRenderer> ().sprite = horizSprites [0];
				GetComponent<SpriteRenderer> ().flipX = true;
				current_direction = Direction.EAST;
				GetComponent<Rigidbody> ().velocity = Vector3.right;
			} else if (temp == 3) { //go down
				GetComponent<SpriteRenderer> ().sprite = downSprites [0];
				current_direction = Direction.SOUTH;
				GetComponent<Rigidbody> ().velocity = Vector3.down;
			} else if (temp == 4) { //go left
				current_direction = Direction.WEST;
				GetComponent<SpriteRenderer> ().sprite = horizSprites [0];
				GetComponent<Rigidbody> ().velocity = Vector3.left;
			}
			spriteTimer = Time.time + spriteDelay;
		} else {
			frames_before_change--;
			if (current_direction == Direction.NORTH) {
				if (Time.time >= spriteTimer) {
					if (GetComponent<SpriteRenderer> ().sprite != upSprites [0])
						GetComponent<SpriteRenderer> ().sprite = upSprites [0];
					else
						GetComponent<SpriteRenderer> ().sprite = upSprites [1];
					spriteTimer = Time.time + spriteDelay;
				}
			} else if (current_direction == Direction.EAST) {
				if (Time.time >= spriteTimer) {
					if (GetComponent<SpriteRenderer> ().sprite != horizSprites [0])
						GetComponent<SpriteRenderer> ().sprite = horizSprites [0];
					else
						GetComponent<SpriteRenderer> ().sprite = horizSprites [1];
					GetComponent<SpriteRenderer> ().flipX = true;
					spriteTimer = Time.time + spriteDelay;
				}
			} else if (current_direction == Direction.SOUTH) {
				if (Time.time >= spriteTimer) {
					if (GetComponent<SpriteRenderer> ().sprite != downSprites [0])
						GetComponent<SpriteRenderer> ().sprite = downSprites [0];
					else
						GetComponent<SpriteRenderer> ().sprite = downSprites [1];
					spriteTimer = Time.time + spriteDelay;
				}
			} else { //direction.WEST
				if (Time.time >= spriteTimer) {
					if (GetComponent<SpriteRenderer> ().sprite != horizSprites [0])
						GetComponent<SpriteRenderer> ().sprite = horizSprites [0];
					else
						GetComponent<SpriteRenderer> ().sprite = horizSprites [1];
					spriteTimer = Time.time + spriteDelay;
				}
			}
		}

		if (has_boomerang) {
			if (frames_between_throws > 0) {
				frames_between_throws--;
			} else {
				print ("time to throw boomerang for goriya");
				ThrowBoomerang ();
			}
		} else {
			current_gb.goriya_pos = this.transform.position;
		}
	}

	void ThrowBoomerang() {
		GameObject go = Instantiate (this.boomerang) as GameObject;
//		if (go.GetComponent<GoriyaBoomerang> ().speed == 5f) {
//			print ("WHHHHAFJFLFDKA");
//		}
		current_gb = new GoriyaBoomerang ();
		current_gb.the_boomerang = go;
		current_gb.goriya_pos = this.transform.position;
		Vector3 current_pos = this.transform.position;
		if (current_direction == Direction.NORTH) {
			current_gb.current_direction = Direction.NORTH;
			current_pos.y += 1;
			current_gb.target = new Vector3 (current_pos.x, current_pos.y + 4, current_pos.z);
		} else if (current_direction == Direction.EAST) {
			current_gb.current_direction = Direction.EAST;
			current_pos.x += 1;
			current_gb.target = new Vector3 (current_pos.x + 4, current_pos.y, current_pos.z);
		} else if (current_direction == Direction.SOUTH) {
			current_gb.current_direction = Direction.SOUTH;
			current_pos.y -= 1;
			current_gb.target = new Vector3 (current_pos.x, current_pos.y - 4, current_pos.z);
		} else { //current direction is WEST
			current_gb.current_direction = Direction.WEST;
			current_pos.x -= 1;
			current_gb.target = new Vector3 (current_pos.x - 4, current_pos.y, current_pos.z);
		}
		go.transform.position = current_pos;
		if (current_direction == Direction.NORTH) {
			go.GetComponent<Rigidbody> ().velocity = Vector3.up * current_gb.speed;
		} else if (current_direction == Direction.EAST) {
			go.GetComponent<Rigidbody> ().velocity = Vector3.right * current_gb.speed;
		} else if (current_direction == Direction.SOUTH) {
			go.GetComponent<Rigidbody> ().velocity = Vector3.down * current_gb.speed;
		} else { //curent direction is WEST
			go.GetComponent<Rigidbody> ().velocity = Vector3.left * current_gb.speed;
		}
		has_boomerang = false;
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "GoriyaBoomerang") {
			Destroy (coll.gameObject);
			has_boomerang = true;
			frames_between_throws = Random.Range (50, 100);
		}
	}
}
