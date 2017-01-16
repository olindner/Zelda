/* The following is an example of a lightweight state machine (explained more in the tutorial videos).
 * It is not recommended for implementing Link, but it may be useful for enemies.
 */ 

using UnityEngine;
using System.Linq;

// State Machines are responsible for processing states, notifying them when they're about to begin or conclude, etc.
public class StateMachine
{
	private State _current_state;
	
	public void ChangeState(State new_state)
	{
		if(_current_state != null)
		{
			_current_state.OnFinish();
		}
		
		_current_state = new_state;
		// States sometimes need to reset their machine. 
		// This reference makes that possible.
		_current_state.state_machine = this;
		_current_state.OnStart();
	}
	
	public void Reset()
	{
		if(_current_state != null)
			_current_state.OnFinish();
		_current_state = null;
	}
	
	public void Update()
	{
		if(_current_state != null)
		{
			float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
			_current_state.OnUpdate(time_delta_fraction);
		}
	}

	public bool IsFinished()
	{
		return _current_state == null;
	}
}

// A State is merely a bundle of behavior listening to specific events, such as...
// OnUpdate -- Fired every frame of the game.
// OnStart -- Fired once when the state is transitioned to.
// OnFinish -- Fired as the state concludes.
// State Constructors often store data that will be used during the execution of the State.
public class State
{
	// A reference to the State Machine processing the state.
	public StateMachine state_machine;
	
	public virtual void OnStart() {}
	public virtual void OnUpdate(float time_delta_fraction) {} // time_delta_fraction is a float near 1.0 indicating how much more / less time this frame took than expected.
	public virtual void OnFinish() {}
	
	// States may call ConcludeState on themselves to end their processing.
	public void ConcludeState() { state_machine.Reset(); }
}

// A State that takes a renderer and a sprite, and implements idling behavior.
// The state is capable of transitioning to a walking state upon key press.
public class StateIdleWithSprite : State
{
	PlayerController pc;
	SpriteRenderer renderer;
	Sprite sprite;
	
	public StateIdleWithSprite(PlayerController pc, SpriteRenderer renderer, Sprite sprite)
	{
		this.pc = pc;
		this.renderer = renderer;
		this.sprite = sprite;
	}
	
	public override void OnStart()
	{
		renderer.sprite = sprite;
		if (pc.link_run_down.Contains (sprite))
			pc.current_direction = Direction.SOUTH;
		else if (pc.link_run_left.Contains (sprite))
			pc.current_direction = Direction.WEST;
		else if (pc.link_run_up.Contains (sprite))
			pc.current_direction = Direction.NORTH;
		else if (pc.link_run_right.Contains (sprite))
			pc.current_direction = Direction.EAST;
	}
	
	public override void OnUpdate(float time_delta_fraction)
	{
		if(pc.current_state == EntityState.ATTACKING)
			return;

		// Transition to walking animations on key press.
		if (pc.num_cooldown_frames == 0) {
			if (Input.GetKeyDown (KeyCode.DownArrow))
				state_machine.ChangeState (new StatePlayAnimationForHeldKey (pc, renderer, pc.link_run_down, 6, KeyCode.DownArrow));
			if (Input.GetKeyDown (KeyCode.UpArrow))
				state_machine.ChangeState (new StatePlayAnimationForHeldKey (pc, renderer, pc.link_run_up, 6, KeyCode.UpArrow));
			if (Input.GetKeyDown (KeyCode.RightArrow))
				state_machine.ChangeState (new StatePlayAnimationForHeldKey (pc, renderer, pc.link_run_right, 6, KeyCode.RightArrow));
			if (Input.GetKeyDown (KeyCode.LeftArrow))
				state_machine.ChangeState (new StatePlayAnimationForHeldKey (pc, renderer, pc.link_run_left, 6, KeyCode.LeftArrow));
		}
	}
}

// A State for playing an animation until a particular key is released.
// Good for animations such as walking.
public class StatePlayAnimationForHeldKey : State
{
	PlayerController pc;
	SpriteRenderer renderer;
	KeyCode key;
	Sprite[] animation;
	Sprite current;
	int animation_length;
	float animation_progression;
	float animation_start_time;
	int fps;
	
	public StatePlayAnimationForHeldKey(PlayerController pc, SpriteRenderer renderer, Sprite[] animation, int fps, KeyCode key)
	{
		this.pc = pc;
		this.renderer = renderer;
		this.key = key;
		this.animation = animation;
		this.current = animation [0];
		this.animation_length = animation.Length;
		this.fps = fps;
		
		if(this.animation_length <= 0)
			Debug.LogError("Empty animation submitted to state machine!");
	}
	
	public override void OnStart()
	{
		animation_start_time = Time.time;
		if (key == KeyCode.DownArrow)
			pc.current_direction = Direction.SOUTH;
		else if (key == KeyCode.LeftArrow)
			pc.current_direction = Direction.WEST;
		else if (key == KeyCode.RightArrow)
			pc.current_direction = Direction.EAST;
		else if (key == KeyCode.UpArrow)
			pc.current_direction = Direction.NORTH;
	}
	
