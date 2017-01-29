using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	public GameObject leftArm;
	public GameObject rightArm;
	public float delay;
	private float extendTimer;
	private float retractTimer;
	public float speed;

	// Use this for initialization
	void Start () {
		extendTimer = Time.time + delay;
		retractTimer = extendTimer + delay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time >= extendTimer) {
			rightArm.transform.position = Vector3.MoveTowards (transform.position, new Vector3(transform.position.x + 20f, transform.position.y, transform.position.z), Time.deltaTime * speed);
			leftArm.transform.position = Vector3.MoveTowards (transform.position, new Vector3(transform.position.x - 20f, transform.position.y, transform.position.z), Time.deltaTime * speed);
			extendTimer = Time.time + delay;
		} else if (Time.time >= retractTimer) {
			rightArm.transform.position = Vector3.MoveTowards (transform.position, new Vector3(transform.position.x - 20f, transform.position.y, transform.position.z), Time.deltaTime * speed);
			leftArm.transform.position = Vector3.MoveTowards (transform.position, new Vector3(transform.position.x + 20f, transform.position.y, transform.position.z), Time.deltaTime * speed);
			retractTimer = Time.time + delay;
		}
	}
}
