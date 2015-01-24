using UnityEngine;
using System.Collections;

/* Reveal - Move a group of objects through space in order to reveal them to the player/viewer
 * Created: Thom Parkin
 * -  This script will be attached to every object that is to be moved.
 * Changelog:
 * 
 */
public class Reveal : MonoBehaviour {

	public bool is_active;
	public Vector3 FinalLocation;
	public int MovementSpeed;

	private float _movementFactor = 10;
	private Vector3 _targetLocation;
	private float newX, newY, newZ;
	private const float LERP_TRANSLATION_FACTOR = 0.05f;

	// Use this for initialization
	void Start () {
		_movementFactor = (float) MovementSpeed * LERP_TRANSLATION_FACTOR;
		_targetLocation = new Vector3(FinalLocation.x, FinalLocation.y, FinalLocation.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (!is_active) //not yet active; ignore
			return;
		if (transform.position == FinalLocation)  //no more movement
			return;

		newX = Mathf.Lerp(transform.position.x, _targetLocation.x, Time.deltaTime * _movementFactor);
		newY = Mathf.Lerp(transform.position.y, _targetLocation.y, Time.deltaTime * _movementFactor);
		newZ = Mathf.Lerp(transform.position.z, _targetLocation.z, Time.deltaTime * _movementFactor);
		transform.position = updatePosition(newX, newY, newZ);
	}

	/// <summary>
	/// Updates the position based on X,Y,Z passed in
	/// </summary>
	/// <returns>A single "Transform.position" that can be used easily</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	private Vector3 updatePosition(float x, float y, float z)
	{
		return new Vector3(x, y, z);
	}

	/// <summary>
	/// Visual 'shudder' at the stop
	/// Once the object has reached its final location (within a tolerance), apply a small movement representing a shudder
	/// </summary>
	private Vector3 shudder()
	{
		return new Vector3(); //dummy - remove before flight
	}
}
