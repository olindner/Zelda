using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour {
	public Material material1;
	public Material material2;

	public Color colorStart = Color.red;
	public Color colorEnd = Color.black;
	public float duration = 15.0f;
	public Renderer rend;
	public float lerp;


	public bool notDead;
	//public bool lerpingMaterial;
	public bool lerpingColor;

	public static DeathScreen screen;

	void Start() {
		rend = GetComponent<Renderer>();
		rend.material = material1;
		notDead = true;
//		lerpingMaterial = false;
		lerpingColor = false;
		screen = this;
		lerp = 0f;
	}

	void Update() {
		if (PlayerController.instance.num_hearts <= 0) {
			notDead = false;
			rend.material = material2;
//			lerpingMaterial = true;
			lerpingColor = true;
		}

//		if (rend.material == material2) {
//			lerpingMaterial = false;
//			lerpingColor = true;
//		}

//		if (lerpingMaterial) {
//			print ("lerping materials");
//			float lerp = Mathf.PingPong(Time.time, duration) / duration;
//			rend.material.Lerp(material1, material2, lerp);
		if (lerpingColor) {
			if (lerp < 1) {
				print ("lerping colors");
				rend.material.color = Color.Lerp (colorStart, colorEnd, lerp);
				lerp += 0.015f;
			} else {
				rend.material.color = colorEnd;
			}
		}
	}
}
