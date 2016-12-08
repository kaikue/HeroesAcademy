using UnityEngine;
using System.Collections;

public class Biohazard : MonoBehaviour {

	public int damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {

		if (other.tag == "Player")
			other.GetComponent<PlayerScript> ().hurt (damage, false);

	}

}
