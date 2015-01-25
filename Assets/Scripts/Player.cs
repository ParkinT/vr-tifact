using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    bool isAudioPlaying = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (!isAudioPlaying && 
            (Input.GetKeyDown(KeyCode.W)/* || Input.GetKeyDown(KeyCode.A) || 
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)*/))
        {
            isAudioPlaying = true;
            AudioController.Play("Footsteps");
        }
        else if (Input.GetKeyUp(KeyCode.W) /*&& Input.GetKeyUp(KeyCode.A) && 
            Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.D)*/)
        {
            isAudioPlaying = false;
            AudioController.Stop("Footsteps");
        }
	}
}
