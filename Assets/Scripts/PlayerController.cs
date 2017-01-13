using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    /* Inspector Tunables */
    public float PlayerMovementVelocity;
	public int showDamageForFrames = 2;

    /* Private Data */
    Rigidbody rb;
	public bool receive_damage = false;
	public Material[] materials;
	public int remainingDamageFrames = 0;
	public int remainingDamageFlashes = 0;
	public Color[] originalColors;

	public int num_rupees = 0;
	public float num_hearts;
	public int heart_capacity = 3;
	public int num_keys;
	public int num_bombs;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

//		materials = Utils.GetAllMaterials (gameObject);
//		originalColors = new Color[materials.Length];
//		for (int i = 0; i < materials.Length; i++) {
//			originalColors [i] = materials [i].color;
//		}
    }
    
    // Update is called once per frame
    void Update () {
        ProcessMovement();
        ProcessAttacks();
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

	void OnCollisionEnter(Collision coll) {
		//future enemy damage stuff
		if (coll.gameObject.tag == "Enemy") {
			//receive_damage = true;
			//do something here like decrease health? not sure how to put all
			//the code together with these files, like difference between
			//PlayerControl and PlayerController.
			print("dude you touched me");
			num_hearts -= 0.5f;
			if (num_hearts <= 0.0) {
				print ("ah dude I ded");
				//something idk...gotta start the dying animation I guess?
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
}
