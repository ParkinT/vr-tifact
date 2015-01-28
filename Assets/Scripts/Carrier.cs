using UnityEngine;
using System.Collections;

public class Carrier : MonoBehaviour {
	
	protected bool isHoldingObj = false;
	protected Carryable _heldObject = null;

	public virtual void Update() {
		if (isHoldingObj && _heldObject != null)
		{
			_heldObject.transform.position = this.gameObject.transform.position;
		}
	}

	public virtual void PickupObject(Carryable targetObject)
	{
		_heldObject = targetObject;
		_heldObject.transform.position = this.gameObject.transform.position;
		_heldObject.PickedUp (this);
		isHoldingObj = true;

		//_heldObject.rigidbody.isKinematic = true;
		print("We picked up an object");
	}
	
	public virtual void DropObject()
	{
		_heldObject.Dropped ();
		_heldObject = null;

		isHoldingObj = false;
		//_heldObject.rigidbody.isKinematic = false;
		
		print("Object dropped");
	}
}
