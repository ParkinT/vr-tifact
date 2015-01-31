using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	private GameObject Player;
	private GameObject Orb;
	private Vector3 carried = new Vector3(0.3f, 0, 0.7f);
	private bool _carrying;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		Orb = GameObject.Find ("ball");
		_carrying = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Click registered while pointed to Collider
	void OnMouseDown()
	{
		if (_carrying)  //ignore
			return; 
		this.transform.parent = Player.transform;
		this.transform.localPosition = carried;
		_carrying = this.transform.parent != Orb.transform;
	}
}
