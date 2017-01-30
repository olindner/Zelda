using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushFarBlock : MonoBehaviour {

	public Vector3 target;
	public Vector3 local_target;
	public Vector3 last_pos;
	public Vector3 original_pos;
	public bool is_moving = false;
	public bool done_moving = false;
	public bool done_moving_forever = false;

	// Use this for initialization
	void Start () {
		last_pos = original_pos;
	}
	
	// Update is called once per frame
	void Update () {
		if (is_moving) {
			Vector3 destination = local_target;
			destination = Vector3.Lerp (this.transform.position, local_target, 0.1f);
			transform.position = destination;
			if ((Mathf.Abs (transform.position.x - local_target.x) <= 0.1f) 
				&& (Mathf.Abs (transform.position.y - local_target.y) <= 0.1f)
				&& (Mathf.Abs (transform.position.z - local_target.z) <= 0.1f)) {
				transform.position = local_target;
				is_moving = false;
				done_moving = true;
				last_pos = local_target;
				if (local_target == target) {
					//print ("moving block got to target");
					RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].num_push_blocks_left--;
					print ("num blocks left in here is " + RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].num_push_blocks_left);
					if (RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].num_push_blocks_left == 0) {
						RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].all_blocks_pushed = true;
					}
					done_moving_forever = true;
				}
			}
		} else if (!done_moving_forever) {
			transform.position = last_pos;
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Player" && !done_moving_forever && !done_moving && !is_moving) {
			Vector3 direction_of_push = this.transform.position - original_pos;
			direction_of_push = direction_of_push.normalized;
			Vector3 temp = last_pos;
			if (PlayerController.instance.current_direction == Direction.NORTH && direction_of_push == Vector3.up) {
				temp.y += 1;
				local_target = temp;
				this.is_moving = true;
			} else if (PlayerController.instance.current_direction == Direction.EAST && direction_of_push == Vector3.right) {
				temp.x += 1;
				local_target = temp;
				this.is_moving = true;
			} else if (PlayerController.instance.current_direction == Direction.SOUTH && direction_of_push == Vector3.down) {
				temp.y -= 1;
				local_target = temp;
				this.is_moving = true;
			} else if (PlayerController.instance.current_direction == Direction.WEST && direction_of_push == Vector3.left) {
				temp.x -= 1;
				local_target = temp;
				this.is_moving = true;
			} else {
				if (RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].all_blocks_pushed) {
					this.transform.position = target;
				} else {
					this.transform.position = last_pos;
				}
			}
		} else if (coll.gameObject.tag == "Sword") {
			if (RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].all_blocks_pushed) {
				this.transform.position = target;
			} else {
				this.transform.position = last_pos;
			}
			Destroy (coll.gameObject);
		}
	}
}
