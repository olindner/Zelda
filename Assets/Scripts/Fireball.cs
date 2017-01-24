using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	public float spriteDelay;
	private float spriteTimer;
	public Sprite[] array;
	private int here;
	public int angle; 
	private Vector3 target;
	public float speed;

	// Use this for initialization
	void Start ()
	{
		spriteTimer = Time.time + spriteDelay;
		here = 0;
		target = Vector3.zero;
		Vector3 pos = PlayerController.instance.transform.position;

		if (angle == 1) target = PlayerController.instance.transform.position;

		if (pos.x >= 66f && pos.x <= 75f && pos.y >= 48f && pos.y <= 51f) { //Area 1
			if (angle == 0) target = new Vector3(65f, 52f, 0);
			else if (angle == 2) target = new Vector3(65f, 46f, 0);
		}
		if (pos.x >= 66f && pos.x <= 72f && pos.y >= 51f && pos.y <= 53f) { //Area 2
			if (angle == 0) target = new Vector3(70f, 53f, 0);
			else if (angle == 2) target = new Vector3(65f, 51f, 0);
		}
		if (pos.x >= 66f && pos.x <= 72f && pos.y >= 46f && pos.y <= 48f) { //Area 3
			if (angle == 0) target = new Vector3(65f, 47f, 0);
			else if (angle == 2) target = new Vector3(72f, 45f, 0);
		}
		if (pos.x >= 72f && pos.x <= 76f && pos.y >= 50f && pos.y <= 53f) { //Area 4
			if (angle == 0) target = new Vector3(75f, 53f, 0);
			else if (angle == 2) target = new Vector3(72f, 53f, 0);
		}
		if (pos.x >= 72f && pos.x <= 76f && pos.y >= 46f && pos.y <= 49f) { //Area 5
			if (angle == 0) target = new Vector3(75f, 45f, 0);
			else if (angle == 2) target = new Vector3(71f, 45f, 0);
		}
		if (pos.x >= 76f && pos.x <= 78f && pos.y >= 49f && pos.y <= 50f) { //Area 6
			if (angle == 0) target = new Vector3(78f, 51f, 0);
			else if (angle == 2) target = new Vector3(78f, 48f, 0);
		}
	}
	
	// Update is called once per frame
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

		transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * speed);
		if (transform.position == target) Destroy(gameObject);

	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Wall") {
			Destroy(gameObject);
		}
	}
}
