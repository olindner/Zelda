using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
	none,
	sword,
	arrow,
	boomerang
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

//	void OnCollisionEnter(Collision coll) {
//		if (this.def.type == WeaponType.boomerang && coll.gameObject.tag == "Player") {
//			print ("Boomerang triggered player");
//			print ("boom velocity " + this.gameObject.GetComponent<Rigidbody> ().velocity);
//			if (this.gameObject.GetComponent<Rigidbody> ().velocity.x > 0) {
//				this.gameObject.GetComponent<Rigidbody> ().velocity = this.def.velocity * Vector3.left;
//			} else if (this.gameObject.GetComponent<Rigidbody> ().velocity.x < 0) {
//				this.gameObject.GetComponent<Rigidbody> ().velocity = this.def.velocity * Vector3.right;
//			} else if (this.gameObject.GetComponent<Rigidbody> ().velocity.y > 0) {
//				this.gameObject.GetComponent<Rigidbody> ().velocity = this.def.velocity * Vector3.down;
//			} else {
//				this.gameObject.GetComponent<Rigidbody> ().velocity = this.def.velocity * Vector3.up;
//			}
//		}
//	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (type == WeaponType.boomerang) {
			Vector3 fwd;
			if (this.gameObject.GetComponent<Rigidbody> ().velocity.x > 0) {
				fwd = transform.TransformDirection (Vector3.right);
			} else if (this.gameObject.GetComponent<Rigidbody> ().velocity.x < 0) {
				fwd = transform.TransformDirection (Vector3.left);
			} else if (this.gameObject.GetComponent<Rigidbody> ().velocity.y > 0) {
				fwd = transform.TransformDirection (Vector3.up);
			} else {
				fwd = transform.TransformDirection (Vector3.down);
			}
			int layermask = 1 << 10;
			layermask = ~layermask;
			if (Physics.Raycast (transform.position, fwd, 0.5f, layermask)) {
				print ("ah shoot something is there!!!");
				this.GetComponent<Rigidbody> ().velocity *= -1;
			}
		}
	}
}
