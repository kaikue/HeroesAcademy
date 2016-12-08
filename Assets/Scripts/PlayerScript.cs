using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

	private Animator anim;

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
		anim = gameObject.GetComponent<Animator> ();
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

		//check below feet & to sides to see if the player is on the ground
		bool onGround = false;
		float[] footPoses = new float[] {
			gameObject.transform.position.x - playerWidth,
//			gameObject.transform.position.x,
			gameObject.transform.position.x + playerWidth
		};
		Vector2[] footEnds = new Vector2[] {
			new Vector2 (footPoses [0], this.transform.position.y - groundHeight),
			new Vector2 (footPoses [1], this.transform.position.y - groundHeight),
		};
		onGround = Physics2D.Linecast (footEnds [0], footEnds [1]);
//		foreach(float footPos in footPoses) {
//			onGround |= Physics2D.OverlapPoint (new Vector2(footPos, gameObject.transform.position.y) + Vector2.down * groundHeight) != null;
//		}

		if(up && onGround) {
			rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
		}

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
				if (health < timeDiff) {
					timeDiff = health - 1;
				}
				hurt (timeDiff, false);
			}
		}

		//animate
		if (left)
			anim.SetBool ("faceLeft", true);
		else if (right)
			anim.SetBool ("faceLeft", false);
		if (onGround && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) 
			|| Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
			anim.SetBool ("walking", true);
		else
			anim.SetBool ("walking", false);

	}

	public void hurt(int damage, bool enemyInduced) {
		bool enemyToLeft = false;
		if (enemyInduced) {
			//find closest enemy, which is the attacker
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			float dist = 1000000000f;
			GameObject attacker = null;
			foreach (GameObject enemy in enemies) {
				Vector2 enemyPos = enemy.transform.position;
				Vector2 pcPos = this.transform.position;
				Vector2 diff = enemyPos - pcPos;
				float d = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
				if (d < dist) {
					dist = d;
					attacker = enemy;
				}
			}
			if (attacker.transform.position.x < this.transform.position.x)
				enemyToLeft = true;
			else
				enemyToLeft = false;
		}
		Vector3 couchpos = gameObject.transform.position;
		couchpos.z += 0.1f; //keep player sprite in front of couch sprite
		Vector3 offset = (dir == LEFT ? Vector3.left : Vector3.right) * 2;
		if (enemyInduced) {
			if (enemyToLeft)
				offset = Vector3.right * 2;
			else
				offset = Vector3.left * 2;
		}
		couchpos += offset;
		GameObject spawned = ((Transform)Instantiate(couch, couchpos, Quaternion.identity)).gameObject;
		Vector2 force = (dir == LEFT ? Vector2.left : Vector2.right) * 500 * damage + Vector2.up * 200 * damage;
		if (enemyInduced) {
			if (enemyToLeft)
				force = Vector2.right * 500 * damage + Vector2.up * 200 * damage;
			else
				force = Vector2.left * 500 * damage + Vector2.up * 200 * damage;
		}
		spawned.GetComponent<Rigidbody2D> ().AddForce (force);
		spawned.GetComponent<SpriteRenderer> ().sprite = couches [Random.Range (0, couches.Length)];
		health -= damage;
		updateHealthText();
	}

	void updateHealthText() {
		healthText.text = "Health: " + health.ToString ();
		if (health <= 0.0f)
			SceneManager.LoadScene ("ObstacleCourse");
	}
}
