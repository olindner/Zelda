using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {
	public static CameraPan c;

	public float easing = 0.15f;

	public float camZ;
	public PlayerController pc;
	public Vector3 current_pos;
	public float height;
	public float width;
	public float HUDheight;
	public bool panning_down;
	public bool panning_up;
	public bool panning_left;
	public bool panning_right;
	public Camera camera;
	public Vector3 destination;

	// Use this for initialization
	void Awake () {
		c = this;
		camZ = this.transform.position.z;
		HUDheight = 52f;
		panning_down = false;
		panning_up = false;
		panning_left = false;
		panning_right = false;
		foreach (Transform child in transform) {
			if (child.name == "Main Camera") {
				camera = child.GetComponent<Camera>();
			}
		}
		//height = 2 * camera.orthographicSize * (240.0f - HUDheight)/240.0f;
		//print ("height: " + height);
		//width = 2 * camera.orthographicSize * (256.0f / 240.0f);
		//print ("width: " + width);
		height = 15.0f * (240.0f - HUDheight)/240.0f;
		width = 16.0f;
		current_pos = new Vector3(this.transform.position.x, this.transform.position.y, camZ);
		destination = current_pos;
		print ("current_pos: " + current_pos);
	}
	
	// Update is called once per frame
	void Update () {
		if (panning_down) {
			current_pos = Vector3.Lerp (transform.position, destination, easing);
			transform.position = current_pos;
			current_pos = transform.position;
			if (Mathf.Abs(transform.position.y - destination.y) <= 0.1) {
				transform.position = destination;
				current_pos = destination;
				panning_down = false;
				//print ("done panning down!");
			}
		} else if (panning_right) {
			current_pos = Vector3.Lerp (transform.position, destination, easing);
			transform.position = current_pos;
			current_pos = transform.position;
			if (Mathf.Abs(transform.position.x - destination.x) <= 0.1) {
				transform.position = destination;
				current_pos = destination;
				panning_right = false;
			}
		} else if (panning_up) {
			current_pos = Vector3.Lerp (transform.position, destination, easing);
			transform.position = current_pos;
			current_pos = transform.position;
			if (Mathf.Abs(transform.position.y - destination.y) <= 0.1) {
				transform.position = destination;
				current_pos = destination;
				panning_up = false;
			}
		} else if (panning_left) {
			current_pos = Vector3.Lerp (transform.position, destination, easing);
			transform.position = current_pos;
			current_pos = transform.position;
			if (Mathf.Abs(transform.position.x - destination.x) <= 0.1) {
				print ("done panning left!");
				transform.position = destination;
				current_pos = destination;
				panning_left = false;
			}
		}
	}

//	//makes camera pan down to lower cell
//	public void panDown() {
//		current_pos.y -= height;
//		current_pos = Vector3.Lerp (transform.position, current_pos, easing);
//		transform.position = current_pos;
//	}
//
//	//makes camera pan down to right cell
//	public void panRight() {
//		current_pos.x += width;
//		current_pos = Vector3.Lerp (transform.position, current_pos, easing);
//		transform.position = current_pos;
//	}
}
