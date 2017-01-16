using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerController : MonoBehaviour {

	//public int frame = 0; //for debugging

    /* Inspector Tunables */
    public float PlayerMovementVelocity;
	public int showDamageForFrames = 2;

    Rigidbody rb;
	//public bool receive_damage = false;
	public Material[] materials;
	public int remainingDamageFrames = 0;
	public int remainingDamageFlashes = 0;
	public Color[] originalColors;

	public int num_cooldown_frames = 0;
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

	StateMachine animation_state_machine;
	StateMachine control_state_machine;

	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
	public WeaponDefinition[] weaponDefinitions;
	//public Weapon current_weapon;

	public static PlayerController instance;

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

		animation_state_machine = new StateMachine ();
		animation_state_machine.ChangeState (new StateIdleWithSprite (this, 
			GetComponent<SpriteRenderer> (), link_run_down[0]));
    }
    
    // Update is called once per frame
    void Update () {
		//frame++;
		if (current_state == EntityState.ATTACKING)
			current_state = EntityState.NORMAL;
		

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
		if (Input.GetKeyDown (KeyCode.Z)) {
			current_state = EntityState.ATTACKING;
			if (current_direction == Direction.SOUTH)
				GetComponent<SpriteRenderer> ().sprite = link_attack [0];
			else if (current_direction == Direction.WEST)
				GetComponent<SpriteRenderer> ().sprite = link_attack [1];
			else if (current_direction == Direction.NORTH)
				GetComponent<SpriteRenderer> ().sprite = link_attack [2];
			else //direction is EAST
				GetComponent<SpriteRenderer> ().sprite = link_attack [3];

			if (selected_weapon_prefab.name == "Sword") {
				GameObject sword = GenerateWeapon (WeaponType.sword);
				UseSword (sword, getWeaponDefinition(WeaponType.sword));
			}
		}
	}

	GameObject GenerateWeapon(WeaponType wt) {
		GameObject go = Instantiate (selected_weapon_prefab) as GameObject;
		WeaponDefinition def = getWeaponDefinition(wt);
		go.tag = "Sword";
		Vector3 LinkPos = rb.transform.position;
//		Weapon w = new Weapon();
//		w.SetType(wt);
//		w.def = def;

		switch(wt) {
		case WeaponType.sword:
			if (this.current_direction == Direction.SOUTH) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [0];
				LinkPos.x += 0.1f;
				LinkPos.y -= 0.7f;
				go.transform.position = LinkPos;
			} else if (this.current_direction == Direction.WEST) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [1];
				LinkPos.x += 0.7f;
				LinkPos.y -= 0.06f;
				go.transform.position = LinkPos;
			} else if (this.current_direction == Direction.NORTH) {
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [2];
				LinkPos.x -= 0.1f;
				LinkPos.y += 0.7f;
				go.transform.position = LinkPos;
			} else { //current direction is EAST
				go.GetComponent<SpriteRenderer> ().sprite = def.sprites_dlur [3];
				LinkPos.x -= 0.7f;
				LinkPos.y -= 0.06f;
				go.transform.position = LinkPos;
			}
			break;
		//more cases to come
		}

		return go;
	}

	void UseSword(GameObject sword, WeaponDefinition df) {
		if (num_hearts >= 3) { //I'm not sure when he's allowed to shoot--it's either at 3 or "full capacity"
			//it shoots
			switch (this.current_direction) {
			case Direction.SOUTH:
				sword.GetComponent<Rigidbody> ().velocity = Vector3.down * df.velocity;
				break;
			case Direction.WEST:
				sword.GetComponent<Rigidbody> ().velocity = Vector3.left * df.velocity;
				break;
			case Direction.NORTH:
				sword.GetComponent<Rigidbody> ().velocity = Vector3.up * df.velocity;
				break;
			case Direction.EAST:
				sword.GetComponent<Rigidbody> ().velocity = Vector3.right * df.velocity;
				break;
			}
		} else {
			//it just appears for a bit an then doesn't shoot
		}
	}

	void OnCollisionEnter(Collision coll) {
		//print ("entered collision enter function");
		if (coll.gameObject.tag == "Enemy" && num_cooldown_frames == 0) {
			//receive_damage = true;
			//do something here like decrease health? not sure how to put all
			//the code together with these files, like difference between
			//PlayerControl and PlayerController.
			print ("dude you touched me");
//			Sprite current = animation_state_machine.
			//animation_state_machine.ChangeState(new StatePlayAnimationForDamage(this, GetComponent<SpriteRenderer>(),

			ShowDamage(5);
			//num_hearts -= 0.5f;

			num_cooldown_frames = 50;
			GetComponent<Rigidbody> ().velocity *= (-1f * damage_hopback_vel);

			if (num_hearts <= 0.0) {
				print ("ah dude I ded");
				//something idk...gotta start the dying animation I guess?
			} 
			else { //made it so can only take damage down to 0, don't want negative health
				ShowDamage (5);
				num_hearts -= 0.5f;
				thing.GetComponent<Hud> ().TookDamage ();
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Rupee") {
			num_rupees++;
			print ("num rupees:" + num_rupees);
			Destroy (collider.gameObject);
			print ("collected rupee");
		} else if (collider.gameObject.tag == "Heart") {
			if (num_hearts <= heart_capacity - 1) {
				num_hearts++;
			} else {
				num_hearts = heart_capacity;
			}
			print ("num hearts:" + num_hearts);
			Destroy (collider.gameObject);
			print ("collected heart");
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

}
