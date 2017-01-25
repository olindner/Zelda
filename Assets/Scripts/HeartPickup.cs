using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour {

	public Sprite[] sprites;
	public float spriteDelay = 0.2f;
	private float spriteTimer;

	// Use this for initialization
	void Start () {
		spriteTimer = Time.time + spriteDelay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time >= spriteTimer) {
			if (GetComponent<SpriteRenderer>().sprite != sprites[0]) GetComponent<SpriteRenderer>().sprite = sprites[0];
			else GetComponent<SpriteRenderer>().sprite = sprites[1];
			spriteTimer = Time.time + spriteDelay;
		}
	}
}
