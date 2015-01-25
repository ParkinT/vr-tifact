using UnityEngine;
using System.Collections;

public class TestManager : MonoBehaviour {

    //private Transform[] revealableObjects = GameObject.Find("RevealableObjects").GetComponentInChildren<Transform>();
    private bool hasBeenRevealed = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	 if (Input.GetKeyDown(KeyCode.P) && !hasBeenRevealed)
     {
         foreach (Transform child in transform)
         {
             hasBeenRevealed = true;
             //if ((obj.GetComponent("Reveal") as Reveal) != null)
             {
                 child.GetComponent<Reveal>().is_active = true;
             }
         }
     }
	}
}
