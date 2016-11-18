using UnityEngine;
using System.Collections;

public class walk : MonoBehaviour {

	// Use this for initialization
	public Animator anim;
	private bool walkFlag;
	private bool backFlag;
	public GameObject character;
	private bool right;
	private bool left;
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("w", walkFlag);
		anim.SetBool ("s", backFlag);
		if (Input.GetKeyDown (KeyCode.W)) {
			print ("HELLO");
			walkFlag = true;
			backFlag = false;
		

		} else if (Input.GetKeyDown (KeyCode.S)) {
			backFlag = true;
			walkFlag = false;
		} else if (Input.GetKeyDown (KeyCode.D)) {
			right = true;
		} else if (Input.GetKeyDown (KeyCode.A)) {
			left = true;
		}

		if (Input.GetKeyUp (KeyCode.W))
			walkFlag = false;

		if (Input.GetKeyUp (KeyCode.S))
			backFlag = false;
		
		if (Input.GetKeyUp (KeyCode.D))
			right = false;
		
		if (Input.GetKeyUp (KeyCode.A))
			left = false;

		if (right) {
			character.transform.Rotate(Vector3.up * Time.deltaTime*100);

		}
		if (left) {
			character.transform.Rotate(Vector3.up * -Time.deltaTime*100);

		}
	}
}
