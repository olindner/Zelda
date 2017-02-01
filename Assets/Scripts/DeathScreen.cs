using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour {
	public Material material1;
	public Material material2;

	public Color colorStart = Color.red;
	public Color colorEnd = Color.black;
	public float duration = 60.0f;
	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
		rend.material = material1;
	}
	void Update() {
		float lerp = Mathf.PingPong(Time.time, duration) / duration;
		rend.material.color = Color.Lerp(colorStart, colorEnd, lerp);
	}
}
