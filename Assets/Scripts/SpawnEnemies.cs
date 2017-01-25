using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

	public float WMspawnDelay;
	private float WMspawntimer;
	public GameObject WM;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update ()
	{
		//--------------------WallMasters-------------------//
		if (Time.time >= WMspawntimer) {
			float playerx = PlayerController.instance.transform.position.x;
			float playery = PlayerController.instance.transform.position.y;
			float playerxFloor = Mathf.Floor (playerx);
			float playeryFloor = Mathf.Floor (playery);
			Vector3 spawn = Vector3.zero;

			if (CameraPan.c.GetComponent<RoomController> ().active_col_index == 4 && CameraPan.c.GetComponent<RoomController> ().active_row_index == 2) {
				if (playerx >= 65.5f && playerx <= 66.5f) { //player is on left side
					if (playeryFloor >= 37f) { //above bottom 2 squares
						spawn = new Vector3 (Mathf.Floor (playerx) - 1f, Mathf.Floor (playery) - 3f, 0);
					} else {
						spawn = new Vector3 (Mathf.Floor (playerx) - 1f, Mathf.Floor (playery) + 3f, 0);
					}
				}

				GameObject go = Instantiate (WM);
				go.transform.position = spawn;
				WMspawntimer = Time.time + WMspawnDelay;
			}
		}


		//--------------------WallMasters-------------------//
	}
}
