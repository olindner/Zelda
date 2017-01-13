using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

//FUCK YOU KURT

public class PlayerControl : MonoBehaviour {

	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;

	// Use this for initialization
	void Start () {
		animation_state_machine = new StateMachine ();
		animation_state_machine.ChangeState (new StateIdleWithSprite (this, 
			GetComponent<SpriteRenderer> (), link_run_down[0]));
	}
	
	// Update is called once per frame
	void Update () {
		animation_state_machine.Update ();

	}
}