using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerController : MonoBehaviour {

    /* Inspector Tunables */
    public float PlayerMovementVelocity;
	public int showDamageForFrames = 2;

    Rigidbody rb;
	//public bool receive_damage = false;
	public Material[] materials;
	public int remainingDamageFrames = 0;
	public int remainingDamageFlashes = 0;
	public Color[] originalColors;

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

	StateMachine animation_state_machine;
	StateMachine control_state_machine;

	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;

	public static PlayerController instance;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        instance = this;

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
        ProcessMovement();
        ProcessAttacks();
		animation_state_machine.Update ();

		if (remainingDamageFlashes > 0) {
			if (remainingDamageFrames > 0) {
				remainingDamageFrames--;
				if (remainingDamageFrames == 0) {
					UnshowDamage ();
				}
			} else {
				remainingDamageFlashes--;
				ShowDamage (remainingDamageFlashes);
			}
		} else {
			UnshowDamage ();
		}
    }

    /* TODO: Deal with user-invoked movement of the player character */
    void ProcessMovement ()
    {
		float grid_offset_y = 0.0f;
        Vector3 desired_velocity = Vector3.zero;

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

        /* NOTE:
         * A reminder to study and implement the grid-movement mechanic.
         * Also, consider using Rigidbodies (GetComponent<Rigidbody>().velocity)
         * to attain movement automatic collision-detection.
         * https://docs.unity3d.com/ScriptReference/Rigidbody.html
         * Also also, remember to attain framerate-independence via Time.deltaTime
         * https://docs.unity3d.com/ScriptReference/Time-deltaTime.html 
         */
    }

    /* TODO: Deal with user-invoked usage of weapons and items */
    void ProcessAttacks()
    {

    }

	void OnCollisionEnter (Collision coll)
	{
		print ("entered collision enter function");
		if (coll.gameObject.tag == "Enemy") {
			//receive_damage = true;
			//do something here like decrease health? not sure how to put all
			//the code together with these files, like difference between
			//PlayerControl and PlayerController.
			print ("dude you touched me");
//			Sprite current = animation_state_machine.
			//animation_state_machine.ChangeState(new StatePlayAnimationForDamage(this, GetComponent<SpriteRenderer>(),
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
