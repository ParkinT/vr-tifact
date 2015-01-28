using UnityEngine;
using System.Collections;

public class Carryable : MonoBehaviour {

	private bool bIsBeingCarried = false;
	private Carrier carrier = null;

	public bool bCanBeCarried = true;

	public virtual void PickedUp(Carrier carrier) {
		this.carrier = carrier;
		this.bIsBeingCarried = true;
		if (!bCanBeCarried) {
			this.ForceDrop();
		}
	}
	
	public virtual void Dropped() {
		this.carrier = null;
		this.bIsBeingCarried = false;
	}

	public virtual void ForceDrop() {
		if (null != this.carrier) {
			this.carrier.DropObject();
		}
		this.carrier = null;
		this.bIsBeingCarried = false;
	}
}
