using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	public Vector3 targetLocation = Vector3.zero;
	private bool ballAnimationStarted = false;
    private bool inPedestal = false;
    private GameObject pedestal;
    private const float MOVEMENT_SPEED = 1.0f;
    float xPos, yPos, zPos = 0;

	bool bShouldUpdate = false;

	// Use this for initialization
	void Start () 
    {
		pedestal = GameObject.Find("BallSitPos");
		targetLocation = pedestal.transform.position;
//        targetLocation = new Vector3(pedestal.gameObject.transform.position.x, 
//            pedestal.gameObject.transform.position.y + (pedestal.renderer.bounds.size.y / 2) + 
//            (renderer.bounds.size.y / 2), pedestal.gameObject.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (inPedestal && this.transform.parent)
			gameObject.transform.parent = null;
	}

    IEnumerator AnimatePlacement ()
    {
        // Move the ball toward an object until it reaches the desired position
        while (transform.position.x != targetLocation.x || 
            transform.position.y != targetLocation.y || 
            transform.position.z != targetLocation.z) 
        {
            xPos = Mathf.Lerp(transform.position.x, targetLocation.x, Time.deltaTime * MOVEMENT_SPEED);
            yPos = Mathf.Lerp(transform.position.y, targetLocation.y, Time.deltaTime * MOVEMENT_SPEED);
            zPos = Mathf.Lerp(transform.position.z, targetLocation.z, Time.deltaTime * MOVEMENT_SPEED);
            transform.position = new Vector3(xPos, yPos, zPos);
            yield return true;
        }
        inPedestal = true;
    }

    void PlaceBall()
    {
        // Place the ball on the pedestal
        gameObject.rigidbody.isKinematic = true;
        StartCoroutine(AnimatePlacement());
		inPedestal = true;
    }

	public void OnTriggerEnter(Collider other) {
		if ((other.gameObject.tag.ToLower()).Contains("gem")) { //we need to use a script ON THE GEM to set its localPosition because there is some timing issue
			other.gameObject.transform.parent = transform;
			other.gameObject.transform.localScale = new Vector3(0.39f, 0.39f, 0.39f);
			other.gameObject.rigidbody.isKinematic = false;
			//play a sound here
		}
		if (other.gameObject.tag == "pedestal") {
			PlaceBall();
		}
	}
}
