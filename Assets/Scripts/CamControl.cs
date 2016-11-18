using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour {

	public GameObject player;

	public float leftBound;
	public float rightBound;
	public float upBound;
	public float downBound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//position camera to center on player
		float xPos = player.transform.position.x;
		xPos = Mathf.Clamp (xPos, leftBound, rightBound);
		float yPos = player.transform.position.y;
		yPos = Mathf.Clamp (yPos, downBound, upBound);
		float zPos = this.transform.position.z;
		Vector3 pos = new Vector3 (xPos, yPos, zPos);
		this.transform.position = pos;
	
	}
}
