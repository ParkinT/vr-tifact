using UnityEngine;
using System.Collections;

public class Contact : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("Yo, {0}", other.gameObject.name));
    }
}
