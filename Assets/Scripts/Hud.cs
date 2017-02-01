using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hud : MonoBehaviour {

	public Text rupeeText;
	public Text keyText;
	public Text bombText;
	public Text oldmanText;
	public Image[] redHearts;
	public Image[] whiteHearts;
	public Image[] halfHearts;
	private int slot; //keeps track of the "current" health slot
	private bool hidden;
	private RectTransform rt;
	public float ease_factor = 0.1f;
	public Image[] weapons;
	public Image[] slots;
	public Image[] isSelected;
	public Image[] BSelected;
	public float blinkDelay;
	private float blinkTimer;
	public Image map;
	public Image compass;
	public Image redDot;
	public Image grayDot;
	public Image blueMapFull;
	public int activeSlot = 0;
	private bool redSet = false;
	public GameObject[] weaponPrefabs;

	// Use this for initialization
	void Start ()
	{
		blinkTimer = Time.time + blinkDelay;
		foreach (Image i in weapons) {
			i.enabled = false;
		}
		foreach (Image e in slots) {
			e.enabled = false;
		}
		foreach (Image g in isSelected) {
			g.enabled = false;
		}
		foreach (Image h in BSelected) {
			h.enabled = false;
		}
		slots[0].enabled = true; //enable red0empty first

		redDot.enabled = false;
		grayDot.enabled = false;
		blueMapFull.enabled = false;

		map.enabled = false;
		compass.enabled = false;

		hidden = true;
		rt = GetComponent<RectTransform> ();

		oldmanText.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (RoomController.rc.active_col_index == 0 && RoomController.rc.active_row_index == 2) {
			oldmanText.enabled = true;
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			hidden = !hidden;
			//find way to freeze game
		}

		float health = PlayerController.instance.num_hearts;
		int capacity = PlayerController.instance.heart_capacity;
		slot = (int)Mathf.Floor (health);

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

		if (!hidden) {
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				if (activeSlot == 0)
					activeSlot = 1;
				else if (activeSlot == 1)
					activeSlot = 2;
				else if (activeSlot == 2)
					activeSlot = 3;
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (activeSlot == 3)
					activeSlot = 2;
				else if (activeSlot == 2)
					activeSlot = 1;
				else if (activeSlot == 1)
					activeSlot = 0;
			}
		}

		if (Time.time >= blinkTimer) {
			if (activeSlot == 0) {
				slots [2].enabled = false; //disable slot 1
				slots [3].enabled = false; //disable slot 1
				if (slots [0].enabled == false) {
					slots [0].enabled = true;
					slots [1].enabled = false;
				} else if (slots [1].enabled == false) {
					slots [1].enabled = true;
					slots [0].enabled = false;
				} 
			} else if (activeSlot == 1) {
				slots [0].enabled = false; //disable slot 0
				slots [1].enabled = false; //disable slot 0
				slots [4].enabled = false; //disable slot 2
				slots [5].enabled = false; //disable slot 2
				if (slots [2].enabled == false) {
					slots [2].enabled = true;
					slots [3].enabled = false;
				} else if (slots [3].enabled == false) {
					slots [3].enabled = true;
					slots [2].enabled = false;
				} 
			} else if (activeSlot == 2) {
				slots [2].enabled = false; //disable slot 1
				slots [3].enabled = false; //disable slot 1
				slots [6].enabled = false; //disable slot 3
				slots [7].enabled = false; //disable slot 3
				if (slots [4].enabled == false) {
					slots [4].enabled = true;
					slots [5].enabled = false;
				} else if (slots [5].enabled == false) {
					slots [5].enabled = true;
					slots [4].enabled = false;
				} 
			} else if (activeSlot == 3) {
				slots [4].enabled = false; //disable slot 2
				slots [5].enabled = false; //disable slot 2
				if (slots [6].enabled == false) {
					slots [6].enabled = true;
					slots [7].enabled = false;
				} else if (slots [7].enabled == false) {
					slots [7].enabled = true;
					slots [6].enabled = false;
				} 
			}
			if (redDot.enabled == true) {
				redDot.enabled = false;
				grayDot.enabled = true;
			} else if (grayDot.enabled == true) {
				grayDot.enabled = false;
				redDot.enabled = true;
			}
			blinkTimer = Time.time + blinkDelay;
		}

		//Weapon selection (put into "B slot")
		if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
			bool okay = false;
			if (activeSlot == 0 && PlayerController.instance.have_boomerang)
				okay = true;
			if (activeSlot == 1 && PlayerController.instance.num_bombs > 0)
				okay = true;
			if (activeSlot == 2 && PlayerController.instance.has_bow)
				okay = true;
			if (activeSlot == 3 && PlayerController.instance.has_chomper)
				okay = true;

			if (okay) {
				foreach (Image g in isSelected) {
					g.enabled = false;
				}
				isSelected [activeSlot].enabled = true;
				foreach (Image h in BSelected) {
					h.enabled = false;
				}
				BSelected [activeSlot].enabled = true;
				PlayerController.instance.selected_weapon_prefab_b = weaponPrefabs[activeSlot];
			}
		}

		if (PlayerController.instance.have_boomerang) {
			weapons [0].enabled = true;
		}
		if (PlayerController.instance.num_bombs > 0) {
			weapons [1].enabled = true;
		} else
			weapons [1].enabled = false;
		if (PlayerController.instance.has_bow) {
			weapons [2].enabled = true;
		}
		if (PlayerController.instance.has_chomper) {
			weapons [3].enabled = true;
		}

		if (PlayerController.instance.has_map) {
			map.enabled = true;
			//create map on menu
			blueMapFull.enabled = true;
		}
		if (PlayerController.instance.has_compass) {
			compass.enabled = true;
			//create blinking red finish dot
			if (!redSet) {
				redDot.enabled = true;
				redSet = true;
			}
		}
			
		Vector2 desired_ui_position = new Vector2(-200, 220); //when hidden above

		if (!hidden)
            //desired_ui_position = new Vector2(400, -241);
			desired_ui_position = new Vector2(-200, -210); //when displayed
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

	IEnumerator Letters() {
		string message = oldmanText.text;
		oldmanText.text = "";
		foreach (char letter in message.ToCharArray()) {
			oldmanText.text += letter;
			yield return new WaitForSeconds(0.1f);
		}
	}
}
