using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
	public static RoomController rc;

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

	public GameObject key_prefab;
	public GameObject compass_prefab;
	public GameObject map_prefab;
	public GameObject boomerang_prefab;
	public GameObject pushable_block_prefab;

//	public RoomController() {
//	}

	// Use this for initialization
	void Start () {
		rc = this;
		room_height = 11f;
		room_width = 16f;

		map1 = new Room[6, 6];
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 6; j++) {
				Vector3 cam_pos = new Vector3 (i * room_width + x_start, (5-j) * room_height + y_start, z);
				 //map1 [j, i] = Room.MakeNewRoom (cam_pos, this, j, i);
				map1 [j, i] = Room.MakeNewRoom (cam_pos, j, i);
			}
		}

		map1 [0, 1].real_room = true;
		map1 [0, 2].real_room = true;
		map1 [1, 1].real_room = true;
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

		//literally the hackiest hardcoding of everything in every room
		map1 [0, 1].enemy_type = "Spiketrap";
		map1 [0, 1].num_enemies_total = 4;
		map1 [0, 1].num_enemies_left = 4;
		map1 [0, 1].SetEnemyPrefab ();
		map1 [0, 1].has_push_block = true;
		map1 [0, 1].num_push_blocks_total = 1;
		map1 [0, 1].num_push_blocks_left = 1;

		map1 [0, 2].enemy_type = "Goriya";
		map1 [0, 2].num_enemies_total = 3;
		map1 [0, 2].num_enemies_left = 3;
		map1 [0, 2].SetEnemyPrefab ();

		map1 [1, 1].enemy_type = "Keese";
		map1 [1, 1].num_enemies_total = 4;
		map1 [1, 1].num_enemies_left = 4;
		map1 [1, 1].SetEnemyPrefab ();

		map1 [1, 2].needs_key_pickup = true;
		map1 [1, 2].enemy_type = "Stalfos";
		map1 [1, 2].num_enemies_total = 3;
		map1 [1, 2].num_enemies_left = 3;
		map1 [1, 2].SetEnemyPrefab ();

		map1 [1, 4].enemy_type = "Aquamentus"; //dragon boss
		map1 [1, 4].num_enemies_total = 1;
		map1 [1, 4].num_enemies_left = 1;
		map1 [1, 4].SetEnemyPrefab ();

		map1 [1, 5].enemy_type = "None";
		map1 [1, 5].num_enemies_total = 0;
		map1 [1, 5].num_enemies_left = 0;
		map1 [1, 5].has_triforce = true;
		map1 [1, 5].SetEnemyPrefab ();

