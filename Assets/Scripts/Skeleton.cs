using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {

	private Transform tr;
	public float speed;
	Vector3 pos;
	private int health;
	private bool isMoving;
	private int dir;

	// Use this for initialization
	void Start () {
		pos = transform.position;
    	tr = transform;
		health = 2;
		isMoving = false;
		dir = Random.Range(0,3); //pick a random starting direction
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (tr.position == pos)
			isMoving = false;
		else
			isMoving = true;

		//Need to make way to pause after getting to square

		if (!isMoving) {
			int num = Random.Range (0, 15);

			Vector3 rayUp = transform.TransformDirection (Vector3.up);
			Vector3 rayDown = transform.TransformDirection (Vector3.down);
			Vector3 rayLeft = transform.TransformDirection (Vector3.left);
			Vector3 rayRight = transform.TransformDirection (Vector3.right);

			//Equal percentages each direction...
			if ((num == 0 || num == 1) && !Physics.Raycast (transform.position, rayUp, 1)) {
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
		transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Sword") {
			Destroy(col.gameObject);
			health--;
			if (health <= 0) Destroy(this.gameObject);
		}
	}
}
