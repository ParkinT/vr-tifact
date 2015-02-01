using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
	
	private GameObject Orb;
	private Vector3 [] gems;
	private Quaternion [] rotations;

	// Use this for initialization
	void Start () {
		Orb = GameObject.Find("ball");
		gems = new Vector3[4];
		rotations = new Quaternion[4];
		gems[0] = new Vector3(0.087645f, 0.10224f, 0); //earth gem
		rotations[0] = new Quaternion(0, 0, 0.7f, 0.7f);    //earth gem
		gems[1] = new Vector3(0.0009f, 0.09541f, 0.08574f); //water
		rotations[1] = new Quaternion(-0.5f, -0.5f, 0.5f, 0.5f);  //water
		gems[2] = new Vector3(-0.08877373f, 0.09849203f, 0.0004019737f);  //fire
		rotations[2] = new Quaternion(0, 0, 0.7f, -0.7f);  //fire
		gems[3] = new Vector3(0, 0.09804f, 0 ); //wind
		rotations[3] = new Quaternion(0, 0, 0.7f, 0.7f); //wind

	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.parent && gameObject.transform.parent.gameObject == Orb)  //this is a strange kludge
		{
			switch (this.name)  //lock gem into ball
			{
				case "earth" :
					transform.localPosition = gems[0];
					transform.localRotation = rotations[0];
					break;
				case "water" :
					transform.localPosition = gems[1];
					transform.localRotation = rotations[1];
					break;
				case "fire" :
				Debug.Log(this.name + " : " + this.transform.localRotation);
					transform.localPosition = gems[2];
					transform.localRotation = rotations[2];
					break;
				case "wind" :
					transform.localPosition = gems[2];
					transform.localRotation = rotations[2];
					break;
			}
		}
	}
}
