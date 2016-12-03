using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	
	public float speed = 10;
	public float maxSpeed = 7;
	public bool flying = false;
	public bool burning = false;
	public int damage = 10;
	public int fortitude = 20;

	private Rigidbody2D rigidBody;
	private Transform player;

	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;

		if (flying) {
			rigidBody.gravityScale = 0;
		}
	}


	void Update () {
		if (player.position.x < gameObject.transform.position.x) {
			rigidBody.AddForce (new Vector2 (-speed, 0));
		} else if (player.position.x > gameObject.transform.position.x) {
			rigidBody.AddForce (new Vector2 (speed, 0));
		}

		if (flying) {
			if (player.position.y < gameObject.transform.position.y) {
				rigidBody.AddForce (new Vector2 (0, -speed));
			} else if (player.position.y > gameObject.transform.position.y) {
				rigidBody.AddForce (new Vector2 (0, speed));
			}
		}

		float currSpeed = rigidBody.velocity.x;
		currSpeed = Mathf.Clamp (currSpeed, -maxSpeed, maxSpeed);
		rigidBody.velocity = new Vector2 (currSpeed, rigidBody.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Couch") {
			if (collision.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude > fortitude) {
				Destroy (gameObject);
			} else if (burning) {
				Destroy (collision.gameObject);
			}
		}
		else if (collision.gameObject.tag == "Player") {
			player.GetComponent<PlayerScript>().hurt(damage);
		}
	}
}
