using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {
	static public CameraPan c;

	public float easing = 0.05f;

	public float camZ;
	public PlayerController pc;
	Vector3 current_pos;
	float height;
	float width;
	float leftedge;
	float rightedge;
	float topedge;
	float bottomedge;
	float HUDheight;

	// Use this for initialization
	void Awake () {
		c = this;
		camZ = this.transform.position.z;
		HUDheight = 52f;
		height = 2 * this.GetComponent<Camera> ().orthographicSize - HUDheight;
		print ("height: " + height);
		width = 2 * this.GetComponent<Camera> ().orthographicSize * (256.0f / 240.0f);
		print ("width: " + width);
		current_pos = new Vector3(this.transform.position.x, this.transform.position.y, camZ);
		print ("current_pos: " + current_pos);
		leftedge = current_pos.x - this.GetComponent<Camera> ().orthographicSize * (256.0f / 240.0f) + 5.0f;
		print ("leftedge: " + leftedge);
		rightedge = current_pos.x + this.GetComponent<Camera> ().orthographicSize * (256.0f / 240.0f) - 5.0f;
		print ("rightedge: " + rightedge);
		bottomedge = current_pos.y - this.GetComponent<Camera> ().orthographicSize + 5.0f;
		print ("bottomedge: " + bottomedge);
		topedge = current_pos.y + this.GetComponent<Camera> ().orthographicSize - HUDheight - 5.0f;
		print ("topedge: " + topedge);
	}
	
	// Update is called once per frame
	void Update () {
		if (pc.transform.position.x <= leftedge || pc.transform.position.x >= rightedge 
			|| pc.transform.position.y <= bottomedge || pc.transform.position.y >= topedge) {
			if (pc.transform.position.x <= leftedge) {
				//pan left
				current_pos.x -= width;
			} else if (pc.transform.position.x >= rightedge) {
				//pan right
				current_pos.x += width;
			} else if (pc.transform.position.y <= bottomedge) {
				//pan down
				current_pos.y -= height;
			} else { //if (pc.transform.position.y >= topedge)
				//pan up
				current_pos.y += height;
			}
			current_pos = Vector3.Lerp (transform.position, current_pos, easing);
			transform.position = current_pos;

			leftedge = current_pos.x - this.GetComponent<Camera> ().orthographicSize * (256.0f / 240.0f) + 5.0f;
			rightedge = current_pos.x + this.GetComponent<Camera> ().orthographicSize * (256.0f / 240.0f) - 5.0f;
			bottomedge = current_pos.y - this.GetComponent<Camera> ().orthographicSize + 5.0f;
			topedge = current_pos.y + this.GetComponent<Camera> ().orthographicSize - HUDheight - 5.0f;
		}
	}
}
