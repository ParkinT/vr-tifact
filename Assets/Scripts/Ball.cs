using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private bool ballAnimationStarted = false;
    private bool inPedestal = false;
    private GameObject pedestal;
    private Vector3 targetLocation = Vector3.zero;
    private const float MOVEMENT_SPEED = 0.5f;
    float xPos, yPos, zPos = 0;

	// Use this for initialization
	void Start () 
    {
        pedestal = GameObject.Find("Pedestal");
        targetLocation = new Vector3(pedestal.gameObject.transform.position.x, 
            pedestal.gameObject.transform.position.y + (pedestal.renderer.bounds.size.y / 2) + 
            (renderer.bounds.size.y / 2), pedestal.gameObject.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (/*gameObject.renderer.bounds.Intersects(pedestal.renderer.bounds) && */!ballAnimationStarted)
        {
            ballAnimationStarted = true;
            PlaceBall();
        }
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
            yield return null;
        }
        inPedestal = true;
    }

    void PlaceBall()
    {
        // Place the ball on the pedestal
        gameObject.rigidbody.isKinematic = true;
        StartCoroutine(AnimatePlacement());
    }
}
