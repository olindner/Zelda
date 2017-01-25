using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
	none,
	sword,
	arrow,
	boomerang,
	bomb
}

[System.Serializable]
public class WeaponDefinition {
	public WeaponType type = WeaponType.none;
	public Sprite[] sprites_dlur; //dlur meaning down left up right, order of sprites
	public int delayBetweenShots = 0;
	public float velocity = 0f;
	public GameObject weaponPrefab;
}


public class Weapon : MonoBehaviour {

	[SerializeField]
	private WeaponType _type;
	public WeaponDefinition def;
	public GameObject w_go;

	public PlayerController pc;
	Vector3 fwd;
	public Vector3 target1;
	public bool on_way_back;

	public Weapon(WeaponType type, WeaponDefinition def, GameObject w_go, PlayerController pc) {
		this._type = type;
		this.def = def;
		this.w_go = w_go;
		this.pc = pc;
	}

	public WeaponType type {
		get {
			return _type;
		}
		set {
			SetType (value);
		}
	}

	public void SetType(WeaponType eType) {
		_type = eType;
	}

	void OnCollisionEnter(Collision coll) {
		//print ("collided with something!");
		if (this.type == WeaponType.boomerang && (coll.gameObject.tag == "Wall"
			|| coll.gameObject.tag == "DoorUp" || coll.gameObject.tag == "DoorLeft"
			|| coll.gameObject.tag == "DoorRight" || coll.gameObject.tag == "DoorDown"
			|| coll.gameObject.tag == "LockedDoorUp" || coll.gameObject.tag == "LockedDoorLeft"
			|| coll.gameObject.tag == "LockedDoorRight")) {
			//print ("Boomerang triggered player");
			//print ("boom velocity " + this.gameObject.GetComponent<Rigidbody> ().velocity);
//			if (PlayerController.instance.current_direction == Direction.EAST) {
//				this.gameObject.GetComponent<Rigidbody> ().velocity = this.def.velocity * Vector3.left;
//				//print ("new velocity " + this.def.velocity * Vector3.left);
//			} else if (PlayerController.instance.current_direction == Direction.WEST) {
//				this.gameObject.GetComponent<Rigidbody> ().velocity = this.def.velocity * Vector3.right;
//			} else if (PlayerController.instance.current_direction == Direction.NORTH) {
//				this.gameObject.GetComponent<Rigidbody> ().velocity = this.def.velocity * Vector3.down;
//			} else {
//				this.gameObject.GetComponent<Rigidbody> ().velocity = this.def.velocity * Vector3.up;
//			}
			on_way_back = true;
			Vector3 new_direction = PlayerController.instance.transform.position - this.transform.position;
			this.gameObject.GetComponent<Rigidbody> ().velocity = new_direction.normalized * this.def.velocity;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (type == WeaponType.boomerang) {
			transform.Rotate (0, 0, 3*Time.time);
			if (on_way_back && !PlayerController.instance.have_boomerang) {
				Vector3 new_direction = PlayerController.instance.transform.position - this.transform.position;
				this.gameObject.GetComponent<Rigidbody> ().velocity = new_direction.normalized * this.def.velocity;
			}
		}
	}
}
