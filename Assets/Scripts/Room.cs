using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	static public Room r;

	public bool is_active = false;
	public bool real_room = false;

	public int num_enemies_total = 0;
	public int num_enemies_left = 0;
	public string enemy_type = "None";

	public bool needs_key_pickup = false;
	public bool needs_compass_pickup = false;
	public bool needs_special_key_pickup = false;
	public bool needs_map_pickup = false;
	public bool needs_boomerang_pickup = false;
	public bool needs_bomb_pickup = false;
	public bool needs_clock_pickup = false;
	public bool has_triforce = false;
	public bool must_kill_all_enemies = false;

	public bool key_picked_up = false;
	public bool compass_picked_up = false;
	public bool special_key_picked_up = false;
	public bool map_picked_up = false;
	public bool boomerang_picked_up = false;
	public bool bomb_picked_up = false;
	public bool clock_picked_up = false;
	public bool triforce_picked_up = false;
	public bool all_enemies_killed = false;

	public int num_times_attacked_old_man = 0; //once, only right fire attacks; twice, both fires attack 

	public List<GameObject> things_inside_room; //contains enemies, collectibles, etc
	public Vector3 cam_pos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
