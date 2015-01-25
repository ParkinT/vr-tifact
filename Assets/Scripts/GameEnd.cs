using UnityEngine;
using System.Collections;

public class GameEnd : Reveal {
	public GameObject PlayerControllerReference;

	// Update is called once per frame
	void Update () {
		if (is_active)
		{
			//disable player controls
			(PlayerControllerReference.GetComponent<CharacterController>()).enabled = false;
			//Start flying

		}
	}
}
