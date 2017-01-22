﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerController : MonoBehaviour {

	//public int frame = 0; //for debugging
	//fuck you austin jk i love you

    /* Inspector Tunables */
    public float PlayerMovementVelocity;
	public int showDamageForFrames = 2;

    Rigidbody rb;
	//public bool receive_damage = false;
	public Material[] materials;
	public Material[] tile_materials;
	public int remainingDamageFrames = 0;
	public int remainingDamageFlashes = 0;
	public Color[] originalColors;

	public int num_cooldown_frames = 0;
	public int num_cooldown_weapon_frames = 0;
	public int num_cooldown_attack_frames = 0;
	public float damage_hopback_vel = 2.65f; //how fast Link hops back when damaged in comparison to velocity before

	public int num_rupees = 0;
	public float num_hearts;
	public int heart_capacity = 3;
	public int num_keys;
	public int num_bombs;
	public GameObject thing; //had to rename to thing because naming/static error

	//state machine stuff
	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

	public Sprite[] link_down_damage;
	public Sprite[] link_up_damage;
	public Sprite[] link_right_damage;
	public Sprite[] link_left_damage;

	public Sprite[] link_attack; //first element down, then left, then up, then right
	//public Sprite[] swords; //first element down, then left, then up, then right

	public Sprite[] link_dead;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;

	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
	public WeaponDefinition[] weaponDefinitions;
	public Weapon current_weapon;

	public static PlayerController instance;

	public CameraPan cam_pan;
	public bool done_dying = false;
	public float gameRestartDelay = 2f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        instance = this;

		W_DEFS = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions) {
			W_DEFS [def.type] = def;
		}

		materials = Utils.GetAllMaterials (gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++) {
			originalColors [i] = materials [i].color;
		}

		//tile_materials = Utils.GetAllMaterials (GameObject.Find ("Map Anchor"));

		animation_state_machine = new StateMachine ();
		animation_state_machine.ChangeState (new StateIdleWithSprite (this, 
			GetComponent<SpriteRenderer> (), link_run_down[0]));

		GameObject c_w = new GameObject ();
		current_weapon = new Weapon (WeaponType.none, getWeaponDefinition(WeaponType.none), c_w);
    }
    
    // Update is called once per frame
    void Update () {
		//frame++;
		if (current_state == EntityState.ATTACKING)
			current_state = EntityState.NORMAL;

		if (current_weapon.type != WeaponType.none) {
			current_weapon.def.delayBetweenShots--;
			if (current_weapon.def.delayBetweenShots == 0) {
				Destroy (current_weapon.w_go);
				GameObject c_w = new GameObject ();
				current_weapon = new Weapon (WeaponType.none, getWeaponDefinition(WeaponType.none), c_w);
			}
		}
		

        ProcessMovement();
        ProcessAttacks();
		animation_state_machine.Update ();

		if (remainingDamageFlashes > 0) {
			//print ("frame number: " + frame);
			//print (remainingDamageFlashes + " damage flashes left");
			if (remainingDamageFrames > 0) {
				//print (remainingDamageFrames + " damage frames left");
				remainingDamageFrames--;
				if (remainingDamageFrames == showDamageForFrames / 2) {
					//print ("no damage frames left!");
					UnshowDamage ();
				}
			} else {
				//print ("decreasing damage flashes left");
				remainingDamageFlashes--;
				ShowDamage (remainingDamageFlashes);
			}
		} else {
			//print ("no damage flashes left!");
			UnshowDamage ();
		}

		if (num_cooldown_frames > 0) {
			num_cooldown_frames--;
			if (num_cooldown_frames == 0) {
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				//print ("cooldown over!!!!");
			}
		}
		if (num_cooldown_weapon_frames > 0) {
			num_cooldown_weapon_frames--;
		}
		if (num_cooldown_attack_frames > 0) {
			num_cooldown_attack_frames--;
			if (num_cooldown_attack_frames == 0) {
				if (GetComponent<SpriteRenderer> ().sprite == link_attack [0]) {
					animation_state_machine.ChangeState (new StateIdleWithSprite (this, 
						GetComponent<SpriteRenderer> (), link_run_down [0]));
				} else if (GetComponent<SpriteRenderer> ().sprite == link_attack [1]) {
					animation_state_machine.ChangeState (new StateIdleWithSprite (this, 
						GetComponent<SpriteRenderer> (), link_run_left [0]));
				} else if (GetComponent<SpriteRenderer> ().sprite == link_attack [2]) {
					animation_state_machine.ChangeState (new StateIdleWithSprite (this, 
						GetComponent<SpriteRenderer> (), link_run_up [0]));
				} else if (GetComponent<SpriteRenderer> ().sprite == link_attack [3]) {
					animation_state_machine.ChangeState (new StateIdleWithSprite (this, 
						GetComponent<SpriteRenderer> (), link_run_right [0]));
				}
			}
		}
		if (done_dying) {
			DelayedRestart (gameRestartDelay);
		}
    }

    /* TODO: Deal with user-invoked movement of the player character */
    void ProcessMovement ()
    {
		float grid_offset_y = 0.0f;
        Vector3 desired_velocity = Vector3.zero;

		if (num_cooldown_frames == 0) {
			if (Input.GetKey (KeyCode.UpArrow)) {
				desired_velocity = new Vector3 (0, 1, 0);
				float temp;
				if (rb.position.x % 0.5f < 0.25) {
					temp = Mathf.Floor (rb.position.x / 0.5f) * 0.5f;
				} else {
					temp = Mathf.Ceil (rb.position.x / 0.5f) * 0.5f;
				}
				Vector3 newpos = new Vector3 (temp, rb.transform.position.y, 0);
				rb.transform.position = newpos;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				desired_velocity = new Vector3 (-1, 0, 0);
				float temp;
				if ((rb.position.y - grid_offset_y) % 0.5f < 0.25) {
					temp = Mathf.Floor ((rb.position.y - grid_offset_y) / 0.5f) * 0.5f + grid_offset_y;
				} else {
					temp = Mathf.Ceil ((rb.position.y - grid_offset_y) / 0.5f) * 0.5f + grid_offset_y;
				}
				Vector3 newpos = new Vector3 (rb.transform.position.x, temp, 0);
				rb.transform.position = newpos;
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				desired_velocity = Vector3.right;
				float temp;
				if ((rb.position.y - grid_offset_y) % 0.5f < 0.25) {
					temp = Mathf.Floor ((rb.position.y - grid_offset_y) / 0.5f) * 0.5f + grid_offset_y;
				} else {
					temp = Mathf.Ceil ((rb.position.y - grid_offset_y) / 0.5f) * 0.5f + grid_offset_y;
				}
				Vector3 newpos = new Vector3 (rb.transform.position.x, temp, 0);
				rb.transform.position = newpos;
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				desired_velocity = Vector3.down;
				float temp;
				if (rb.position.x % 0.5f < 0.25) {
					temp = Mathf.Floor (rb.position.x / 0.5f) * 0.5f;
				} else {
					temp = Mathf.Ceil (rb.position.x / 0.5f) * 0.5f;
				}
				Vector3 newpos = new Vector3 (temp, rb.transform.position.y, 0);
				rb.transform.position = newpos;
			}

			rb.velocity = desired_velocity * PlayerMovementVelocity;
		}

        /* NOTE:
         * A reminder to study and implement the grid-movement mechanic.
         * Also, consider using Rigidbodies (GetComponent<Rigidbody>().velocity)
         * to attain movement automatic collision-detection.
         * https://docs.unity3d.com/ScriptReference/Rigidbody.html
         * Also also, remember to attain framerate-independence via Time.deltaTime
         * https://docs.unity3d.com/ScriptReference/Time-deltaTime.html 
         */
    }

	static public WeaponDefinition getWeaponDefinition(WeaponType wt) {
		if (W_DEFS.ContainsKey (wt))
			return W_DEFS [wt];

		return (new WeaponDefinition ());
	}

    /* TODO: Deal with user-invoked usage of weapons and items */
    void ProcessAttacks() {
		if (num_cooldown_weapon_frames == 0 && Input.GetKeyDown (KeyCode.Z)) {
			num_cooldown_weapon_frames = 24;
			num_cooldown_attack_frames = 12;
			current_state = EntityState.ATTACKING;
			if (current_direction == Direction.SOUTH)
				//GetComponent<SpriteRenderer> ().sprite = link_attack [0];
				animation_state_machine.ChangeState(new StateIdleWithSprite(this, 
					GetComponent<SpriteRenderer>(), link_attack[0]));
			else if (current_direction == Direction.WEST)
				//GetComponent<SpriteRenderer> ().sprite = link_attack [1];
				animation_state_machine.ChangeState(new StateIdleWithSprite(this, 
					GetComponent<SpriteRenderer>(), link_attack[1]));
			else if (current_direction == Direction.NORTH)
				//GetComponent<SpriteRenderer> ().sprite = link_attack [2];
				animation_state_machine.ChangeState(new StateIdleWithSprite(this, 
					GetComponent<SpriteRenderer>(), link_attack[2]));
			else //direction is EAST
				//GetComponent<SpriteRenderer> ().sprite = link_attack [3];
				animation_state_machine.ChangeState(new StateIdleWithSprite(this, 
					GetComponent<SpriteRenderer>(), link_attack[3]));

			if (selected_weapon_prefab.name == "Sword") {
				Weapon sword = GenerateWeapon (WeaponType.sword);
				UseSword (sword);
			}
		}
	}

	Weapon GenerateWeapon(WeaponType wt) {
		GameObject go = Instantiate (selected_weapon_prefab) as GameObject;
		WeaponDefinition def = getWeaponDefinition(wt);
		go.tag = "Sword";
		Vector3 LinkPos = rb.transform.position;

		switch(wt) {
		case WeaponType.sword:
			if (this.current_direction == Direction.SOUTH) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [0];
				LinkPos.x += 0.1f;
				LinkPos.y -= 0.7f;
				go.transform.position = LinkPos;
				Vector3 go_bc_center = go.GetComponent<BoxCollider> ().center;
				Vector3 go_bc_size = go.GetComponent<BoxCollider> ().size;
				go_bc_center.y -= 0.15f;
				go_bc_size.y = 0.7f;
				go_bc_size.x = 0.7f;
				go.GetComponent<BoxCollider> ().center = go_bc_center;
				go.GetComponent<BoxCollider> ().size = go_bc_size;
			} else if (this.current_direction == Direction.WEST) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [1];
				LinkPos.x -= 0.7f;
				LinkPos.y -= 0.06f;
				go.transform.position = LinkPos;
				Vector3 go_bc_center = go.GetComponent<BoxCollider> ().center;
				Vector3 go_bc_size = go.GetComponent<BoxCollider> ().size;
				go_bc_center.x += 0.15f;
				go_bc_size.x = 0.7f;
				go_bc_size.y = 0.7f;
				go.GetComponent<BoxCollider> ().center = go_bc_center;
				go.GetComponent<BoxCollider> ().size = go_bc_size;
			} else if (this.current_direction == Direction.NORTH) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [2];
				LinkPos.x -= 0.1f;
				LinkPos.y += 0.7f;
				go.transform.position = LinkPos;
				Vector3 go_bc_center = go.GetComponent<BoxCollider> ().center;
				Vector3 go_bc_size = go.GetComponent<BoxCollider> ().size;
				go_bc_center.y += 0.15f;
				go_bc_size.y = 0.7f;
				go_bc_size.x = 0.7f;
				go.GetComponent<BoxCollider> ().center = go_bc_center;
				go.GetComponent<BoxCollider> ().size = go_bc_size;
			} else { //current direction is EAST
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [3];
				LinkPos.x += 0.7f;
				LinkPos.y -= 0.06f;
				go.transform.position = LinkPos;
				Vector3 go_bc_center = go.GetComponent<BoxCollider> ().center;
				Vector3 go_bc_size = go.GetComponent<BoxCollider> ().size;
				go_bc_center.x -= 0.15f;
				go_bc_size.y = 0.7f;
				go_bc_size.x = 0.7f;
				go.GetComponent<BoxCollider> ().center = go_bc_center;
				go.GetComponent<BoxCollider> ().size = go_bc_size;
			}
			break;
		//more cases to come
		}
		Weapon w = new Weapon (wt, def, go);
		return w;
	}

	void UseSword(Weapon sword) {
		if (num_hearts >= 3) { //I'm not sure when he's allowed to shoot--it's either at 3 or "full capacity"
			//it shoots
			switch (this.current_direction) {
			case Direction.SOUTH:
				sword.w_go.GetComponent<Rigidbody> ().velocity = Vector3.down * sword.def.velocity;
				//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
				break;
			case Direction.WEST:
				sword.w_go.GetComponent<Rigidbody> ().velocity = Vector3.left * sword.def.velocity;
				//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
				break;
			case Direction.NORTH:
				sword.w_go.GetComponent<Rigidbody> ().velocity = Vector3.up * sword.def.velocity;
				//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
				break;
			case Direction.EAST:
				sword.w_go.GetComponent<Rigidbody> ().velocity = Vector3.right * sword.def.velocity;
				//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
				break;
			}
		} else {
			Destroy(this.current_weapon.w_go);
			this.current_weapon = sword;
			sword.def.delayBetweenShots = 5;
		}
	}

