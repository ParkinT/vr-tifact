using UnityEngine;
using System.Collections;

public class MouseCarrier : Carrier {
	
	private bool bLMBState = false, bPrevLMBState = false;

	private ColliderCollector colliderCollector = null;
	
	// Use this for initialization
	void Start () {
		this.colliderCollector = this.GetComponent<ColliderCollector>();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

		bPrevLMBState = bLMBState;
		bLMBState = Input.GetButton ("Fire1");
		
		if (bLMBState != bPrevLMBState) {
			// Drop gesture
			if ((!bLMBState && bPrevLMBState) && base.isHoldingObj)
			{
				print("Drop object");
				base.DropObject();
			}
			
			// Pick up gesture
			if (((bLMBState && !bPrevLMBState)) && !base.isHoldingObj)
			{
				GameObject pickup = null;
				foreach(Collider c in this.colliderCollector.managedColliders) {
					if(c.tag.Equals("ball")) { 
						pickup = c.gameObject;
						break;
					}
					else if (c.tag.Equals("gem")) {
						pickup = c.gameObject;
					}
				}
				if(null != pickup) {
					Carryable carryable = pickup.GetComponent<Carryable>();
					if(null != carryable){
						Debug.Log("Pickup object");
						base.PickupObject(carryable);
					}
				}
			}
		}
	}

	public override void DropObject ()
	{
		Collider collider = base._heldObject.collider;
		if (this.colliderCollector.managedColliders.Contains (collider)) {
			this.colliderCollector.managedColliders.Remove (collider);
		}
		base.DropObject ();
	}
}