//		map1 [2, 0].enemy_type = "Fire";
//		map1 [2, 0].num_enemies_total = 2;
//		map1 [2, 0].num_enemies_left = 2;
//		map1 [2, 0].SetEnemyPrefab ();

		map1 [2, 1].enemy_type = "Gel";
		map1 [2, 1].num_enemies_total = 3;
		map1 [2, 1].num_enemies_left = 3;
		map1 [2, 1].SetEnemyPrefab ();
		map1 [2, 1].has_push_block = true;
		map1 [2, 1].num_push_blocks_total = 1;
		map1 [2, 1].num_push_blocks_left = 1;

		map1 [2, 2].enemy_type = "Gel";
		map1 [2, 2].num_enemies_total = 5;
		map1 [2, 2].num_enemies_left = 5;
		map1 [2, 2].needs_map_pickup = true;
		map1 [2, 2].SetEnemyPrefab ();

		map1 [2, 3].enemy_type = "Goriya";
		map1 [2, 3].num_enemies_total = 3;
		map1 [2, 3].num_enemies_left = 3;
		map1 [2, 3].needs_boomerang_pickup = true;
		map1 [2, 3].SetEnemyPrefab ();

		map1 [2, 4].enemy_type = "WallMaster";
		map1 [2, 4].SetEnemyPrefab ();

		map1 [3, 1].enemy_type = "Keese";
		map1 [3, 1].num_enemies_total = 6;
		map1 [3, 1].num_enemies_left = 6;
		map1 [3, 1].must_kill_all_enemies = true;
		map1 [3, 1].SetEnemyPrefab ();

		map1 [3, 2].enemy_type = "Stalfos";
		map1 [3, 2].num_enemies_total = 5;
		map1 [3, 2].num_enemies_left = 5;
		map1 [3, 2].needs_key_pickup = true;
		map1 [3, 2].SetEnemyPrefab ();

		map1 [3, 3].enemy_type = "Keese";
		map1 [3, 3].num_enemies_total = 8;
		map1 [3, 3].num_enemies_left = 8;
		map1 [3, 3].needs_compass_pickup = true;
		map1 [3, 3].is_active = false;
		map1 [3, 3].SetEnemyPrefab ();

		map1 [4, 2].enemy_type = "Stalfos";
		map1 [4, 2].num_enemies_total = 3;
		map1 [4, 2].num_enemies_left = 3;
		map1 [4, 2].SetEnemyPrefab ();

		map1 [5, 1].enemy_type = "Keese";
		map1 [5, 1].num_enemies_total = 3;
		map1 [5, 1].num_enemies_left = 3;
		map1 [5, 1].needs_key_pickup = true;
		map1 [5, 1].SetEnemyPrefab ();

		map1 [5, 3].enemy_type = "Stalfos";
		map1 [5, 3].num_enemies_total = 5;
		map1 [5, 3].num_enemies_left = 5;
		map1 [5, 3].needs_key_pickup = true;
		map1 [5, 3].SetEnemyPrefab ();

		map1 [5, 2].is_active = true;
		active_row_index = 5;
		active_col_index = 2;
		map1 [5, 2].FindMinMax ();
