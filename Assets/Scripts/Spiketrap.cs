using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiketrap : MonoBehaviour {

	private Vector3 origin;
	private bool isMoving;

	// Use this for initialization
	void Start () {
		origin = transform.position;
		isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (!isMoving) {
			Vector3 pos = PlayerController.instance.transform.position; //get player position (could also do with a raycast)

			if (pos.x == origin.x) { //eventually do round/modulo to check edge of square, not middle
				
			} 
			else if (pos.y == origin.y) {
				
			}
		}
	}
}
