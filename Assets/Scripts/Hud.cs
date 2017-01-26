using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hud : MonoBehaviour {

	public Text rupeeText;
	public Text keyText;
	public Text bombText;
	public Image[] redHearts;
	public Image[] whiteHearts;
	public Image[] halfHearts;
	private int slot; //keeps track of the "current" health slot
	private bool hidden;
	private RectTransform rt;
	public float ease_factor = 0.1f;
	public Image Red0Empty;
	public Image Blue0Empty;
	public float blinkDelay = 0.2f;
	private float blinkTimer;
	public Image map;
	public Image compass;
	public Image redDot;
	//public Image grayDot;
	public Image blueMapFull;

	// Use this for initialization
	void Start ()
	{
		blinkTimer = Time.time + blinkDelay;
		Red0Empty.enabled = true;
		Blue0Empty.enabled = false;
		redDot.enabled = false;
		//grayDot.enabled = false;
		blueMapFull.enabled = false;

		map.enabled = false;
		compass.enabled = false;

		hidden = true;
		rt = GetComponent<RectTransform> ();
		
		float health = PlayerController.instance.num_hearts;
		int capacity = PlayerController.instance.heart_capacity;
		slot = (int)Mathf.Floor(health);

		//Display red hearts
		int count = (int)Mathf.Floor (health);
		foreach (Image i in redHearts) {
			if (count > 0)
				i.enabled = true;
			else
				i.enabled = false;
			count--;
		}

		//Display half hearts
//		if (health % 1 != 0) {
			int num = (int)Mathf.Floor (health);
			foreach (Image i in halfHearts) {
				if (num == 0 && health % 1.0 != 0.0)
					i.enabled = true;
				else
					i.enabled = false;
				num--;
			}
//		}

		//Display white hearts
//		if (capacity - health > 0.5) {
			int hi = (int)Mathf.Ceil (health);
			int bye = 0;
			foreach (Image i in whiteHearts) {
			if (bye >= hi && bye < capacity)
					i.enabled = true;
				else
					i.enabled = false;
				bye++;
			}
//		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Return)) {
			hidden = !hidden;
		}

		if (Time.time >= blinkTimer && !hidden) { //issue: only when !hidden (menu is down)
			if (Red0Empty.enabled == true) {
				Red0Empty.enabled = false;
				Blue0Empty.enabled = true;
			} else if (Blue0Empty.enabled == true) {
				Blue0Empty.enabled = false;
				Red0Empty.enabled = true;
			} 
//			else if (redDot.enabled == true) {
//				redDot.enabled = false;
//				grayDot.enabled = true;
//			} else if (grayDot.enabled == true) {
//				grayDot.enabled = false;
//				redDot.enabled = true;
//			}
			blinkTimer = Time.time + blinkDelay;
		}

		if (PlayerController.instance.has_map) {
			map.enabled = true;
			//create map on menu
			blueMapFull.enabled = true;
		}
		if (PlayerController.instance.has_compass) { //issue? does everytime
			compass.enabled = true;
			//create blinking red finish dot
			redDot.enabled = true;
		}

		Vector2 desired_ui_position = new Vector2(400, 375);

		if (!hidden)
            //desired_ui_position = new Vector2(400, -241);
			desired_ui_position = new Vector2(400, -41);
        rt.anchoredPosition += (desired_ui_position - rt.anchoredPosition) * ease_factor;

		int rupees = PlayerController.instance.num_rupees;
		rupeeText.text = "x" + rupees.ToString();
		int keys = PlayerController.instance.num_keys;
		keyText.text = "x" + keys.ToString();
		int bombs = PlayerController.instance.num_bombs;
		bombText.text = "x" + bombs.ToString();
	}

	public void TookDamage ()
	{
		if (halfHearts [slot-1].IsActive ()) { //if current "slot" is a half heart,
			halfHearts [slot-1].enabled = false; //remove halfheart,
			whiteHearts [slot-1].enabled = true; //and make it an empty one
			slot--;
		} 
		else if (redHearts [slot-1].IsActive ()) { //if its a full heart,
			redHearts[slot-1].enabled = false; //remove it,
			halfHearts[slot-1].enabled = true; //and make it a halfheart
		}
	}

	//Replenishing heart (think it heals one full heart amount?)
	public void CollectedHeart () {

	}

	//Increases heart capacity by one
	public void CollectedBIGHeart () {
		
	}
}
