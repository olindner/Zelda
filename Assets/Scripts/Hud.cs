using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hud : MonoBehaviour {

	public Text rupeeText;
	public Text keyText;
	public Text bombText;

	// Use this for initialization
	void Start () {
		
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
}
