﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
	public Room[,] map1;
	public float room_height;
	public float room_width;
	public float x_start = 7.52f;
	public float y_start = 5.79f;
	public float z = -11f;

	public int active_row_index = -1;
	public int active_col_index = -1;

	public GameObject[] enemies;

	public PlayerController pc;

//	public RoomController() {
//	}

	// Use this for initialization
	void Start () {
		room_height = 11f;
		room_width = 16f;

		map1 = new Room[6, 6];
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 6; j++) {
				Vector3 cam_pos = new Vector3 ((5 - i) * room_height + y_start, j * room_width + x_start, z);
				map1 [i, j] = Room.MakeNewRoom (cam_pos, this);
				map1 [i, j].FindMinMax ();
			}
		}

		map1 [0, 1].real_room = true;
		map1 [0, 2].real_room = true;
		map1 [1, 2].real_room = true;
		map1 [1, 4].real_room = true;
		map1 [1, 5].real_room = true;
		map1 [2, 0].real_room = true;
		map1 [2, 1].real_room = true;
		map1 [2, 2].real_room = true;
		map1 [2, 3].real_room = true;
		map1 [2, 4].real_room = true;
		map1 [3, 1].real_room = true;
		map1 [3, 2].real_room = true;
		map1 [3, 3].real_room = true;
		map1 [4, 2].real_room = true;
		map1 [5, 1].real_room = true;
		map1 [5, 2].real_room = true;
		map1 [5, 3].real_room = true;

		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 6; j++) {
				if (map1 [i, j].real_room) {
					map1 [i, j].cam_pos = new Vector3 ((5 - i) * room_height + y_start, j * room_width + x_start, z);
				}
			}
		}

		//literally the hackiest hardcoding of everything in every room
		map1 [0, 1].enemy_type = "Spiketrap";
		map1 [0, 1].num_enemies_total = 4;
		map1 [0, 1].num_enemies_left = 4;

		map1 [0, 2].needs_bomb_pickup = true;
		map1 [0, 2].enemy_type = "Goriya";
		map1 [0, 2].num_enemies_total = 3;
		map1 [0, 2].num_enemies_left = 3;

		map1 [1, 2].needs_key_pickup = true;
		map1 [1, 2].enemy_type = "Stalfos";
		map1 [1, 2].num_enemies_total = 3;
		map1 [1, 2].num_enemies_left = 3;

		map1 [1, 4].enemy_type = "Aquamentus"; //dragon boss
		map1 [1, 4].num_enemies_total = 1;
		map1 [1, 4].num_enemies_left = 1;

		map1 [1, 5].enemy_type = "None";
		map1 [1, 5].num_enemies_total = 0;
		map1 [1, 5].num_enemies_left = 0;
		map1 [1, 5].has_triforce = true;

		map1 [2, 0].enemy_type = "Fire";
		map1 [2, 0].num_enemies_total = 2;
		map1 [2, 0].num_enemies_left = 2;

		map1 [2, 1].enemy_type = "Gel";
		map1 [2, 1].num_enemies_total = 3;
		map1 [2, 1].num_enemies_left = 3;

		map1 [2, 2].enemy_type = "Gel";
		map1 [2, 2].num_enemies_total = 5;
		map1 [2, 2].num_enemies_left = 5;
		map1 [2, 2].needs_map_pickup = true;

		map1 [2, 3].enemy_type = "Goriya";
		map1 [2, 3].num_enemies_total = 3;
		map1 [2, 3].num_enemies_left = 3;
		map1 [2, 3].needs_boomerang_pickup = true;
		map1 [2, 3].needs_bomb_pickup = true;

		map1 [2, 4].enemy_type = "WallMaster";

		map1 [3, 1].enemy_type = "Keese";
		map1 [3, 1].num_enemies_total = 6;
		map1 [3, 1].num_enemies_left = 6;
		map1 [3, 1].must_kill_all_enemies = true;

		map1 [3, 2].enemy_type = "Stalfos";
		map1 [3, 2].num_enemies_total = 5;
		map1 [3, 2].num_enemies_left = 5;
		map1 [3, 2].needs_key_pickup = true;

		map1 [3, 3].enemy_type = "Keese";
		map1 [3, 3].num_enemies_total = 8;
		map1 [3, 3].num_enemies_left = 8;
		map1 [3, 3].needs_compass_pickup = true;
		map1 [3, 3].is_active = false;

		map1 [4, 2].enemy_type = "Stalfos";
		map1 [4, 2].num_enemies_total = 3;
		map1 [4, 2].num_enemies_left = 3;

		map1 [5, 1].enemy_type = "Keese";
		map1 [5, 1].num_enemies_total = 3;
		map1 [5, 1].num_enemies_left = 3;
		map1 [5, 1].needs_key_pickup = true;

		map1 [5, 3].enemy_type = "Stalfos";
		map1 [5, 3].num_enemies_total = 5;
		map1 [5, 3].num_enemies_left = 5;
		map1 [5, 3].needs_key_pickup = true;

		map1 [5, 2].is_active = true;
		active_row_index = 5;
		active_col_index = 2;
//		print ("INITIAL ACTIVE ROW INDEX " + active_row_index);
//		print ("INITIAL ACTIVE COL INDEX " + active_col_index);
	}
	
	// Update is called once per frame
	void Update () {
		if ((CameraPan.c.panning_down || CameraPan.c.panning_up || CameraPan.c.panning_left || CameraPan.c.panning_right)
			&& active_row_index != -1 && active_col_index != -1) {
			if (map1 [active_row_index, active_col_index].things_inside_room.Count > 0) {
				foreach (GameObject go in map1[active_row_index, active_col_index].things_inside_room) {
					Destroy (go);
				}
				foreach (Vector2 v in map1[active_row_index, active_col_index].init_pos_of_enemies) {
					map1 [active_row_index, active_col_index].init_pos_of_enemies.Remove (v);
				}
			}
			map1 [active_row_index, active_col_index].is_active = false;
			print ("camera is panning!");
			active_row_index = -1;
			active_col_index = -1;
		}
		if (active_row_index == -1 && active_col_index == -1
		    && !(CameraPan.c.panning_down || CameraPan.c.panning_up
		    || CameraPan.c.panning_left || CameraPan.c.panning_right)) {
			ChangeActiveRoom ();
			for (int i = 0; i < 6; i++) {
				for (int j = 0; j < 6; j++) {
					map1 [i, j].FindMinMax ();
				}
			}
			if (map1 [active_row_index, active_col_index].enemy_type == "Spiketrap") {
				map1 [active_row_index, active_col_index].InstantiateSpiketraps ();
			} else if (map1[active_row_index, active_col_index].enemy_type != "WallMaster") {
				map1 [active_row_index, active_col_index].InstantiateEnemies ();
			}
		}
	}

	void ChangeActiveRoom() {
		//print ("current cam pos y " + this.transform.position.y);
		float row_index = (this.transform.position.y - y_start) / room_height;
		//print ("row index = " + row_index);
		//print ("current cam pos x " + this.transform.position.x);
		row_index = 5f - row_index;
		float col_index = (this.transform.position.x - x_start) / room_width;
		//print ("col index = " + col_index);
		print ("Row_index = " + row_index);
		print ("Col_index = " + col_index);
		map1 [(int)row_index, (int)col_index].is_active = true;
		active_row_index = (int)row_index;
		active_col_index = (int)col_index;
	}
}