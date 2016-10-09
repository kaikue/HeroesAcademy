using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public Transform couch;

	private const bool LEFT = false;
	private const bool RIGHT = !LEFT;

	private Rigidbody2D rigidBody;
	private float speed = 20;
	private float jumpHeight = 5;
	private float groundHeight;
	private bool dir;

	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		groundHeight = gameObject.GetComponent<PolygonCollider2D>().bounds.size.y / 2 + 0.05f;
		dir = RIGHT;
	}
	
	void Update () {
		//left/right movement
		bool left = Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow);
		bool right = Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow);
		if (left && !right) {
			rigidBody.AddForce (new Vector2 (-speed, 0));
			dir = LEFT;
		} else if (right && !left) {
			rigidBody.AddForce (new Vector2 (speed, 0));
			dir = RIGHT;
		} else {
			//slow to a stop
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x * 0.75f, rigidBody.velocity.y);
		}

		//jump
		bool up = Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow);
		//this currently only checks directly under the player's center, need to check all positions to the sides too
		bool onGround = Physics2D.OverlapPoint (new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + Vector2.down * groundHeight) != null;
		if(up && onGround)
		{
			rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
		}

		//couch
		bool space = Input.GetKeyDown(KeyCode.Space);
		if (space) {
			Instantiate(couch, gameObject.transform.position, Quaternion.identity);
		}
	}
}
