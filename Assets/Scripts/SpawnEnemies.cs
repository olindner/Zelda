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

			if (playerx % 16f >= 1.5f && playerx % 16f <= 2.5f) { //player is on left side
				if (playeryFloor % 11f > 3f) { //above bottom 2 squares
					spawn = new Vector3 (Mathf.Floor (playerx) - 1f, Mathf.Floor (playery) - 3f, 0);
				} else {
					spawn = new Vector3 (Mathf.Floor (playerx) - 1f, Mathf.Floor (playery) + 3f, 0);
				}
			
				GameObject go = Instantiate (WM);
				go.transform.position = spawn;
				WMspawntimer = Time.time + WMspawnDelay;
			}
		}


		//--------------------WallMasters-------------------//
	}
}
