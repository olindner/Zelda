using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : MonoBehaviour {

	private char dir = 'N';
	public float spriteDelay;
	private float spriteTimer;
	public Sprite[] array;
	private int here;
	public int distance = 2;
	private float changeTimer;
	private float changeDelay = 1f; //has to wait 1 second to change distance again

	// Use this for initialization
	void Start () {
		//gameObject.SetActive(false); //start off disabled
		spriteTimer = Time.time + spriteDelay;
		here = 0;
	}

	// Update is called once per frame
	void Update () {

		if (Time.time >= spriteTimer) {
			GetComponent<SpriteRenderer> ().sprite = array [here];
			if (here == 0)
				here = 1;
			else if (here == 1)
				here = 0;
			spriteTimer = Time.time + spriteDelay;
		}

		Vector3 rayUp = transform.TransformDirection (Vector3.up);
		Vector3 rayDown = transform.TransformDirection (Vector3.down);
		Vector3 rayLeft = transform.TransformDirection (Vector3.left);
		Vector3 rayRight = transform.TransformDirection (Vector3.right);
		//RaycastHit hit;

		//Lengthen distance
		if ((Input.GetKeyDown (KeyCode.UpArrow) && Physics.Raycast(transform.position, rayUp, 1) && dir == 'S') ||
			(Input.GetKeyDown (KeyCode.DownArrow) && Physics.Raycast(transform.position, rayDown, 1) && dir == 'N') ||
			(Input.GetKeyDown (KeyCode.LeftArrow) && Physics.Raycast(transform.position, rayLeft, 1) && dir == 'E') ||
			(Input.GetKeyDown (KeyCode.RightArrow) && Physics.Raycast(transform.position, rayRight, 1) && dir == 'W')) distance += 1;
		//Shorten distance
		if ((Input.GetKeyDown (KeyCode.UpArrow) && Physics.Raycast(transform.position, rayUp, 1) && dir == 'N') ||
			(Input.GetKeyDown (KeyCode.DownArrow) && Physics.Raycast(transform.position, rayDown, 1) && dir == 'S') ||
			(Input.GetKeyDown (KeyCode.LeftArrow) && Physics.Raycast(transform.position, rayLeft, 1) && dir == 'W') ||
			(Input.GetKeyDown (KeyCode.RightArrow) && Physics.Raycast(transform.position, rayRight, 1) && dir == 'E') && distance >= 2) distance -= 1;

		if (dir == 'N') transform.position = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y + distance, 0);
		else if (dir == 'E') transform.position = new Vector3(PlayerController.instance.transform.position.x + distance, PlayerController.instance.transform.position.y, 0);
		else if (dir == 'S') transform.position = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y - distance, 0);
		else if (dir == 'W') transform.position = new Vector3(PlayerController.instance.transform.position.x - distance, PlayerController.instance.transform.position.y, 0);

		if (Input.GetKeyDown(KeyCode.X)) { //set as x for now to not create issues
			if (dir == 'N') dir = 'E';
			else if (dir == 'E') dir = 'S';
			else if (dir == 'S') dir = 'W';
			else if (dir == 'W') dir = 'N';
		}

	}
}
