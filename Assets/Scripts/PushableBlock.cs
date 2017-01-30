using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour {

	public Vector3 target;
	public Vector3 original_pos;
	bool is_moving = false;
	bool done_moving = false;

	public RoomController rc;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (is_moving) {
			Vector3 destination = target;
			destination = Vector3.Lerp (this.transform.position, target, 0.1f);
			transform.position = destination;
			if ((Mathf.Abs (transform.position.x - target.x) <= 0.1f) && Mathf.Abs (transform.position.y - target.y) <= 0.1f
				&& Mathf.Abs (transform.position.z - target.z) <= 0.1f) {
				transform.position = target;
				is_moving = false;
				done_moving = true;
				print ("moving block got to target");
				rc.map1 [rc.active_row_index, rc.active_col_index].num_push_blocks_left--;
				if (rc.map1 [rc.active_row_index, rc.active_col_index].num_push_blocks_left == 0) {
					rc.map1 [rc.active_row_index, rc.active_col_index].all_blocks_pushed = true;
				}
			}
		} else if (!done_moving) {
			transform.position = original_pos;
		}

	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Player" && !done_moving && !is_moving) {
			Vector3 direction_of_push = this.transform.position - original_pos;
			direction_of_push = direction_of_push.normalized;
			if (PlayerController.instance.current_direction == Direction.NORTH && direction_of_push == Vector3.up
			    && target.x == original_pos.x && (target.y - 1f) == original_pos.y) {
				this.is_moving = true;
			} else if (PlayerController.instance.current_direction == Direction.EAST && direction_of_push == Vector3.right
			           && target.x - 1 == original_pos.x && target.y == original_pos.y) {
				this.is_moving = true;
			} else {
				if (RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].all_blocks_pushed) {
					this.transform.position = target;
				} else {
					this.transform.position = original_pos;
				}
			}
		} else if (coll.gameObject.tag == "Sword") {
			if (RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].all_blocks_pushed) {
				this.transform.position = target;
			} else {
				this.transform.position = original_pos;
			}
			Destroy (coll.gameObject);
		}
	}
}
