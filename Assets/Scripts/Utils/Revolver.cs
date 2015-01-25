using UnityEngine;
using System.Collections;

public class Revolver : MonoBehaviour {

	enum REVOLVE_AXIS {
		INVALID = 0,
		Y,
		X,
		Z,
		MAX
	}
	
	enum SCOPE {
		INVALID = 0,
		LOCAL,
		WORLD,
		MAX
	}

	[SerializeField]
	private SCOPE scope;
	[SerializeField]
	private REVOLVE_AXIS axis;
	
	[SerializeField]
	private float revolveSpeed;
	private Vector3 revolveAxis;
	
	[SerializeField]
	private bool randomRevolveSpeed;
	[SerializeField]
	private float minRevolveSpeed;
	[SerializeField]
	private float maxRevolveSpeed;

	public float GetRevolutionSpeed() {
		return this.revolveSpeed;
	}

	// Use this for initialization
	void Start () {
		switch(axis) {
		case REVOLVE_AXIS.X:
			revolveAxis = Vector3.right;
			break;
		case REVOLVE_AXIS.Y:
			revolveAxis = Vector3.up;
			break;
		case REVOLVE_AXIS.Z:
			revolveAxis = Vector3.forward;
			break;
		}
		if(scope == SCOPE.LOCAL)
			revolveAxis = this.transform.rotation * revolveAxis;
		if(randomRevolveSpeed)
			revolveSpeed = Random.Range(minRevolveSpeed, maxRevolveSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAround(this.transform.position, revolveAxis, Time.deltaTime * revolveSpeed);
	}
}