//	void OnCollisionEnter(Collision coll) {
//		//???
//	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Rupee") {
			num_rupees++;
			//print ("num rupees:" + num_rupees);
			Destroy (collider.gameObject);
			//print ("collected rupee");
		} else if (collider.gameObject.tag == "Heart") {
			if (num_hearts <= heart_capacity - 1) {
				num_hearts++;
			} else {
				num_hearts = heart_capacity;
			}
			//print ("num hearts:" + num_hearts);
			Destroy (collider.gameObject);
			//print ("collected heart");
		} else if (collider.gameObject.tag == "BigHeart") {
			heart_capacity++;
			num_hearts = heart_capacity;
			Destroy (collider.gameObject);
		} else if (collider.gameObject.tag == "Key") {
			num_keys++;
			Destroy (collider.gameObject);
		} else if (collider.gameObject.tag == "Bomb") {
			num_bombs++;
			Destroy (collider.gameObject);
		} else if (collider.gameObject.tag == "Enemy" && num_cooldown_frames == 0) {
			print ("dude you touched me");
			if (num_hearts > 0) {
				ShowDamage (5);
				num_hearts -= 0.5f;
				thing.GetComponent<Hud> ().TookDamage ();
				num_cooldown_frames = 50;
				GetComponent<Rigidbody> ().velocity *= (-1f * damage_hopback_vel);
				if (num_hearts == 0.0) {
					print ("ah dude I ded");
					//DestroyStuffOnDeath ();
					GetComponent<Rigidbody> ().velocity = Vector3.zero;
					animation_state_machine.ChangeState (new StatePlayAnimationForDead (this, 
						GetComponent<SpriteRenderer> (), link_dead, 6));
					//foreach (Material m in tile_materials) {
					//m.color = Color.red;
				//}
				}
			}
		}
	}

	void ShowDamage(int flashes_left) {
		//print ("entered ShowDamage");
		//receive_damage = false;
		remainingDamageFlashes = flashes_left;
		foreach (Material m in materials) {
			m.color = Color.red;
		}
		remainingDamageFrames = showDamageForFrames;
	}

	void UnshowDamage() {
		for (int i = 0; i < materials.Length; i++) {
			materials [i].color = originalColors [i];
		}
	}

	void DelayedRestart(float delay) {
		Invoke("Restart", delay);
	}

	void Restart() {
		SceneManager.LoadScene ("Dungeon");
	}

	void DestroyStuffOnDeath() {
		while (GameObject.FindWithTag ("Enemy") != null) {
			Destroy (GameObject.FindWithTag ("Enemy"));
		}
		while (GameObject.FindWithTag ("Rupee") != null) {
			Destroy (GameObject.FindWithTag ("Rupee"));
		}
		while (GameObject.FindWithTag ("Key") != null) {
			Destroy (GameObject.FindWithTag ("Key"));
		}
		while (GameObject.FindWithTag ("Heart") != null) {
			Destroy (GameObject.FindWithTag ("Heart"));
		}
		while (GameObject.FindWithTag ("BigHeart") != null) {
			Destroy (GameObject.FindWithTag ("BigHeart"));
		}
		while (GameObject.FindWithTag ("Bomb") != null) {
			Destroy (GameObject.FindWithTag ("Bomb"));
		}
		while (GameObject.FindWithTag ("Sword") != null) {
			Destroy (GameObject.FindWithTag ("Sword"));
		}
		while (GameObject.FindWithTag ("Map") != null) {
			Destroy (GameObject.FindWithTag ("Map"));
		}
		while (GameObject.FindWithTag ("Compass") != null) {
			Destroy (GameObject.FindWithTag ("Compass"));
		}
	}
}
