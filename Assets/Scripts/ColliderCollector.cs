using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderCollector : MonoBehaviour {
	
	public List<Collider> managedColliders = new List<Collider>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		managedColliders.Add (other);
	}
	
	void OnTriggerExit(Collider other)
	{
		if (managedColliders.Contains (other)) {
			managedColliders.Remove(other);
		}
	}
}
