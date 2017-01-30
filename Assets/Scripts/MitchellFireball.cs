using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitchellFireball : MonoBehaviour {

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
		Vector3 pos = PlayerController.instance.transform.position;

		if (angle == 0) target = new Vector3(pos.x - 8, pos.y - 3, 0); //bottom left
		else if (angle == 1) target = new Vector3(pos.x + 8, pos.y - 3, 0); //bottom right
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
