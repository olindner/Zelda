using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {

	private Transform tr;
	public float speed;
	Vector3 pos;
	private int health;
	private bool isMoving;
	private int dir;
	public float spriteTimeDelay;
	private float timer;

	public float prob_drop_rupee = 0.75f;
	public GameObject rupee;
	public GameObject blue_rupee;

	public int showDamageForFrames = 2;
	public Material[] materials;
	public Material[] tile_materials;
	public int remainingDamageFrames = 0;
	public int remainingDamageFlashes = 0;
	public Color[] originalColors;
	public int num_cooldown_frames = 0;
	public float damage_hopback_vel = 2.65f;

	public Room room;

	// Use this for initialization
	void Start () {
		pos = transform.position;
    	tr = transform;
		health = 2;
		isMoving = false;
		dir = Random.Range(0,3); //pick a random starting direction
		timer = Time.time + spriteTimeDelay;

		materials = Utils.GetAllMaterials (gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++) {
			originalColors [i] = materials [i].color;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time >= timer) {
			GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
			timer = Time.time + spriteTimeDelay;
		}

		if (tr.position == pos) {
			isMoving = false;
		}
		else
			isMoving = true;

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
				this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			}
		}

		if (!isMoving) {
			int num = Random.Range (0, 15);

			Vector3 rayUp = transform.TransformDirection (Vector3.up);
			Vector3 rayDown = transform.TransformDirection (Vector3.down);
			Vector3 rayLeft = transform.TransformDirection (Vector3.left);
			Vector3 rayRight = transform.TransformDirection (Vector3.right);
			RaycastHit hit;

			//Equal percentages each direction...
			if (num == 0 || num == 1) { //Up
				if (!Physics.Raycast (transform.position, rayUp, out hit, 1) || //no collision whatsoever
				    (Physics.Raycast (transform.position, rayUp, out hit, 1) && hit.transform.tag == "Key") || //okay to walk on key
				    (Physics.Raycast (transform.position, rayUp, out hit, 1) && hit.transform.tag == "Bomb") || //okay to walk on bomb
				    (Physics.Raycast (transform.position, rayUp, out hit, 1) && hit.transform.tag == "Rupee")) { //okay to walk on rupee

					pos += Vector3.up;
					dir = 0;
				}
			} else if (num == 2 || num == 3) { //Down
				if (!Physics.Raycast (transform.position, rayDown, out hit, 1) || //no collision whatsoever
				    (Physics.Raycast (transform.position, rayDown, out hit, 1) && hit.transform.tag == "Key") || //okay to walk on key
				    (Physics.Raycast (transform.position, rayDown, out hit, 1) && hit.transform.tag == "Bomb") || //okay to walk on bomb
				    (Physics.Raycast (transform.position, rayDown, out hit, 1) && hit.transform.tag == "Rupee")) { //okay to walk on rupee

					pos += Vector3.down;
					dir = 1;
				}
			} else if (num == 4 || num == 5) { //Left
				if (!Physics.Raycast (transform.position, rayLeft, out hit, 1) || //no collision whatsoever
				    (Physics.Raycast (transform.position, rayLeft, out hit, 1) && hit.transform.tag == "Key") || //okay to walk on key
				    (Physics.Raycast (transform.position, rayLeft, out hit, 1) && hit.transform.tag == "Bomb") || //okay to walk on bomb
				    (Physics.Raycast (transform.position, rayLeft, out hit, 1) && hit.transform.tag == "Rupee")) { //okay to walk on rupee

					pos += Vector3.left;
					dir = 2;
				}
			} else if (num == 6 || num == 7) { //Right
				if (!Physics.Raycast (transform.position, rayRight, out hit, 1) || //no collision whatsoever
				    (Physics.Raycast (transform.position, rayRight, out hit, 1) && hit.transform.tag == "Key") || //okay to walk on key
				    (Physics.Raycast (transform.position, rayRight, out hit, 1) && hit.transform.tag == "Bomb") || //okay to walk on bomb
				    (Physics.Raycast (transform.position, rayRight, out hit, 1) && hit.transform.tag == "Rupee")) { //okay to walk on rupee

					pos += Vector3.right;
					dir = 3;
				}
			} 
			//...added likelihood to continue moving in same direction
			//0=UP, 1=DOWN, 2=LEFT, 3=RIGHT
			else if (num >= 8 && num <= 14) {
				if (dir == 0) {
					if (!Physics.Raycast (transform.position, rayUp, out hit, 1) || //no collision whatsoever
				    	(Physics.Raycast (transform.position, rayUp, out hit, 1) && hit.transform.tag == "Key") || //okay to walk on key
						(Physics.Raycast (transform.position, rayUp, out hit, 1) && hit.transform.tag == "Bomb") || //okay to walk on bomb
						(Physics.Raycast (transform.position, rayUp, out hit, 1) && hit.transform.tag == "Rupee")) { //okay to walk on rupee

						pos += Vector3.up;
					}
				} 
				else if (dir == 1) {
					if (!Physics.Raycast (transform.position, rayDown, out hit, 1) || //no collision whatsoever
						(Physics.Raycast (transform.position, rayDown, out hit, 1) && hit.transform.tag == "Key") || //okay to walk on key
						(Physics.Raycast (transform.position, rayDown, out hit, 1) && hit.transform.tag == "Bomb") || //okay to walk on bomb
						(Physics.Raycast (transform.position, rayDown, out hit, 1) && hit.transform.tag == "Rupee")) { //okay to walk on rupee

						pos += Vector3.down;
					}
				} 
				else if (dir == 2) {
					if (!Physics.Raycast (transform.position, rayLeft, out hit, 1) || //no collision whatsoever
						(Physics.Raycast (transform.position, rayLeft, out hit, 1) && hit.transform.tag == "Key") || //okay to walk on key
						(Physics.Raycast (transform.position, rayLeft, out hit, 1) && hit.transform.tag == "Bomb") || //okay to walk on bomb
						(Physics.Raycast (transform.position, rayLeft, out hit, 1) && hit.transform.tag == "Rupee")) { //okay to walk on rupee

						pos += Vector3.left;
					}
				} 
				else if (dir == 3) {
					if (!Physics.Raycast (transform.position, rayRight, out hit, 1) || //no collision whatsoever
						(Physics.Raycast (transform.position, rayRight, out hit, 1) && hit.transform.tag == "Key") || //okay to walk on key
						(Physics.Raycast (transform.position, rayRight, out hit, 1) && hit.transform.tag == "Bomb") || //okay to walk on bomb
						(Physics.Raycast (transform.position, rayRight, out hit, 1) && hit.transform.tag == "Rupee")) { //okay to walk on rupee

						pos += Vector3.right;
					}

				}
			}
		}
		transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Sword") {
			GetComponent<Rigidbody> ().velocity = damage_hopback_vel * col.rigidbody.velocity;
			num_cooldown_frames = 5;
			Destroy(col.gameObject);
			health--;
			ShowDamage (5);
			if (health <= 0) {
				room.num_enemies_left--;
				room.things_inside_room.Remove (this.gameObject);
				Destroy (this.gameObject);
				if (Random.Range (0f, 1f) < prob_drop_rupee) {
					GameObject go;
					if (Random.Range (0f, 1f) < 0.5) {
						go = Instantiate (rupee) as GameObject;
					} else {
						go = Instantiate (blue_rupee) as GameObject;
					}
					room.things_inside_room.Add (go);
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
}
