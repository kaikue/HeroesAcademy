using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	
	public float speed = 20;
	public bool flying = false;
	public bool burning = false;

	private Rigidbody2D rigidBody;
	private Transform player;

	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}


	void Update () {
		if (player.position.x < gameObject.transform.position.x) {
			rigidBody.AddForce (new Vector2 (-speed, 0));
		} else if (player.position.x > gameObject.transform.position.x) {
			rigidBody.AddForce (new Vector2 (speed, 0));
		} else {
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x * 0.75f, rigidBody.velocity.y);
		}


	}
}
