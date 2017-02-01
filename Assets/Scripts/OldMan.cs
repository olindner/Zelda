using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMan : MonoBehaviour {

	public float delay = 0.1f;
	public string message;
	public GUIText hi;

	// Update is called once per frame
	void Start () {
		message = hi.text;
		//hi = "";
	}

	void Update ()
	{
		if (RoomController.rc.active_col_index == 0 && RoomController.rc.active_row_index == 2) StartCoroutine(Letters());
	}

	IEnumerator Letters() {
		foreach (char letter in message.ToCharArray()) {
			hi.text += letter;
			yield return new WaitForSeconds(delay);
		}
	}
}
