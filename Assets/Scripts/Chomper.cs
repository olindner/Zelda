using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : MonoBehaviour {

	private char dir = 'N';

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false); //start off disabled
	}

	// Update is called once per frame
	void Update () {
		if (dir == 'N') transform.position = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y + 2f, 0);
		else if (dir == 'E') transform.position = new Vector3(PlayerController.instance.transform.position.x + 2f, PlayerController.instance.transform.position.y, 0);
		else if (dir == 'S') transform.position = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y - 2f, 0);
		else if (dir == 'W') transform.position = new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y, 0);

		if (Input.GetKeyDown(KeyCode.X)) { //set as x for now to not create issues
			if (dir == 'N') dir = 'E';
			else if (dir == 'E') dir = 'S';
			else if (dir == 'S') dir = 'W';
			else if (dir == 'W') dir = 'N';
		}
	}
}
