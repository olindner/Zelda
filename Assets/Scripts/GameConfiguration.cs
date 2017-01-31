﻿/* A component for storing application-wide config settings */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfiguration : MonoBehaviour {

    // NOTE: more cheats may be necessary. Please consult the project spec.
    public static bool cheat_invincibility = false;
    public static bool mitchell_bloch_superstar_mode = false; // if you're bored...

    /* Inspector Tunables */
    public int target_framerate = 60;

    void Awake()
    {
        Application.targetFrameRate = target_framerate;
    }

    void Update()
    {
        ProcessCheats();
    }

    /* Flip cheats on and off in response to user input */
    void ProcessCheats()
    {
        // Note: standardized controls may be found in project spec.
		if (Input.GetKey(KeyCode.F1)) {
			PlayerController.instance.DelayedRestart (PlayerController.instance.gameRestartDelay);
		} else if (Input.GetKey(KeyCode.F2)) {
			PlayerController.instance.transform.position = new Vector3 (39.52f, 38.79f, 0f);
			CameraPan.c.transform.position = new Vector3 (39.52f, 38.79f, -11f);
			RoomController.rc.active_col_index = 2;
			RoomController.rc.active_row_index = 9;
			PlayerController.instance.num_frozen_frames = 24;
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		} else if (Input.GetKey(KeyCode.F3)) {
			PlayerController.instance.cheat_health = true;
		} else if (Input.GetKey(KeyCode.F4)) {
			PlayerController.instance.cheat_items = true;
		}
    }
}
