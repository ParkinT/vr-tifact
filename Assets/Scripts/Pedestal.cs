using UnityEngine;
using System.Collections;

public class Pedestal : MonoBehaviour {

	[SerializeField]
	private Reveal[] earthGemReveals;
	[SerializeField]
	private Reveal[] waterGemReveals;
	[SerializeField]
	private Reveal[] fireGemReveals;
	[SerializeField]
	private Reveal[] windGemReveals;

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

	void OnTriggerEnter(Collider other) {
		switch (other.gameObject.name) {
		case "earth":
			this.RunReveal(0);
			break;
		case "water":
			this.RunReveal(1);
			break;
		case "fire":
			this.RunReveal(2);
			break;
		case "wind":
			this.RunReveal(3);
			break;
		}
	}
}
