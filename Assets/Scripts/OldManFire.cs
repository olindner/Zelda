using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManFire : MonoBehaviour {

	public Sprite[] animation;
	public int framechange = 6;
	public int num_frames_left = 0;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().sprite = animation [0];
		num_frames_left = framechange;
	}
	
	// Update is called once per frame
	void Update () {
		if (num_frames_left > 0) {
			num_frames_left--;
			if (num_frames_left == 0) {
				if (GetComponent<SpriteRenderer> ().sprite == animation [1])
					GetComponent<SpriteRenderer> ().sprite = animation [0];
				else
					GetComponent<SpriteRenderer> ().sprite = animation [1];
				num_frames_left = framechange;
			}
		}
		//can add stuff about shooting too if you want
	}
}
