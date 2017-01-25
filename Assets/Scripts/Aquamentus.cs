using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquamentus : MonoBehaviour {

	public Sprite[] walkSprites;
	public float spriteDelay;
	private float spriteTimer;
	public Sprite openMouth;
	public int health;
	private bool isMoving;
	private Vector3 pos;
	private int dir;
	public float moveDistance;
	public float speed;
	public GameObject fireball;
	public float shootDelay;
	private float shootTimer;
	public float mouthDelay;
	private float mouthTimer;
	private int here = 0;

	public int showDamageForFrames = 2;
	public Material[] materials;
	public Material[] tile_materials;
	public int remainingDamageFrames = 0;
	public int remainingDamageFlashes = 0;
	public Color[] originalColors;
	public int num_cooldown_frames = 0;
	public float damage_hopback_vel = 2.65f;

	public GameObject bigheart;

	public Room room;
	// Use this for initialization
	void Start () {
		health = 10; //? Not sure
		isMoving = false;
		pos = transform.position;
		dir = Random.Range(0,1); //random starting direction, 0=RIGHT, 1=LEFT
		shootTimer = Time.time + shootDelay;
		spriteTimer = Time.time + spriteDelay;

		materials = Utils.GetAllMaterials (gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++) {
			originalColors [i] = materials [i].color;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Time.time >= spriteTimer) {
			GetComponent<SpriteRenderer>().sprite = walkSprites[here];
			if (here == 0) here = 1;
			else if (here == 1) here = 0;
			spriteTimer = Time.time + spriteDelay;
		}

		if (transform.position == pos) {
			isMoving = false;
		} else
			isMoving = true;

		if (!isMoving) {
			int num = Random.Range (0, 5);
			Vector3 rayRight = transform.TransformDirection (Vector3.right);

			if (num == 0 && !Physics.Raycast (transform.position, rayRight, moveDistance)) { //RIGHT
				pos += Vector3.right * moveDistance;
				dir = 0;

			} else if (num == 1 && transform.position.x >= 74f) { //LEFT
				pos += Vector3.left * moveDistance;
				dir = 1;
			} else { //favors continuous movement
				if (dir == 0 && !Physics.Raycast (transform.position, rayRight, moveDistance)) { //RIGHT
					pos += Vector3.right * moveDistance;
				} else if (dir == 1 && transform.position.x >= 74f) { //LEFT
					pos += Vector3.left * moveDistance;
				}
			}
		}
		transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);

		if (Time.time >= shootTimer) {
			mouthTimer = shootTimer + mouthDelay;
			GetComponent<SpriteRenderer> ().sprite = openMouth;
			if (Time.time >= mouthTimer) {
				GetComponent<SpriteRenderer> ().sprite = walkSprites[here];
				GameObject ball0 = Instantiate (fireball);
				ball0.gameObject.transform.position = transform.position;
				ball0.GetComponent<Fireball> ().angle = 0;
				GameObject ball1 = Instantiate (fireball);
				ball1.gameObject.transform.position = transform.position;
				ball1.GetComponent<Fireball> ().angle = 1;
				GameObject ball2 = Instantiate (fireball);
				ball2.gameObject.transform.position = transform.position;
				ball2.GetComponent<Fireball> ().angle = 2;

				shootTimer = Time.time + shootDelay;
			}
		}

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
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Sword" || coll.gameObject.tag == "Boomerang") {
			health--;
			ShowDamage (5);
			if (health == 0) {
				room.num_enemies_left--;
				room.things_inside_room.Remove (this.gameObject);
				Vector3 current_pos = this.gameObject.transform.position;
				Destroy (this.gameObject);
				GameObject bigHeart = Instantiate (bigheart) as GameObject;
				bigHeart.transform.position = current_pos;
				room.things_inside_room.Add (bigHeart);
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
