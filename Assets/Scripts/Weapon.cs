using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
	none,
	sword,
	bow,
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

	public Weapon(WeaponType type, WeaponDefinition def, GameObject w_go) {
		this._type = type;
		this.def = def;
		this.w_go = w_go;
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

//	void OnTriggerEnter(Collider collider) {
//		if (collider.gameObject.tag == "Wall") {
//			print ("omg a wall nooo");
//			Destroy (this);
//			print ("destroyed!");
//		}
//	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
