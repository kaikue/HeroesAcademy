using UnityEngine;
using System.Collections;

public class EnemySpawnTrigger : MonoBehaviour {

	public GameObject player;
	public GameObject enemy;

	void Start () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {

		if (other.gameObject == player)
			enemy.SetActive (true);

	}
	

}
