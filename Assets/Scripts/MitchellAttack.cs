using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitchellAttack : MonoBehaviour {

	public GameObject fireball;
	public float shootDelay;
	private float shootTimer;
	public Sprite ded;
	private bool isDead = false;

	public Room room;

	// Use this for initialization
	void Start () {
		shootTimer = Time.time + shootDelay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time >= shootTimer && !isDead) {
			GameObject fb1 = Instantiate (fireball);
			GameObject fb2 = Instantiate (fireball);
			fb1.gameObject.transform.position = fb2.gameObject.transform.position = transform.position;
			fb1.GetComponent<MitchellFireball> ().angle = 0;
			fb2.GetComponent<MitchellFireball> ().angle = 1;
			shootTimer = Time.time + shootDelay;
		}

		if (RoomController.rc.active_col_index == 4 && RoomController.rc.active_row_index == 9 && RoomController.rc.map1 [RoomController.rc.active_row_index, RoomController.rc.active_col_index].all_blocks_pushed) {
			GetComponent<SpriteRenderer>().sprite = ded;
			isDead = true;
		}
	}
}
