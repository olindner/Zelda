using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goriya : MonoBehaviour {

	public float speed;
	public Sprite[] horizSprites; //defaults left
	public Sprite[] upSprites;
	public Sprite[] downSprites;
	public float spriteDelay;
	private float spriteTimer;
	public float throwDelay;
	private float throwTimer;
	private bool isMoving =  false;
	Vector3 pos;
	private int dir;
	private bool isStunned = false;
	private int here = 0;

	public Room room;

	public Direction current_direction;

	public GameObject boomerang;
	public bool has_boomerang = true;
	private SpriteRenderer sr;

	//public GoriyaBoomerang current_gb;

	// Use this for initialization
	void Start () {
		spriteTimer = Time.time + spriteDelay;
		throwTimer = Time.time + throwDelay;
		pos = transform.position;
		dir = Random.Range(0,3);
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (transform.position == pos)
			isMoving = false;
		else
			isMoving = true;

		if (!isMoving && !isStunned) {
			int num = Random.Range (0, 15);

			//Make sure set position in middle of square
			CorrectPosition ();
			////////////////////////////////////////////

			Vector3 rayUp = transform.TransformDirection (Vector3.up);
			Vector3 rayDown = transform.TransformDirection (Vector3.down);
			Vector3 rayLeft = transform.TransformDirection (Vector3.left);
			Vector3 rayRight = transform.TransformDirection (Vector3.right);
			//RaycastHit hit;

			//Equal percentages each direction...
			if (num == 0 || num == 1) { //Up
				if (!Physics.Raycast (transform.position, rayUp, 1)) {
					pos += Vector3.up;
					dir = 0;
				}
			} else if (num == 2 || num == 3) { //Down
				if (!Physics.Raycast (transform.position, rayDown, 1)) {
					pos += Vector3.down;
					dir = 1;
				}
			} else if (num == 4 || num == 5) { //Left
				if (!Physics.Raycast (transform.position, rayLeft, 1)) {
					pos += Vector3.left;
					dir = 2;
				}
			} else if (num == 6 || num == 7) { //Right
				if (!Physics.Raycast (transform.position, rayRight, 1)) {
					pos += Vector3.right;
					dir = 3;
				}
			} 
			//...added likelihood to continue moving in same direction
			//0=UP, 1=DOWN, 2=LEFT, 3=RIGHT
			else if (num >= 8 && num <= 14) {
				if (dir == 0 && !Physics.Raycast (transform.position, rayUp, 1))
					pos += Vector3.up;
				else if (dir == 1 && !Physics.Raycast (transform.position, rayDown, 1))
					pos += Vector3.down;
				else if (dir == 2 && !Physics.Raycast (transform.position, rayLeft, 1))
					pos += Vector3.left;
				else if (dir == 3 && !Physics.Raycast (transform.position, rayRight, 1))
					pos += Vector3.right;
			}
		}

		if (Time.time >= spriteTimer) {
			if (dir == 0 && here == 0) { //Up
				sr.sprite = upSprites [here];
				here = 1;
			} else if (dir == 0 && here == 1) {
				sr.sprite = upSprites [here];
				here = 0;
			} else if (dir == 1 && here == 0) { //Down
				sr.sprite = downSprites [here];
				here = 1;
			} else if (dir == 1 && here == 1) {
				sr.sprite = downSprites [here];
				here = 0;
			} else if (dir == 2 && here == 0) { //Left
				sr.sprite = horizSprites [here];
				sr.flipX = false;
				here = 1;
			} else if (dir == 2 && here == 1) {
				sr.sprite = horizSprites [here];
				sr.flipX = false;
				here = 0;
			} else if (dir == 3 && here == 0) { //Right
				sr.sprite = horizSprites [here];
				sr.flipX = true;
				here = 1;
			} else if (dir == 3 && here == 1) {
				sr.sprite = horizSprites [here];
				sr.flipX = true;
				here = 0;
			}
			spriteTimer = Time.time + spriteDelay;
		}

		transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);

		if (has_boomerang && Time.time >= throwTimer) {
			//ThrowBoomerang ();
			GameObject go = Instantiate (boomerang);
			//go.GetComponent<SpriteRenderer>().gameObject.transform.position = transform.position;
			go.gameObject.transform.position = transform.position;
			print(go.gameObject.transform.position == transform.position);
//			//Vector3 current_pos = transform.position;
//			if (current_direction == Direction.NORTH) {
//				go.GetComponent<GoriyaBoomerang>().current_direction = Direction.NORTH;
//				go.GetComponent<GoriyaBoomerang>().target = new Vector3 (transform.position.x, transform.position.y + 4f, 0);
//			} else if (current_direction == Direction.EAST) {
//				go.GetComponent<GoriyaBoomerang>().current_direction = Direction.EAST;
//				go.GetComponent<GoriyaBoomerang>().target = new Vector3 (transform.position.x + 4f, transform.position.y, 0);
//			} else if (current_direction == Direction.SOUTH) {
//				go.GetComponent<GoriyaBoomerang>().current_direction = Direction.SOUTH;
//				go.GetComponent<GoriyaBoomerang>().target = new Vector3 (transform.position.x, transform.position.y - 4f, 0);
//			} else { //current direction is WEST
//				go.GetComponent<GoriyaBoomerang>().current_direction = Direction.WEST;
//				go.GetComponent<GoriyaBoomerang>().target = new Vector3 (transform.position.x - 4f, transform.position.y, 0);
//			}
			has_boomerang = false;
		}
	}

	void ThrowBoomerang() {
		//print("b4");
		//GameObject go = Instantiate (boomerang);
		//print("after");
//		if (go.GetComponent<GoriyaBoomerang> ().speed == 5f) {
//			print ("WHHHHAFJFLFDKA");
//		}
//		current_gb = new GoriyaBoomerang ();
//		current_gb.the_boomerang = go;
//		current_gb.goriya_pos = this.transform.position;
		//Vector3 current_pos = transform.position;
		if (current_direction == Direction.NORTH) {
			GameObject go = Instantiate (boomerang, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);
			go.GetComponent<GoriyaBoomerang>().target = new Vector3 (transform.position.x, transform.position.y + 3.5f, 0);
		} else if (current_direction == Direction.EAST) {
			GameObject go = Instantiate (boomerang, new Vector3(transform.position.x + 0.5f, transform.position.y, 0), Quaternion.identity);
			go.GetComponent<GoriyaBoomerang>().target = new Vector3 (transform.position.x + 3.5f, transform.position.y, 0);
		} else if (current_direction == Direction.SOUTH) {
			GameObject go = Instantiate (boomerang, new Vector3(transform.position.x, transform.position.y - 0.5f, 0), Quaternion.identity);
			go.GetComponent<GoriyaBoomerang>().target = new Vector3 (transform.position.x, transform.position.y - 3.5f, 0);
		} else { //current direction is WEST
			GameObject go = Instantiate (boomerang, new Vector3(transform.position.x - 0.5f, transform.position.y, 0), Quaternion.identity);
			go.GetComponent<GoriyaBoomerang>().target = new Vector3 (transform.position.x - 3.5f, transform.position.y, 0);
		}
		//go.transform.position = current_pos;
//		if (current_direction == Direction.NORTH) {
//			go.GetComponent<Rigidbody> ().velocity = Vector3.up * current_gb.speed;
//		} else if (current_direction == Direction.EAST) {
//			go.GetComponent<Rigidbody> ().velocity = Vector3.right * current_gb.speed;
//		} else if (current_direction == Direction.SOUTH) {
//			go.GetComponent<Rigidbody> ().velocity = Vector3.down * current_gb.speed;
//		} else { //curent direction is WEST
//			go.GetComponent<Rigidbody> ().velocity = Vector3.left * current_gb.speed;
//		}
		has_boomerang = false;
	}

	void OnCollisionEnter(Collision coll) { //caught it!
		if (coll.gameObject.tag == "GoriyaBoomerang") {
			Destroy (coll.gameObject);
			has_boomerang = true;
			throwTimer = Time.time + throwDelay;
		}
	}

	void CorrectPosition ()
	{
		float tempx;
		float tempy;
		if (transform.position.x - Mathf.Floor (transform.position.x) <= 0.5) {
			tempx = Mathf.Floor (transform.position.x);
		}
		else {
			tempx = Mathf.Ceil(transform.position.x);
		}

		if (transform.position.y - Mathf.Floor (transform.position.y) <= 0.5) {
			tempy = Mathf.Floor (transform.position.y);
		}
		else {
			tempy = Mathf.Ceil(transform.position.y);
		}
		transform.position = new Vector3(tempx, tempy, 0);
		isMoving = false; //hopefully make it pick new direction
	}
}
