using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goriya : MonoBehaviour {

	public Sprite[] horizSprites;
	public Sprite[] upSprites;
	public Sprite[] downSprites;
	public float spriteDelay;
	private float spriteTimer;

	public Room room;

	public Direction current_direction;

	// Use this for initialization
	void Start () {
		spriteTimer = Time.time + spriteDelay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		int temp = Random.Range (1, 4);

		if (temp == 1) { //go up
			if (Time.time >= spriteTimer) {
				if (GetComponent<SpriteRenderer> ().sprite != upSprites [0])
					GetComponent<SpriteRenderer> ().sprite = upSprites [0];
				else
					GetComponent<SpriteRenderer> ().sprite = upSprites [1];
				spriteTimer = Time.time + spriteDelay;
			}
			current_direction = Direction.NORTH;
		} else if (temp == 2) { //go right
			if (Time.time >= spriteTimer) {
				if (GetComponent<SpriteRenderer> ().sprite != horizSprites [0]) {
					GetComponent<SpriteRenderer> ().sprite = horizSprites [0];
				}
				else
					GetComponent<SpriteRenderer> ().sprite = horizSprites [1];
				GetComponent<SpriteRenderer> ().flipX = true;
				spriteTimer = Time.time + spriteDelay;
			}
			current_direction = Direction.EAST;
		} else if (temp == 3) { //go down
			if (Time.time >= spriteTimer) {
				if (GetComponent<SpriteRenderer> ().sprite != downSprites [0])
					GetComponent<SpriteRenderer> ().sprite = downSprites [0];
				else
					GetComponent<SpriteRenderer> ().sprite = downSprites [1];
				spriteTimer = Time.time + spriteDelay;
			}
			current_direction = Direction.SOUTH;
		} else if (temp == 4) { //go left
			if (Time.time >= spriteTimer) {
				if (GetComponent<SpriteRenderer> ().sprite != horizSprites [0])
					GetComponent<SpriteRenderer> ().sprite = horizSprites [0];
				else
					GetComponent<SpriteRenderer> ().sprite = horizSprites [1];
				spriteTimer = Time.time + spriteDelay;
			}
			current_direction = Direction.WEST;
		}
			

		if (Input.GetKeyDown (KeyCode.UpArrow)) { //Moving Up
			if (Time.time >= spriteTimer) {
				if (GetComponent<SpriteRenderer>().sprite != upSprites[0]) GetComponent<SpriteRenderer>().sprite = upSprites[0];
				else GetComponent<SpriteRenderer>().sprite = upSprites[1];
				spriteTimer = Time.time + spriteDelay;
			}
			//move up
		}
		else if (Input.GetKeyDown (KeyCode.DownArrow)) { //Moving Down
			if (Time.time >= spriteTimer) {
				if (GetComponent<SpriteRenderer>().sprite != downSprites[0]) GetComponent<SpriteRenderer>().sprite = downSprites[0];
				else GetComponent<SpriteRenderer>().sprite = downSprites[1];
				spriteTimer = Time.time + spriteDelay;
			}
			//move down
		}
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) { //Moving Left
			if (Time.time >= spriteTimer) {
				if (GetComponent<SpriteRenderer>().sprite != horizSprites[0]) GetComponent<SpriteRenderer>().sprite = horizSprites[0];
				else GetComponent<SpriteRenderer>().sprite = horizSprites[1];
				spriteTimer = Time.time + spriteDelay;
			}
			//move left
		}
	}
}
