using System.Collections;
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
	public bool hasCompass = false;
	public bool hasMap = false;

	public bool has_map;
	public bool has_compass;
	public bool has_bow;

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

	public GameObject selected_weapon_prefab_a;
	public GameObject selected_weapon_prefab_b;
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
	public WeaponDefinition[] weaponDefinitions;
	public Weapon current_weapon_A;
	public Weapon current_weapon_B;
	public bool have_boomerang = false;

	public static PlayerController instance;

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
		current_weapon_A = new Weapon (WeaponType.none, getWeaponDefinition(WeaponType.none), c_w, this);
		current_weapon_B = new Weapon (WeaponType.none, getWeaponDefinition(WeaponType.none), c_w, this);

		if (selected_weapon_prefab_b.name == "Boomerang") {
			have_boomerang = true;
		}
    }
    
    // Update is called once per frame
    void Update () {
		//frame++;
		if (current_state == EntityState.ATTACKING)
			current_state = EntityState.NORMAL;

		if (current_weapon_A.type != WeaponType.none) {
			current_weapon_A.def.delayBetweenShots--;
			if (current_weapon_A.def.delayBetweenShots == 0) {
				Destroy (current_weapon_A.w_go);
				GameObject c_w = new GameObject ();
				current_weapon_A = new Weapon (WeaponType.none, getWeaponDefinition(WeaponType.none), c_w, this);
			}
		}

		if (current_weapon_B.type == WeaponType.arrow) {
			current_weapon_B.def.delayBetweenShots--;
			if (current_weapon_B.def.delayBetweenShots == 0) {
				Destroy (current_weapon_B.w_go);
				GameObject c_w = new GameObject ();
				current_weapon_B = new Weapon (WeaponType.none, getWeaponDefinition (WeaponType.none), c_w, this);
			}
		}

		if (current_weapon_B.type == WeaponType.bomb) {
			current_weapon_B.def.delayBetweenShots--;
			if (current_weapon_B.def.delayBetweenShots == 0) {
				print ("BOMB GOES BOOM");
				//BombKills ();
				Destroy (current_weapon_B.w_go);
				GameObject c_w = new GameObject ();
				current_weapon_B = new Weapon (WeaponType.none, getWeaponDefinition (WeaponType.none), c_w, this);
			}
		}

//		print ("boomerang position " + current_weapon_B.w_go.transform.position);
//		print ("boomerang target " + current_weapon_B.target1);
//		print ("on way back " + current_weapon_B.on_way_back);
		if (current_weapon_B.type == WeaponType.boomerang && !current_weapon_B.on_way_back 
			&& (Mathf.Abs (current_weapon_B.w_go.transform.position.x - current_weapon_B.target1.x) <= 0.1
			&& Mathf.Abs (current_weapon_B.w_go.transform.position.y - current_weapon_B.target1.y) <= 0.1)) {
			//print ("ON MAH WAY BACK DAWG");
			current_weapon_B.on_way_back = true;
			Vector3 new_direction = this.transform.position - current_weapon_B.w_go.transform.position;
			current_weapon_B.w_go.GetComponent<Rigidbody> ().velocity = new_direction.normalized * current_weapon_B.def.velocity;
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

		if (num_cooldown_frames == 0 
			&& (!CameraPan.c.panning_down && !CameraPan.c.panning_up 
				&& !CameraPan.c.panning_left && !CameraPan.c.panning_right)) {
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
		if (num_cooldown_weapon_frames == 0 && (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown(KeyCode.S))) {
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

			if (Input.GetKeyDown (KeyCode.A)) {
				Weapon sword = GenerateWeapon (WeaponType.sword, 'a');
				UseSword (sword);
			} else if (Input.GetKeyDown (KeyCode.S)) {
				if (selected_weapon_prefab_b.name == "Boomerang" && have_boomerang) {
					Weapon boomerang = GenerateWeapon (WeaponType.boomerang, 'b');
					UseBoomerang (boomerang);
				} else if (selected_weapon_prefab_b.name == "Arrow" && has_bow) {
					Weapon arrow = GenerateWeapon (WeaponType.arrow, 'b');
					UseArrow (arrow);
					num_rupees--;
				} else if (selected_weapon_prefab_b.name == "Bomb" && num_bombs > 0) {
					Weapon bomb = GenerateWeapon (WeaponType.bomb, 'b');
					DropBomb (bomb);
					num_bombs--;
				}
			}
		}
	}

	Weapon GenerateWeapon(WeaponType wt, char ab) {
		GameObject go;
		if (ab == 'a') {
			go = Instantiate (selected_weapon_prefab_a) as GameObject;
		} else if (ab == 'b') {
			go = Instantiate (selected_weapon_prefab_b) as GameObject;
		}
		WeaponDefinition def = getWeaponDefinition(wt);
		Vector3 LinkPos = rb.transform.position;

		switch(wt) {
		case WeaponType.sword:
			go.tag = "Sword";
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
		case WeaponType.boomerang:
			go.tag = "Boomerang";
			//go.transform.position = this.transform.position;
			if (this.current_direction == Direction.SOUTH) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [0];
				LinkPos.y -= 1f;
				go.transform.position = LinkPos;
			} else if (this.current_direction == Direction.WEST) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [1];
				LinkPos.x -= 1f;
				go.transform.position = LinkPos;
			} else if (this.current_direction == Direction.NORTH) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [2];
				LinkPos.y += 1f;
				go.transform.position = LinkPos;
			} else {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [3];
				LinkPos.x += 1f;
				go.transform.position = LinkPos;
			}
			break;
		case WeaponType.arrow:
			go.tag = "Sword"; //IDK maybe it's okay
			if (this.current_direction == Direction.SOUTH) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [0];
				LinkPos.y -= 1f;
				go.transform.position = LinkPos;
			} else if (this.current_direction == Direction.WEST) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [1];
				LinkPos.x -= 1f;
				go.transform.position = LinkPos;
			} else if (this.current_direction == Direction.NORTH) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [2];
				LinkPos.y += 1f;
				go.transform.position = LinkPos;
			} else {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [3];
				LinkPos.x += 1f;
				go.transform.position = LinkPos;
			}
			break;
		case WeaponType.bomb:
			print ("gonna generate a bomb!");
			go.tag = "Bomb";
			go.GetComponent<BoxCollider> ().isTrigger = false;
			go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [0];
			go.transform.position = LinkPos;
			break;
		}
		Weapon w = new Weapon (wt, def, go, this);
		return w;
	}

	void DropBomb(Weapon bomb) {
		print ("dropped bomb");
		Destroy (this.current_weapon_B.w_go);
		this.current_weapon_B = bomb;
		current_weapon_B.def.delayBetweenShots = 120;
	}

	void BombKills() {
		Vector3 pos = current_weapon_B.w_go.transform.position;

		//kills stuff in the Manhattan distance of 1
		//uses Room to do this by searching through all the objects in the room
	}


	void UseBoomerang(Weapon boomerang) {
		Destroy (this.current_weapon_B.w_go);
		this.current_weapon_B = boomerang;
		have_boomerang = false;
		this.current_weapon_B.on_way_back = false;
		switch (this.current_direction) {
		case Direction.SOUTH:
			current_weapon_B.w_go.GetComponent<Rigidbody> ().velocity = Vector3.down * current_weapon_B.def.velocity;
			current_weapon_B.target1 = this.transform.position;
			current_weapon_B.target1.y -= 5f;
			//print ("sword velocity: " + boomerang.w_go.GetComponent<Rigidbody> ().velocity);
			print ("boomerang start " + this.transform.position);
			print ("boomerang target " + current_weapon_B.target1);
			break;
		case Direction.WEST:
			current_weapon_B.w_go.GetComponent<Rigidbody> ().velocity = Vector3.left * current_weapon_B.def.velocity;
			current_weapon_B.target1 = this.transform.position;
			current_weapon_B.target1.x -= 5f;
			//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
			break;
		case Direction.NORTH:
			current_weapon_B.w_go.GetComponent<Rigidbody> ().velocity = Vector3.up * current_weapon_B.def.velocity;
			current_weapon_B.target1 = this.transform.position;
			current_weapon_B.target1.y += 5f;
			//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
			break;
		case Direction.EAST:
			current_weapon_B.w_go.GetComponent<Rigidbody> ().velocity = Vector3.right * current_weapon_B.def.velocity;
			current_weapon_B.target1 = this.transform.position;
			current_weapon_B.target1.x += 5f;
			//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
			break;
		}
	}

	void UseArrow(Weapon arrow) {
		if (num_rupees > 0) {
			switch (this.current_direction) {
			case Direction.SOUTH:
				arrow.w_go.GetComponent<Rigidbody> ().velocity = Vector3.down * arrow.def.velocity;
				//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
				break;
			case Direction.WEST:
				arrow.w_go.GetComponent<Rigidbody> ().velocity = Vector3.left * arrow.def.velocity;
				//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
				break;
			case Direction.NORTH:
				arrow.w_go.GetComponent<Rigidbody> ().velocity = Vector3.up * arrow.def.velocity;
				//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
				break;
			case Direction.EAST:
				arrow.w_go.GetComponent<Rigidbody> ().velocity = Vector3.right * arrow.def.velocity;
				//print ("sword velocity: " + sword.w_go.GetComponent<Rigidbody> ().velocity);
				break;
			}
		}
	}

	void UseSword(Weapon sword) {
		if (num_hearts == heart_capacity) { //I'm not sure when he's allowed to shoot--it's either at 3 or "full capacity"
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
			Destroy(this.current_weapon_A.w_go);
			this.current_weapon_A = sword;
			sword.def.delayBetweenShots = 5;
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Enemy" && num_cooldown_frames == 0) {
			//print ("dude you touched me");
			if (num_hearts > 0) {
				ShowDamage (5);
				num_hearts -= 0.5f;
				thing.GetComponent<Hud> ().TookDamage ();
				num_cooldown_frames = 50;
				GetComponent<Rigidbody> ().velocity *= (-1f * damage_hopback_vel);
				if (num_hearts == 0.0) {
					//print ("ah dude I ded");
					//DestroyStuffOnDeath ();
					GetComponent<Rigidbody> ().velocity = Vector3.zero;
					animation_state_machine.ChangeState (new StatePlayAnimationForDead (this, 
						GetComponent<SpriteRenderer> (), link_dead, 6));
					//foreach (Material m in tile_materials) {
					//m.color = Color.red;
					//}
				}
			}
		} else if (coll.gameObject.tag == "Boomerang") {
			Destroy (coll.gameObject);
			GameObject c_w = new GameObject ();
			current_weapon_B = new Weapon (WeaponType.none, getWeaponDefinition (WeaponType.none), c_w, this);
			have_boomerang = true;
		}
	}

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
			num_hearts += 1;
			Destroy (collider.gameObject);
		} else if (collider.gameObject.tag == "Key") {
			num_keys++;
			Destroy (collider.gameObject);
		} else if (collider.gameObject.tag == "Bomb") {
			num_bombs++;
			Destroy (collider.gameObject);
		} else if (collider.gameObject.tag == "Bow") {
			has_bow = true;
			//change animation to hold up bow
			Destroy (collider.gameObject);
		} else if (collider.gameObject.tag == "Map") {
			has_map = true;
			Destroy (collider.gameObject);
			//put map on hud
		} else if (collider.gameObject.tag == "Compass") {
			has_compass = true;
			Destroy (collider.gameObject);
			//put little red dot on hud
		} else if (collider.gameObject.tag == "Enemy" && num_cooldown_frames == 0) {
			//print ("dude you touched me");
			if (num_hearts > 0) {
				ShowDamage (5);
				num_hearts -= 0.5f;
				thing.GetComponent<Hud> ().TookDamage ();
				num_cooldown_frames = 50;
				GetComponent<Rigidbody> ().velocity *= (-1f * damage_hopback_vel);
				if (num_hearts == 0.0) {
					//print ("ah dude I ded");
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

	void OnTriggerExit (Collider col)
	{
		//print("hello?");
		//print (col.gameObject.name);
		if (col.gameObject.name == "WallMaster") {
		//print("In");
			GetComponent<Collider>().isTrigger = false; //allow Player to collide properly again
			//black screen wipe
			//move to first room
			transform.position = new Vector3(40f, 2f, 0);
			//set direction to north/up
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
}
