using UnityEngine;
using System.Collections;

public class PosClamp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float x = Mathf.Clamp (this.transform.position.x, 23f, Mathf.Infinity);
		this.transform.position = new Vector2 (x, this.transform.position.y);
	
	}
}
