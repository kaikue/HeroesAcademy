using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public Transform couch;
	public Text healthText;
	private int health;

	private const bool LEFT = false;
	private const bool RIGHT = !LEFT;

	private Rigidbody2D rigidBody;
	private float speed = 20;
	private float jumpHeight = 7;
	private float maxSpeed = 8;
	private float groundHeight;
	private float playerWidth;
	private bool dir;
	private float pressedTime;

	public Sprite[] couches;

	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		groundHeight = gameObject.GetComponent<Collider2D>().bounds.size.y / 2 + 0.05f;
		playerWidth = gameObject.GetComponent<Collider2D> ().bounds.size.x / 2 - 0.02f;
		dir = LEFT;
		health = 100;
		updateHealthText();
	}
	
	void Update () {

		//jump
		bool up = Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow);
		//this currently only checks directly under the player's center, need to check all positions to the sides too
		bool onGround = Physics2D.OverlapPoint (new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + Vector2.down * groundHeight) != null;
		if (!onGround)
			onGround = Physics2D.OverlapPoint (new Vector2(gameObject.transform.position.x + playerWidth, gameObject.transform.position.y) + Vector2.down * groundHeight) != null;
		if (!onGround)
			onGround = Physics2D.OverlapPoint (new Vector2(gameObject.transform.position.x - playerWidth, gameObject.transform.position.y) + Vector2.down * groundHeight) != null;
		if(up && onGround)
		{
			rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
		}

		//left/right movement
		bool left = Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow);
		bool right = Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow);
		if (left && !right) {
//			if (onGround) {
				rigidBody.AddForce (new Vector2 (-speed, 0));
				dir = LEFT;
//			}
		} else if (right && !left) {
//			if (onGround) {
				rigidBody.AddForce (new Vector2 (speed, 0));
				dir = RIGHT;
//			} 
		} else {
			//slow to a stop
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x * 0.75f, rigidBody.velocity.y);
		}

		//fix to not exceed max speed

		float currSpeed = rigidBody.velocity.x;
		currSpeed = Mathf.Clamp (currSpeed, -maxSpeed, maxSpeed);
		rigidBody.velocity = new Vector2 (currSpeed, rigidBody.velocity.y);

		//hold space to spawn couch
		if (Input.GetKeyDown(KeyCode.Space)) {
			pressedTime = Time.time;
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (health > 1) {
				int timeDiff = (int)((Time.time - pressedTime) * 10) + 1;
				//adjust couch trajectory if health is too low
				if (health < timeDiff)
					timeDiff = health - 1;
				print (timeDiff);
				hurt (timeDiff);
			}
		}
	}

	void hurt(int damage) {
		Vector3 couchpos = gameObject.transform.position;
		couchpos.z += 0.1f; //keep player sprite in front of couch sprite
		Vector3 offset = (dir == LEFT ? Vector3.left : Vector3.right) * 2;
		couchpos += offset;
		GameObject spawned = ((Transform)Instantiate(couch, couchpos, Quaternion.identity)).gameObject;
		Vector2 force = (dir == LEFT ? Vector2.left : Vector2.right) * 50 * damage + Vector2.up * 20 * damage;
		spawned.GetComponent<Rigidbody2D> ().AddForce (force);
		spawned.GetComponent<SpriteRenderer> ().sprite = couches [Random.Range (0, couches.Length)];
		health -= damage;
		updateHealthText();
	}

	void updateHealthText() {
		healthText.text = "Health: " + health.ToString ();
	}
}
