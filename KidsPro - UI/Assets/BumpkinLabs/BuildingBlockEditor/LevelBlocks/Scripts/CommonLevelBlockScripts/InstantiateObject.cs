using UnityEngine;
using System.Collections;

public class InstantiateObject : MonoBehaviour 
{
	public GameObject objectToCreate;
	public Vector3 offset = Vector3.zero;

	void Start()
	{
		if (objectToCreate != null)
		{
			Vector3 pos = transform.position + offset;
			Quaternion rot = transform.rotation;

			Instantiate(objectToCreate, pos, rot);
		}
		else
		{
			Debug.LogWarning("InstantiateObject called with no object");
		}
	}
}
