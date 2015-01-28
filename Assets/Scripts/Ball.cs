using UnityEngine;
using System.Collections;

public class Ball : Carryable
{
	[SerializeField]
	private Transform earthTransform;
	[SerializeField]
	private Transform waterTransform;
	[SerializeField]
	private Transform fireTransform;
	[SerializeField]
	private Transform windTransform;
	
	[SerializeField]
	private Reveal[] earthGemReveals;
	[SerializeField]
	private Reveal[] waterGemReveals;
	[SerializeField]
	private Reveal[] fireGemReveals;
	[SerializeField]
	private Reveal[] windGemReveals;

	public override void Dropped ()
	{
		base.Dropped ();
		AudioController.Play("BallPlaceDown");
	} 

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "gem") {
			Transform newParent = this.transform;
			int revealID = -1;
			switch(other.name) {
			case "earth":
				newParent = earthTransform;
				revealID = 0;
				break;
			case "water":
				newParent = waterTransform;
				revealID = 1;
				break;
			case "fire":
				newParent = fireTransform;
				revealID = 2;
				break;
			case "wind":
				newParent = windTransform;
				revealID = 3;
				break;
			}
			other.transform.parent = newParent;
			other.transform.localPosition = Vector3.zero;
			other.transform.localScale = Vector3.one;
			other.transform.localRotation = Quaternion.identity;
			other.enabled = false;
			this.RunReveal(revealID);
			
			Carryable carryable = other.GetComponent<Carryable>();
			if(carryable != null) {
				carryable.ForceDrop();
				carryable.bCanBeCarried = false;
			}
		}
	}
	
	public void RunReveal(int revealID) {
		Reveal[] toReveal;
		switch (revealID) {
		case 0:
			toReveal = earthGemReveals;
			break;
		case 1:
			toReveal = waterGemReveals;
			break;
		case 2:
			toReveal = fireGemReveals;
			break;
		case 3:
			toReveal = windGemReveals;
			break;
		default:
			Debug.LogError("Reveal ID not a proper id.");
			return;
		}
		for (int i = 0; i < toReveal.Length; ++i) {
			toReveal[i].Play();
		}
	}
}
