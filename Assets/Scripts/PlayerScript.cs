using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private Rigidbody2D rigidbody;
	private float speed = 0.1f;

	// Use this for initialization
	void Start () {
		rigidbody = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
		{
			rigidbody.MovePosition(rigidbody.position - new Vector2 (speed, 0));
		}
		if(Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow))
		{
			rigidbody.MovePosition(rigidbody.position + new Vector2 (speed, 0));
		}
	}
}
