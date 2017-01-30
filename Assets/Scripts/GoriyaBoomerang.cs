using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriyaBoomerang : MonoBehaviour {

	private SpriteRenderer sr;
	public float speed = 5f;
	public Vector3 target;
	Vector3 origin;
//	public bool on_way_back = false;
	public Vector3 goriya_pos;
	private bool movingout;
	private bool movingin;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		origin = sr.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, 3*Time.time);

		if (sr.transform.position == origin) {
			movingout = true;
			movingin = false;
		}
		else if (sr.transform.position == target) {
			movingout = false;
			movingin = true;
		}

		if(movingout) sr.transform.position = Vector3.MoveTowards(sr.transform.position, target, Time.deltaTime * speed);
		else if (movingin) sr.transform.position = Vector3.MoveTowards(sr.transform.position, origin, Time.deltaTime * speed);
	}

	void OnCollisionEnter(Collision coll) { //shouldn't it be ANY collider?
		if (coll.gameObject.tag == "Wall" || coll.gameObject.tag == "DoorUp"
		    || coll.gameObject.tag == "DoorLeft" || coll.gameObject.tag == "DoorRight"
		    || coll.gameObject.tag == "DoorDown" || coll.gameObject.tag == "LockedDoorUp"
		    || coll.gameObject.tag == "LockedDoorLeft" || coll.gameObject.tag == "LockedDoorRight"
		    || coll.gameObject.tag == "Player") {
			movingin = true;
//			Vector3 new_direction = goriya_pos - this.transform.position;
//			this.gameObject.GetComponent<Rigidbody> ().velocity = new_direction.normalized * 5f;
		}
	}
}
