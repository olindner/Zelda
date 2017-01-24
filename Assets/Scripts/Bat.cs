using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour {

	public Sprite[] sprites;
	public float spriteDelay;
	private float spriteTimer;
	private int hi;
	public bool isMoving;
	private Vector3 target;
	public float sleepDelay;
	private float sleepTimer;
	public float speed;
	public float dist;

	// Use this for initialization
	void Start () {
		spriteTimer = Time.time + spriteDelay;
		hi = 0;
		isMoving = false;
		sleepTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time >= sleepTimer) { //no longer sleeping
			//alternate sprites
			if (Time.time >= spriteTimer) {
				GetComponent<SpriteRenderer> ().sprite = sprites [hi];
				if (hi == 0)
					hi = 1;
				else if (hi == 1)
					hi = 0;
				spriteTimer = Time.time + spriteDelay;
			}

			if (transform.position == target)
				isMoving = false;

			if (!isMoving) { //not moving so randomly choose a state
				int num = Random.Range (0, 99);
				if (num == 1) { //1% chance do sleep
					sleepTimer = Time.time + sleepDelay;
				} else { //99% to move
					isMoving = true;

					Vector3 rayUp = transform.TransformDirection (Vector3.up);
					Vector3 rayLeft = transform.TransformDirection (Vector3.left);
					Vector3 rayNE = transform.TransformDirection (new Vector3 (1, 1, 0));
					Vector3 rayNW = transform.TransformDirection (new Vector3 (-1, 1, 0));
					Vector3 raySE = transform.TransformDirection (new Vector3 (1, -1, 0));
					Vector3 raySW = transform.TransformDirection (new Vector3 (-1, -1, 0));

					while (true) {
						dist = Random.Range (-3f, 3f);
						//Generate a random target, equal likilihood of any 8 directions (4 cardinal, 4 diagonal)
						if (num >= 0 && num < 25) { //Up+Down
							target = new Vector3 (transform.position.x, transform.position.y + dist, 0);
							if (!Physics.Raycast (transform.position, rayUp, dist)) {
								print("Up/down okay");
								break;
							}
						} else if (num >= 25 && num < 50) { //Left+Right
							target = new Vector3 (transform.position.x + dist, transform.position.y, 0); 
							if (!Physics.Raycast (transform.position, rayLeft, dist)) {
								print("Left/right okay");
								break;
							}
						} else if (num >= 50 && num < 75) { //NE+SW
							if (dist >= 0) {
								target = new Vector3 (transform.position.x + Mathf.Sqrt (dist), transform.position.y + Mathf.Sqrt (dist), 0); 
								if (!Physics.Raycast (transform.position, rayNE, Mathf.Abs (dist))) {
									print("NE okay");
									break;
								}
							} else {
								target = new Vector3 (transform.position.x - Mathf.Sqrt (-dist), transform.position.y - Mathf.Sqrt (-dist), 0); 
								if (!Physics.Raycast (transform.position, raySW, Mathf.Abs (dist))) {
									print("SW okay");
									break;
								}
							}
						} else { //NW+SE
							if (dist >= 0) {
								target = new Vector3 (transform.position.x - Mathf.Sqrt (dist), transform.position.y + Mathf.Sqrt (dist), 0); 
								if (!Physics.Raycast (transform.position, rayNW, Mathf.Abs (dist))) {
									print("NW okay");
									break;
								}
							} else {
								target = new Vector3 (transform.position.x + Mathf.Sqrt (-dist), transform.position.y - Mathf.Sqrt (-dist), 0); 
								if (!Physics.Raycast (transform.position, raySE, Mathf.Abs (dist))) {
									print("SE okay");
									break;
								}
							}
						}
					}
				}
			}
			//Set speed changes here?
			transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * speed);
		}
	}
}
