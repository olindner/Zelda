using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goriya : MonoBehaviour {

	public Sprite[] horizSprites;
	public Sprite[] upSprites;
	public Sprite[] downSprites;
	public float spriteDelay;
	private float spriteTimer;

	// Use this for initialization
	void Start () {
		spriteTimer = Time.time + spriteDelay;
	}
	
	// Update is called once per frame
	void Update ()
	{
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
