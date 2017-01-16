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
	public float delayBetweenShots = 0;
	public float velocity = 0;
	public GameObject weaponPrefab;
}


public class Weapon : MonoBehaviour {
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
	public WeaponDefinition[] weaponDefinitions;

	[SerializeField]
	private WeaponType _type;
	public WeaponDefinition def;

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
		if (type == null) {
			this.gameObject.SetActive (false);
			return;
		} else {
			this.gameObject.SetActive (true);
		}
	}

	static public WeaponDefinition getWeaponDefinition(WeaponType wt) {
		if (W_DEFS.ContainsKey (wt))
			return W_DEFS [wt];

		return (new WeaponDefinition ());
	}

	// Use this for initialization
	void Start () {
		W_DEFS = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions) {
			W_DEFS [def.type] = def;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