	public override void OnUpdate(float time_delta_fraction)
	{
		if(pc.current_state == EntityState.ATTACKING)
			return;

		if(this.animation_length <= 0)
		{
			Debug.LogError("Empty animation submitted to state machine!");
			return;
		}
		
		// Modulus is necessary so we don't overshoot the length of the animation.
		int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation_length);
		renderer.sprite = animation[current_frame_index];
		
		// If another key is pressed, we need to transition to a different walking animation.
		if(Input.GetKeyDown(KeyCode.DownArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_down, 6, KeyCode.DownArrow));
		else if(Input.GetKeyDown(KeyCode.UpArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_up, 6, KeyCode.UpArrow));
		else if(Input.GetKeyDown(KeyCode.RightArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_right, 6, KeyCode.RightArrow));
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_left, 6, KeyCode.LeftArrow));
		
		// If we detect the specified key has been released, return to the idle state.
		else if(!Input.GetKey(key) || pc.num_cooldown_frames > 0)
			state_machine.ChangeState(new StateIdleWithSprite(pc, renderer, animation[1]));
	}
}

// A State for playing an animation when damaged.
//public class StatePlayAnimationForDamage : State
//{
//	PlayerController pc;
//	SpriteRenderer renderer;
//	KeyCode key;
//	Sprite[] animation;
//	Sprite current;
//	int animation_length;
//	float animation_progression;
//	float animation_start_time = -1.0f; //this should be -1 if you want it set to the actual time
//	int fps;
//	float cooldown_time;
//	float total_damage_time;
//
//	public StatePlayAnimationForDamage(PlayerController pc, SpriteRenderer renderer, Sprite[] animation, int fps, float cooldown, float damage_time, KeyCode key, float start_time)
//	{
//		this.pc = pc;
//		this.renderer = renderer;
//		this.key = key;
//		this.animation = animation;
//		this.current = animation [0];
//		this.animation_length = animation.Length;
//		this.cooldown_time = cooldown;
//		this.total_damage_time = damage_time;
//		this.fps = fps;
//		if (start_time != -1.0f) {
//			this.animation_start_time = start_time;
//		}
//
//		if(this.animation_length <= 0)
//			Debug.LogError("Empty animation submitted to state machine!");
//	}
//
//	public override void OnStart()
//	{
//		//only set animation_start_time to actual time if none was given
//		if (animation_start_time != -1.0f)
//			animation_start_time = Time.time;
//	}
//
//	public override void OnUpdate(float time_delta_fraction)
//	{
//		if(pc.current_state == EntityState.ATTACKING)
//			return;
//
//		if(this.animation_length <= 0)
//		{
//			Debug.LogError("Empty animation submitted to state machine!");
//			return;
//		}
//
//		//this should stop the damage animation after "total damage time"
//		if (Time.time - animation_start_time > total_damage_time) {
//			if (pc.link_down_damage.Contains (current))
//				state_machine.ChangeState (new StateIdleWithSprite (pc, renderer, pc.link_run_down [1]));
//			else if (pc.link_left_damage.Contains (current))
//				state_machine.ChangeState (new StateIdleWithSprite (pc, renderer, pc.link_run_left [1]));
//			else if (pc.link_up_damage.Contains (current))
//				state_machine.ChangeState (new StateIdleWithSprite (pc, renderer, pc.link_run_up [1]));
//			else if (pc.link_right_damage.Contains (current))
//				state_machine.ChangeState (new StateIdleWithSprite (pc, renderer, pc.link_run_right [1]));
//		}
//
//		// Modulus is necessary so we don't overshoot the length of the animation.
//		int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation_length);
//		renderer.sprite = animation[current_frame_index];
//
//		if (Time.time - animation_start_time > cooldown_time) {
//			// If another key is pressed, we need to transition to a different walking animation.
//			if (Input.GetKeyDown (KeyCode.DownArrow))
//				state_machine.ChangeState (new StatePlayAnimationForDamage (pc, renderer, pc.link_down_damage, 6, cooldown_time, total_damage_time, KeyCode.DownArrow, animation_start_time));
//			else if (Input.GetKeyDown (KeyCode.UpArrow))
//				state_machine.ChangeState (new StatePlayAnimationForDamage (pc, renderer, pc.link_up_damage, 6, cooldown_time, total_damage_time, KeyCode.UpArrow, animation_start_time));
//			else if (Input.GetKeyDown (KeyCode.RightArrow))
//				state_machine.ChangeState (new StatePlayAnimationForDamage (pc, renderer, pc.link_right_damage, 6, cooldown_time, total_damage_time, KeyCode.RightArrow, animation_start_time));
//			else if (Input.GetKeyDown (KeyCode.LeftArrow))
//				state_machine.ChangeState (new StatePlayAnimationForDamage (pc, renderer, pc.link_left_damage, 6, cooldown_time, total_damage_time, KeyCode.LeftArrow, animation_start_time));
//			//eventually we can add in "static" flashing (so Link isn't "running" when
//			//there is no key down), but that's not super important right now
//		}
//	}
//}

// Additional recommended states:
// StateDeath
// StateDamaged
// StateWeaponSwing
// StateVictory

// Additional control states:
// LinkNormalMovement.
// LinkStunnedState.
