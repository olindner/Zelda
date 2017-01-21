using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiketrap : MonoBehaviour {

	public float speed;
	public float speedReset;
	private Vector3 player;
	private Vector3 origin;
	private Vector3 target;
	private bool isMovingOut;
	private bool isMovingBack;

	// Use this for initialization
	void Start () {
		origin = transform.position;
		target = transform.position;
		isMovingOut = false;
		isMovingBack = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (transform.position == origin) {
			isMovingOut = false;
			isMovingBack = false;
		} else if (transform.position == target) {
			isMovingOut = false;
			isMovingBack = true;
		}

		if (!isMovingOut && !isMovingBack) {
			player = PlayerController.instance.transform.position; //get player position (could also do with a raycast)
			target = transform.position;

			if (player.x >= origin.x - 0.5 && player.x <= origin.x + 0.5 && player.y > origin.y) { //player is above
				target = new Vector3 (origin.x, origin.y + 3, 0);
			} else if (player.x >= origin.x - 0.5 && player.x <= origin.x + 0.5 && player.y < origin.y) { //player is below
				target = new Vector3 (origin.x, origin.y - 3, 0);
			} else if (player.y >= origin.y - 0.5 && player.y <= origin.y + 0.5 && player.x > origin.x) { //player is to the right
				target = new Vector3 (origin.x + 5.5f, origin.y, 0);
			} else if (player.y >= origin.y - 0.5 && player.y <= origin.y + 0.5 && player.x < origin.x) { //player is to the left
				target = new Vector3 (origin.x - 5.5f, origin.y, 0);
			}
			transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * speed);
			isMovingOut = true;
		}

		if (isMovingOut)
			transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * speed);

		if (isMovingBack) {
			transform.position = Vector3.MoveTowards (transform.position, origin, Time.deltaTime * speedReset);

		}
	}
}
