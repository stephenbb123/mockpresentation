using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour 
{
	public Vector3 rotation;

	void Update () 
	{
		//Rotate the object.
		transform.Rotate(rotation * Time.deltaTime);
	}
}