//		print ("INITIAL ACTIVE ROW INDEX " + active_row_index);
//		print ("INITIAL ACTIVE COL INDEX " + active_col_index);
	}
	
	// Update is called once per frame
	void Update () {
		if ((CameraPan.c.panning_down || CameraPan.c.panning_up || CameraPan.c.panning_left || CameraPan.c.panning_right)
			&& active_row_index != -1 && active_col_index != -1) {
			map1 [active_row_index, active_col_index].is_active = false;
			if (map1 [active_row_index, active_col_index].things_inside_room.Count > 0) {
				foreach (GameObject go in map1[active_row_index, active_col_index].things_inside_room) {
					Destroy (go);
				}
				map1 [active_row_index, active_col_index].things_inside_room.Clear();
				map1 [active_row_index, active_col_index].init_pos_of_enemies.Clear();
			}
			//print ("camera is panning!");
			active_row_index = -1;
			active_col_index = -1;
		}
		if (active_row_index == -1 && active_col_index == -1
		    && !(CameraPan.c.panning_down || CameraPan.c.panning_up
		    || CameraPan.c.panning_left || CameraPan.c.panning_right)) {
			ChangeActiveRoom ();
			map1 [active_row_index, active_col_index].FindMinMax ();
			if (map1 [active_row_index, active_col_index].enemy_type == "Spiketrap") {
				map1 [active_row_index, active_col_index].InstantiateSpiketraps ();
			} else if (map1 [active_row_index, active_col_index].enemy_type == "Aquamentus") {
				map1 [active_row_index, active_col_index].InstantiateAquamentus ();
			} else if (map1[active_row_index, active_col_index].enemy_type != "WallMaster"
				&& map1[active_row_index, active_col_index].enemy_type != "None") {
				if (!(active_row_index == 1 && active_col_index == 1))
					map1 [active_row_index, active_col_index].InstantiateEnemies ();
			}
			if (map1 [active_row_index, active_col_index].has_push_block && !map1[active_row_index, active_col_index].all_blocks_pushed) {
				map1 [active_row_index, active_col_index].SetPushableBlocks ();
			}
		}

		if (active_row_index == 1 && active_col_index == 1 && !map1[active_row_index, active_col_index].bats_instantiated) {
			print ("should instantiate bats in bow room now");
			map1 [active_row_index, active_col_index].InstantiateBatsInBowRoom ();
			map1 [active_row_index, active_col_index].bats_instantiated = true;
		}

		if (active_row_index != -1 && active_col_index != -1 
			&& map1[active_row_index, active_col_index].needs_key_pickup
			&& !map1[active_row_index, active_col_index].key_dropped) {
			print ("this room needs key pickup");
			if (map1[active_row_index, active_col_index].num_enemies_left == 0 
				&& !map1[active_row_index, active_col_index].key_picked_up) {
				print ("can drop key now because no enemies left");
				map1[active_row_index, active_col_index].init_pos_of_enemies.Clear ();
				Vector3 temp = map1[active_row_index, active_col_index].FindFreeTile ();
				GameObject key = Instantiate (key_prefab) as GameObject;
				key.transform.position = temp;
				map1[active_row_index, active_col_index].things_inside_room.Add (key);
				map1 [active_row_index, active_col_index].key_dropped = true;
			}
		}
			
		if (active_row_index != -1 && active_col_index != -1 && map1[active_row_index, active_col_index].must_kill_all_enemies) {
			print ("must kill all enemies in this room");
			print ("num enemies left is " + map1 [active_row_index, active_col_index].num_enemies_left);
			Room cur_room = map1 [active_row_index, active_col_index];
			if (cur_room.num_enemies_left != 0) {
				print ("putting jagged door on dat shit");
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].SetTile (cur_room.tile_xmax + 1, cur_room.tile_ymin + 3, 100);
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().enabled = true;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().isTrigger = false;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().center = Vector3.zero;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().size = new Vector3 (1f, 0.8f, 1f);
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].gameObject.tag = "JaggedDoor";
				ShowMapOnCamera.MAP [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3] = 100;
			} else {
				print ("num enemies left is " + cur_room.num_enemies_left);
				print ("unlocking the door omgeeee");
				map1[active_row_index, active_col_index].all_enemies_killed = true;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].SetTile (cur_room.tile_xmax + 1, cur_room.tile_ymin + 3, 48);
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().enabled = true;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().size = new Vector3(1f, 0.8f, 1f);
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().isTrigger = true;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].gameObject.layer = 11;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3].gameObject.tag = "DoorRight";
				ShowMapOnCamera.MAP [cur_room.tile_xmax + 1, cur_room.tile_ymin + 3] = 48;
			}
		}

		if (active_row_index != -1 && active_col_index != -1 
			&& map1[active_row_index, active_col_index].needs_boomerang_pickup) {
			if (map1[active_row_index, active_col_index].num_enemies_left == 0 
				&& !map1[active_row_index, active_col_index].boomerang_picked_up) {
				map1[active_row_index, active_col_index].init_pos_of_enemies.Clear ();
				Vector3 temp = map1[active_row_index, active_col_index].FindFreeTile ();
				GameObject boomerang = Instantiate (boomerang_prefab) as GameObject;
				boomerang.GetComponent<BoxCollider> ().isTrigger = true;
				boomerang.transform.position = temp;
				map1[active_row_index, active_col_index].things_inside_room.Add (boomerang);
			}
		}

		if (active_row_index != -1 && active_col_index != -1 
			&& map1[active_row_index, active_col_index].all_blocks_pushed) {
			if (active_row_index == 2 && active_col_index == 1) {
				Room cur_room = map1 [active_row_index, active_col_index];
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmin - 1, cur_room.tile_ymin + 3].SetTile (cur_room.tile_xmin - 1, cur_room.tile_ymin + 3, 51);
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmin - 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().enabled = true;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmin - 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().size = new Vector3(1f, 0.8f, 1f);
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmin - 1, cur_room.tile_ymin + 3].GetComponent<BoxCollider> ().isTrigger = true;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmin - 1, cur_room.tile_ymin + 3].gameObject.layer = 11;
				ShowMapOnCamera.MAP_TILES [cur_room.tile_xmin - 1, cur_room.tile_ymin + 3].gameObject.tag = "DoorLeft";
				ShowMapOnCamera.MAP [cur_room.tile_xmin - 1, cur_room.tile_ymin + 3] = 51;
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
		//print ("Row_index = " + row_index);
		//print ("Col_index = " + col_index);
		map1 [(int)row_index, (int)col_index].is_active = true;
		active_row_index = (int)row_index;
		active_col_index = (int)col_index;
	}
}
