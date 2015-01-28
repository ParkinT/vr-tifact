using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    bool isAudioPlaying = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Handle footstep audio
        if (IsMoving() && !isAudioPlaying)
        {
            StartFootstepAudio();
        }
        else if (!IsMoving() && isAudioPlaying)
        {
            StopFootstepAudio();
        }
	}

    private void StartFootstepAudio()
    {
        isAudioPlaying = true;
        AudioController.Play("Footsteps");
    }

    private void StopFootstepAudio()
    {
        isAudioPlaying = false;
        AudioController.Stop("Footsteps");
    }

    public bool IsMoving()
    {
        if (GetComponent<CharacterController>().velocity == Vector3.zero)
            return false;
        else
            return true;
    }
}
