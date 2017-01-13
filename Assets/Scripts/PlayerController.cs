using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    /* Inspector Tunables */
    public float PlayerMovementVelocity;
	public int showDamageForFrames = 2;

    /* Private Data */
    Rigidbody rb;
	public bool receive_damage = false;
	public Material[] materials;
	public int remainingDamageFrames = 0;
	public int remainingDamageFlashes = 0;
	public Color[] originalColors;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

//		materials = Utils.GetAllMaterials (gameObject);
//		originalColors = new Color[materials.Length];
//		for (int i = 0; i < materials.Length; i++) {
//			originalColors [i] = materials [i].color;
//		}
    }
    
    // Update is called once per frame
    void Update () {
        ProcessMovement();
        ProcessAttacks();
    }

    /* TODO: Deal with user-invoked movement of the player character */
    void ProcessMovement ()
    {
        Vector3 desired_velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            desired_velocity = new Vector3(0, 1, 0);
        else if (Input.GetKey(KeyCode.LeftArrow))
            desired_velocity = new Vector3(-1, 0, 0);
        else if (Input.GetKey(KeyCode.RightArrow))
            desired_velocity = Vector3.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            desired_velocity = Vector3.down;

        rb.velocity = desired_velocity * PlayerMovementVelocity;

        /* NOTE:
         * A reminder to study and implement the grid-movement mechanic.
         * Also, consider using Rigidbodies (GetComponent<Rigidbody>().velocity)
         * to attain movement automatic collision-detection.
         * https://docs.unity3d.com/ScriptReference/Rigidbody.html
         * Also also, remember to attain framerate-independence via Time.deltaTime
         * https://docs.unity3d.com/ScriptReference/Time-deltaTime.html 
         */
    }

    /* TODO: Deal with user-invoked usage of weapons and items */
    void ProcessAttacks()
    {

    }

//	void OnCollisionEnter(Collision coll) {
//		if (coll.gameObject.tag == "Enemy") {
//			receive_damage = true;
//			//do something here like decrease health? not sure how to put all
//			//the code together with these files, like difference between
//			//PlayerControl and PlayerController.
//			//print("dude you touched me");
//		}
//	}
}
