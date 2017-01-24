using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquamentus : MonoBehaviour {

	public Sprite[] sprites;
	public float spriteDelay;
	private float spriteTimer;
	private int health;
	private bool isMoving;
	private Vector3 target;

	// Use this for initialization
	void Start () {
		health = 10; //? Not sure
		isMoving = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

//		if (!isMoving) {
//			
//		}
	}
}
