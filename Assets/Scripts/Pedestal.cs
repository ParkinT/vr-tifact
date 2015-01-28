using UnityEngine;
using System.Collections;

public class Pedestal : MonoBehaviour {

	[SerializeField]
	private Transform ballSitPosition = null;
	[SerializeField]
	private float moveSpeed = 0.5f;

	IEnumerator AnimatePlacement (GameObject movingObject)
	{
		// Move the ball toward an object until it reaches the desired position
		while (movingObject.transform.position != ballSitPosition.position) 
		{
			movingObject.transform.position = Vector3.Lerp(movingObject.transform.position, ballSitPosition.position, Time.deltaTime * moveSpeed);
			yield return null;
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "ball") {
			
			Carryable carryable = other.GetComponent<Carryable>();
			if(carryable != null) {
				Debug.Log("Ball at pedestal");
				carryable.ForceDrop();
				carryable.bCanBeCarried = false;
			}

			StartCoroutine(AnimatePlacement(other.gameObject));
		}
	}
}