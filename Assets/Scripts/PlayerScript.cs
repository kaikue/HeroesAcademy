using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private Rigidbody2D rigidBody;
	private float speed = 20;
	private float jumpHeight = 5;
	private float groundHeight;

	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		groundHeight = gameObject.GetComponent<PolygonCollider2D>().bounds.size.y / 2 + 0.05f;
	}
	
	void Update () {
		//left/right movement
		bool left = Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow);
		bool right = Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow);
		if (left && !right) {
			rigidBody.AddForce (new Vector2 (-speed, 0));
		} else if (right && !left) {
			rigidBody.AddForce (new Vector2 (speed, 0));
		} else {
			//slow to a stop
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x * 0.75f, rigidBody.velocity.y);
		}

		//jump
		bool up = Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow);
		RaycastHit2D[] collisions = Physics2D.RaycastAll(gameObject.transform.position, Vector2.down, groundHeight);
		bool onGround = collisions.Length > 1; //TODO improve
		if(up && onGround)
		{
			//TODO sometimes this applies twice for some reason
			rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
		}
	}
}
