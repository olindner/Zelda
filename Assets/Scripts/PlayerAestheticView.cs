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
    public Sprite SpriteUp;
	public Sprite SpriteDown;
	public Sprite SpriteLeft;
	public Sprite SpriteRight;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        // The aesthetic object should always represent the location of the player object.
        // Thus, we must move to the location of the player object every frame.
        // - AY
        transform.position = player_controller.transform.position;

        ProcessPlayerDirection();
        ProcessPlayerDamageStatus();
    }

    /* TODO: Change player sprite based on the direction the player_controller last moved in */
    void ProcessPlayerDirection()
    {
		if (Input.GetKey(KeyCode.UpArrow))
            GetComponent<SpriteRenderer>().sprite = SpriteUp;
        else if (Input.GetKey(KeyCode.LeftArrow))
			GetComponent<SpriteRenderer>().sprite = SpriteLeft;
        else if (Input.GetKey(KeyCode.RightArrow))
			GetComponent<SpriteRenderer>().sprite = SpriteRight;
        else if (Input.GetKey(KeyCode.DownArrow))
			GetComponent<SpriteRenderer>().sprite = SpriteDown;
    }

    /* TODO: Check if the player_controller is reporting damage. If so, flash a red color */
    void ProcessPlayerDamageStatus()
    {

    }
}
