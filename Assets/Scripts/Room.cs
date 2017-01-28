﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour {
	static public Room r;
	public RoomController room_controller;

	System.Random random = new System.Random();

	public int row, col;

	public bool is_active = false;
	public bool real_room = false;

	public int num_enemies_total = 0;
	public int num_enemies_left = 0;
	public string enemy_type = "None";

	public bool needs_key_pickup = false;
	public bool needs_compass_pickup = false;
	public bool needs_map_pickup = false;
	public bool needs_boomerang_pickup = false;
	public bool has_triforce = false;
	public bool must_kill_all_enemies = false;
	public bool has_push_block = false;

	public bool key_picked_up = false;
	public bool compass_picked_up = false;
	public bool map_picked_up = false;
	public bool boomerang_picked_up = false;
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

	public GameObject enemy_prefab;

	void Start() {
	}

	static public Room MakeNewRoom(Vector3 cam_pos, RoomController rc, int ex, int ey) {
		Room room = new Room ();
		room.cam_pos = cam_pos;
		room.room_controller = rc;

		room.row = ex;
		room.col = ey;

		room.xmax = cam_pos.x + 6f;
		room.xmin = cam_pos.x - 6f;
		room.ymax = cam_pos.y - 2f + 3.5f;
		room.ymin = cam_pos.y - 2f - 3.5f;
		return room;
	}

	// Use this for initialization
	public void FindMinMax () {
//		print ("cam_pos " + cam_pos);
//		print ("xmin " + xmin);
//		print ("xmax " + xmax);
//		print ("ymin " + ymin);
//		print ("ymax " + ymax);

		//print ("roomcontroller print message hi");
		bool found_corner = false;
		for (int i = 0; i < ShowMapOnCamera.S.h; i++) {
			for (int j = 0; j < ShowMapOnCamera.S.w; j++) {
				if (ShowMapOnCamera.MAP_TILES [j, i] != null) {
					//print ("map tiles at " + j + " " + i + " not null");
					//print ("map tile position " + ShowMapOnCamera.MAP_TILES [j, i].transform.position);
					if (ShowMapOnCamera.MAP_TILES [j, i].transform.position.x > xmin
					    && ShowMapOnCamera.MAP_TILES [j, i].transform.position.y > ymin) {
//						print ("setting tile_x = " + tile_xmin + " and tile_y = " + tile_ymax);
						tile_xmin = j;
						tile_ymin = i;
						found_corner = true;
						break;
					}
				}
			}
			if (found_corner) {
				break;
			}
		}

		tile_xmax = tile_xmin + 11;
		tile_ymax = tile_ymin + 6;

//		print ("tile xmin " + tile_xmin);
//		print ("tile xmax " + tile_xmax);
//		print ("tile ymin " + tile_ymin);
//		print ("tile ymax " + tile_ymax);
//		print ("tile xmin ymin " + ShowMapOnCamera.MAP_TILES [tile_xmin, tile_ymin].transform.position);
//		print ("tile xmax ymax " + ShowMapOnCamera.MAP_TILES [tile_xmax, tile_ymax].transform.position);
	}

	public void SetEnemyPrefab() {
		if (enemy_type == "Stalfos") {
			enemy_prefab = room_controller.enemies [0];
		} else if (enemy_type == "Gel") {
			enemy_prefab = room_controller.enemies [1];
		} else if (enemy_type == "Goriya") {
			enemy_prefab = room_controller.enemies [2];
		} else if (enemy_type == "Spiketrap") {
			enemy_prefab = room_controller.enemies [3];
		} else if (enemy_type == "Keese") {
			enemy_prefab = room_controller.enemies [4];
		} else if (enemy_type == "Aquamentus") {
			enemy_prefab = room_controller.enemies [5];
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (is_active && needs_key_pickup) {
			print ("this room needs key pickup");
			if (num_enemies_left == 0 && !key_picked_up) {
				print ("can drop key now because no enemies left");
				//I KNOW WHAT'S WRONG I DIDN'T SET THE VARIABLES FJDKSFJDLSKFJKDSLJFKLSJS
				init_pos_of_enemies.Clear ();
				Vector3 temp = FindFreeTile ();
				GameObject key = Instantiate (room_controller.key_prefab) as GameObject;
				key.transform.position = temp;
				things_inside_room.Add (key);
			}
		}

		if (is_active && must_kill_all_enemies) {
			//I DON'T KNOW HOW TO CHANGE THE DOOR
		}

		if (is_active && needs_boomerang_pickup) {
			if (num_enemies_left == 0 && !boomerang_picked_up) {
				init_pos_of_enemies.Clear ();
				Vector3 temp = FindFreeTile ();
				GameObject boomerang = Instantiate (room_controller.boomerang_prefab) as GameObject;
				boomerang.GetComponent<BoxCollider> ().isTrigger = true;
				boomerang.transform.position = temp;
				things_inside_room.Add (boomerang);
			}
		}
	}

	public void InstantiateEnemies() {
		for (int i = 0; i < num_enemies_left; i++) {
			Vector3 new_pos = FindFreeTile ();
			GameObject go = Instantiate (enemy_prefab) as GameObject;
			go.transform.position = new_pos;
			if (enemy_prefab.name == "Skeleton") {
				go.GetComponent<Skeleton> ().room = this;
			} else if (enemy_prefab.name == "Goriya") {
				go.GetComponent<Goriya> ().room = this;
			} else if (enemy_prefab.name == "Gel") {
				go.GetComponent<Gel> ().room = this;
			} else if (enemy_prefab.name == "Aquamentus") {
				go.GetComponent<Aquamentus> ().room = this;
			} else if (enemy_prefab.name == "Bat") {
				go.GetComponent<Bat> ().room = this;
			}
			things_inside_room.Add (go);
		}
	}

	public void InstantiateSpiketraps() {
		GameObject st1 = Instantiate (enemy_prefab) as GameObject;
		GameObject st2 = Instantiate (enemy_prefab) as GameObject;
		GameObject st3 = Instantiate (enemy_prefab) as GameObject;
		GameObject st4 = Instantiate (enemy_prefab) as GameObject;

		st1.transform.position = ShowMapOnCamera.MAP_TILES[tile_xmin, tile_ymin+1].transform.position;
		st2.transform.position = ShowMapOnCamera.MAP_TILES[tile_xmin, tile_ymax+1].transform.position;
		st3.transform.position = ShowMapOnCamera.MAP_TILES[tile_xmax, tile_ymin+1].transform.position;
		st4.transform.position = ShowMapOnCamera.MAP_TILES[tile_xmax, tile_ymax+1].transform.position;

		things_inside_room.Add(st1);
		things_inside_room.Add(st2);
		things_inside_room.Add(st3);
		things_inside_room.Add(st4);
	}

	public void InstantiateAquamentus() {
		GameObject aq = Instantiate (enemy_prefab) as GameObject;
		aq.transform.position = new Vector3 (76f, 49.5f, 0f);
		things_inside_room.Add (aq);
	}

	Vector3 FindFreeTile() {
		int temp_xtile = random.Next (tile_xmin, tile_xmax);
		int temp_ytile = random.Next (tile_ymin, tile_ymax);
//		print ("tile xmin " + tile_xmin);
//		print ("tile xmax " + tile_xmax);
//		print ("tile ymin " + tile_ymin);
//		print ("tile ymax " + tile_ymax);
//		print ("temp xtile " + temp_xtile);
//		print ("temp ytile " + temp_ytile);
//		if (ShowMapOnCamera.MAP_TILES [temp_xtile, temp_ytile] == null)
//			print ("this tile is fuckin null bitch");
		while (ShowMapOnCamera.MAP_TILES[temp_xtile, temp_ytile] == null
			|| ShowMapOnCamera.MAP_TILES[temp_xtile, temp_ytile].gameObject.tag != "Floor"
			|| init_pos_of_enemies.Contains(new Vector2(temp_xtile, temp_ytile))) {
			temp_xtile = random.Next (tile_xmin, tile_xmax);
			temp_ytile = random.Next (tile_ymin, tile_ymax);
		}
		init_pos_of_enemies.Add (new Vector2 (temp_xtile, temp_ytile));
		float temp_x = Mathf.Floor(ShowMapOnCamera.MAP_TILES [temp_xtile, temp_ytile].transform.position.x);
		float temp_y = Mathf.Floor(ShowMapOnCamera.MAP_TILES [temp_xtile, temp_ytile].transform.position.y);
		return new Vector3 (temp_x, temp_y, 0f);
	}

	public void SetPushableBlocks() {
		GameObject go = Instantiate (room_controller.pushable_block_prefab) as GameObject;
		if (row == 0 && col == 1) {
			go.transform.position = new Vector3 (22f, 60f, 0f);
			go.GetComponent<PushableBlock> ().target = new Vector3 (22f, 61f, 0f);
			//more stuff idk???
		} else if (row == 2 && col == 1) {
			go.transform.position = new Vector3 (23f, 38f, 0f);
			go.GetComponent<PushableBlock> ().target = new Vector3 (24f, 38f, 0f);
		}
	}
}
