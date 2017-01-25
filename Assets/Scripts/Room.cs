using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour {
	static public Room r;
	public RoomController room_controller;

	System.Random random = new System.Random();

	public bool is_active = false;
	public bool real_room = false;

	public int num_enemies_total = 0;
	public int num_enemies_left = 0;
	public string enemy_type = "None";
	public GameObject enemy_prefab;

	public bool needs_key_pickup = false;
	public bool needs_compass_pickup = false;
	public bool needs_map_pickup = false;
	public bool needs_boomerang_pickup = false;
	public bool needs_bomb_pickup = false;
	public bool has_triforce = false;
	public bool must_kill_all_enemies = false;

	public bool key_picked_up = false;
	public bool compass_picked_up = false;
	public bool map_picked_up = false;
	public bool boomerang_picked_up = false;
	public bool bomb_picked_up = false;
	public bool triforce_picked_up = false;
	public bool all_enemies_killed = false;

	public int num_times_attacked_old_man = 0; //once, only right fire attacks; twice, both fires attack 

	public List<GameObject> things_inside_room = new List<GameObject>(); //contains enemies, collectibles, etc
	public List<Vector2> init_pos_of_enemies = new List<Vector2>();
	public Vector3 cam_pos;

	public float xmax;
	public float xmin;
	public float ymax;
	public float ymin;

	public int tile_xmin;
	public int tile_xmax;
	public int tile_ymin;
	public int tile_ymax;

	void Start() {
	}

	static public Room MakeNewRoom(Vector3 cam_pos, RoomController rc) {
		Room room = new Room ();
		room.cam_pos = cam_pos;
		room.room_controller = rc;
		return room;
	}

	// Use this for initialization
	public void FindMinMax () {
		xmax = cam_pos.x + 6f;
		xmin = cam_pos.x - 6f;
		ymax = cam_pos.y - 4f + 3.5f;
		ymin = cam_pos.y - 4f - 3.5f;

		//print ("roomcontroller print message hi");
		for (int i = 0; i < ShowMapOnCamera.S.h; i++) {
			for (int j = 0; j < ShowMapOnCamera.S.w; j++) {
				if (ShowMapOnCamera.MAP_TILES [j, i] != null) {
					if (ShowMapOnCamera.MAP_TILES [j, i].transform.position.x > xmin
					    && ShowMapOnCamera.MAP_TILES [j, i].transform.position.y <= ymax) {
						tile_xmin = j;
						tile_ymax = i;
					}
				}
			}
		}

		tile_xmax = tile_xmin + 12;
		tile_ymin = tile_ymax - 7;

		if (enemy_type == "Stalfos") {
			enemy_prefab = room_controller.enemies[0];
		} else if (enemy_type == "Gel") {
			enemy_prefab = room_controller.enemies[1];
		} else if (enemy_type == "WallMaster") {
			enemy_prefab = room_controller.enemies[2];
		} else if (enemy_type == "Spiketrap") {
			enemy_prefab = room_controller.enemies[3];
		} else if (enemy_type == "Aquamentus") {
			enemy_prefab = room_controller.enemies[4];
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InstantiateEnemies() {
		for (int i = 0; i < num_enemies_total; i++) {
			Vector3 new_pos = FindFreeTile ();
			GameObject go = Instantiate (enemy_prefab) as GameObject;
			go.transform.position = new_pos;
			things_inside_room.Add (go);
		}
	}

	public void InstantiateSpiketraps() {
		GameObject st1 = Instantiate (enemy_prefab) as GameObject;
		GameObject st2 = Instantiate (enemy_prefab) as GameObject;
		GameObject st3 = Instantiate (enemy_prefab) as GameObject;
		GameObject st4 = Instantiate (enemy_prefab) as GameObject;

		st1.transform.position = ShowMapOnCamera.MAP_TILES[tile_xmin, tile_ymin].transform.position;
		st2.transform.position = ShowMapOnCamera.MAP_TILES[tile_xmin, tile_ymax].transform.position;
		st3.transform.position = ShowMapOnCamera.MAP_TILES[tile_xmax, tile_ymin].transform.position;
		st4.transform.position = ShowMapOnCamera.MAP_TILES[tile_xmax, tile_ymax].transform.position;


		things_inside_room.Add(st1);
		things_inside_room.Add(st2);
		things_inside_room.Add(st3);
		things_inside_room.Add(st4);
	}

	Vector3 FindFreeTile() {
		int temp_xtile = random.Next (tile_xmin, tile_xmax);
		int temp_ytile = random.Next (tile_ymin, tile_ymax);
		print ("tile xmin " + tile_xmin);
		print ("tile xmax " + tile_xmax);
		print ("tile ymin " + tile_ymin);
		print ("tile ymax " + tile_ymax);
		print ("temp xtile " + temp_xtile);
		print ("temp ytile " + temp_ytile);

		while (ShowMapOnCamera.MAP_TILES[temp_xtile, temp_ytile].gameObject.tag != "Floor"
			|| init_pos_of_enemies.Contains(new Vector2(temp_xtile, temp_ytile))) {
			temp_xtile = random.Next (tile_xmin, tile_xmax);
			temp_ytile = random.Next (tile_ymin, tile_ymax);
		}
		init_pos_of_enemies.Add (new Vector2 (temp_xtile, temp_ytile));
		float temp_x = Mathf.Floor(ShowMapOnCamera.MAP_TILES [temp_xtile, temp_ytile].transform.position.x);
		float temp_y = Mathf.Floor(ShowMapOnCamera.MAP_TILES [temp_xtile, temp_ytile].transform.position.y);
		return new Vector3 (temp_x, temp_y, 0f);
	}
}
