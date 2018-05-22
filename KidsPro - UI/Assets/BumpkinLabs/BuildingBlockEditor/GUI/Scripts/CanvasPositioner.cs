using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BumpkinLabs.IGE
{
	[ExecuteInEditMode]
	public class CanvasPositioner : MonoBehaviour 
	{
		public Vector3 position;

		// Update is called once per frame
		void Update () 
		{
			Vector3 pos = transform.parent.position;

			pos += Vector3.right * position.x;
			pos += Vector3.up * position.y;
			pos += Vector3.forward * position.z;

			transform.position = pos;
			transform.forward = -Vector3.up;
		}
	}
}
