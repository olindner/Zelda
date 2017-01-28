using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriyaBoomerang : MonoBehaviour {

	public float speed = 5f;
	public Vector3 target;
	public bool on_way_back = false;
	public Vector3 goriya_pos;

	public GameObject the_boomerang;
	public Direction current_direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, 3*Time.time);
		if (on_way_back) {
			Vector3 new_direction = goriya_pos - this.transform.position;
			this.gameObject.GetComponent<Rigidbody> ().velocity = new_direction.normalized * 5f;
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Wall" || coll.gameObject.tag == "DoorUp"
		    || coll.gameObject.tag == "DoorLeft" || coll.gameObject.tag == "DoorRight"
		    || coll.gameObject.tag == "DoorDown" || coll.gameObject.tag == "LockedDoorUp"
		    || coll.gameObject.tag == "LockedDoorLeft" || coll.gameObject.tag == "LockedDoorRight"
		    || coll.gameObject.tag == "Player") {
			on_way_back = true;
			Vector3 new_direction = goriya_pos - this.transform.position;
			this.gameObject.GetComponent<Rigidbody> ().velocity = new_direction.normalized * 5f;
		}
	}
}
