using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Austin : MonoBehaviour {

	public float delay = 0.2f;
	private float timer;
	public Sprite[] sprites;
	private int here = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time >= timer) {
			if (here == 0) here = 1;
			else here = 0;
			GetComponent<SpriteRenderer>().sprite = sprites[here];
			timer = Time.time + delay;
		}
	}
}
