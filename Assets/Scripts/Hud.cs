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

	// Use this for initialization
	void Start ()
	{
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
	void Update () {
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
