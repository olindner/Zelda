/* A component for controlling the visual representation of the player character */

// For an explanation of why separation of aesthetics and behavior is important,
// Read the commentary within EECS494FunBallAesthetics.cs
// - AY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAestheticView : MonoBehaviour {

    /* Inspector Tunables */
    public PlayerController player_controller;
	public PlayerControl player_control;
    public Sprite SpriteUp;
	public Sprite SpriteDown;
	public Sprite SpriteLeft;
	public Sprite SpriteRight;
//	public Sprite[] SpriteDamageUp_R;
//	public Sprite[] SpriteDamageDown_R;
//	public Sprite[] SpriteDamageLeft_R;
//	public Sprite[] SpriteDamageRight_R;
//	public Sprite[] SpriteDamageUp_L;
//	public Sprite[] SpriteDamageDown_L;
//	public Sprite[] SpriteDamageLeft_L;
//	public Sprite[] SpriteDamageRight_L;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        // The aesthetic object should always represent the location of the player object.
        // Thus, we must move to the location of the player object every frame.
        // - AY

//		if (player_controller.remainingDamageFlashes > 0) {
//			if (player_controller.remainingDamageFrames > 0) {
//				player_controller.remainingDamageFrames--;
//				if (player_controller.remainingDamageFrames == 0) {
//					UnshowDamage ();
//				}
//			} else {
//				player_controller.remainingDamageFlashes--;
//				ShowDamage (player_controller.remainingDamageFlashes);
//			}
//		}

        transform.position = player_controller.transform.position;

        ProcessPlayerDirection();
        ProcessPlayerDamageStatus();
    }

    /* TODO: Change player sprite based on the direction the player_controller last moved in */
    void ProcessPlayerDirection()
    {
		//sorry for changing this, but I did it the way that Austin teaches
		//in the tutorial so that it changes between like both Sprite-up sprites
		//(one is left foot forward, other is right foot forward) etc.
		//Technically don't need this function even...so idk.
//		if (Input.GetKey(KeyCode.UpArrow))
//            GetComponent<SpriteRenderer>().sprite = SpriteUp;
//        else if (Input.GetKey(KeyCode.LeftArrow))
//			GetComponent<SpriteRenderer>().sprite = SpriteLeft;
//        else if (Input.GetKey(KeyCode.RightArrow))
//			GetComponent<SpriteRenderer>().sprite = SpriteRight;
//        else if (Input.GetKey(KeyCode.DownArrow))
//			GetComponent<SpriteRenderer>().sprite = SpriteDown;
    }

    /* TODO: Check if the player_controller is reporting damage. If so, flash a red color */
    void ProcessPlayerDamageStatus()
    {
//		//IDK if this is gonna work. If we can think of a better way to
//		//"report" damage I'd like to do that instead.
//		if (player_controller.receive_damage == true) {
//			//print("dude no I'm damaged");
//			ShowDamage (5);
//		}
    }

//	void ShowDamage(int flashes_left) {
//		//print ("entered ShowDamage");
//		player_controller.receive_damage = false;
//		player_controller.remainingDamageFlashes = flashes_left;
//		foreach (Material m in player_controller.materials) {
//			m.color = Color.red;
//		}
//		player_controller.remainingDamageFrames = player_controller.showDamageForFrames;
//	}
//
//	void UnshowDamage() {
//		for (int i = 0; i < player_controller.materials.Length; i++) {
//			player_controller.materials [i].color = player_controller.originalColors [i];
//		}
//	}
//			
}
