using UnityEngine;
using System.Collections;

public class OrbitalCamera : MonoBehaviour 
{
	public float behind = -30;
	public float above = 30f;
	
	private static OrbitalCamera instance;
	
	private Transform followObject;
	private GameObject lockObject;
	private GameObject gravityObject;
	private float verticalPos = 0;
	private float horizontalPos = 0;
	
	private float currentRotation = 0;
	
	private bool affectGravity = true;
	
	/// <summary>
	/// Gets the instance of the orbital camera.
	/// </summary>
	public static OrbitalCamera Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<OrbitalCamera>();
			}
			
			return instance;
		}
	}
	
	/// <summary>
	/// Sets the object the camera will follow and control
	/// </summary>
	public void SetFollowObject(Transform t)
	{
		followObject = t;
		
		if (lockObject == null)
		{
			lockObject = new GameObject("Lock Object");
			lockObject.transform.rotation = Quaternion.identity;
		}
		
		lockObject.transform.position = followObject.position;
	}
	
	/// <summary>
	/// Stops the affecting gravity.
	/// </summary>
	public void StopAffectingGravity()
	{
		affectGravity = false;
		Physics.gravity = Vector3.zero;
		
		followObject.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		followObject.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}
	
	void FixedUpdate()
	{
		if (followObject != null) //Check we're following an object
		{
			if (gravityObject == null) //Ensure there is a gravity object
				gravityObject = new GameObject("Gravity Object");
				
			horizontalPos = Mathf.MoveTowards(horizontalPos, Input.GetAxis("Horizontal"), Time.deltaTime * 5f);
			verticalPos = Mathf.MoveTowards (verticalPos, Input.GetAxis("Vertical"), Time.deltaTime * 5f);
			
			//Work out the rotation of the gravity object
			Vector3 g = lockObject.transform.rotation.eulerAngles;
			g.x = -verticalPos * 30f;
			g.z = horizontalPos * 30f;
			
			gravityObject.transform.rotation = Quaternion.Euler(g);
			
			//Apply the rotation to the physics engine.
			if (affectGravity)
				Physics.gravity = gravityObject.transform.up * -9.81f;
		}
	}
	
	void LateUpdate()
	{
		if (followObject != null)
		{
			//Set the position of the lock object (following the follow object)
			lockObject.transform.position = followObject.position;
			
			//Set the rotation
			currentRotation = Mathf.Repeat(currentRotation + Input.GetAxis("Mouse X"), 360);
			
			lockObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
			
			//Create an offset (desired position)
			Vector3 pos = lockObject.transform.position;
			pos += lockObject.transform.forward * behind;
			pos += lockObject.transform.up * above;
			
			transform.position = pos;
			
			
			//Look at the ball.
			transform.LookAt(followObject.position);
			
			transform.Rotate(new Vector3(-verticalPos * 5f, 0, horizontalPos * 5f));
		}
	}	
}
