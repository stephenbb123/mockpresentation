using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Scene/Camera Controller")]
	[RequireComponent(typeof(Camera))]
	public class CameraController : MonoBehaviour 
	{
		private GameObject centreObject;
		private GameObject offsetObject;
		public Vector3 rotationAngle;
        private float curDist = 0;
        private float lastDist = 0;
        private float distance = 15;
        private float pinchSpeed = 0;
        private float minimumDistance = 5;
        private float maximumDistance = 100;
        Touch touchA = new Touch();
        Touch touchB = new Touch();
        Vector3 touchToWorldA = new Vector3();
        Vector3 touchToWorldB = new Vector3();
        Vector3 center = new Vector3();
        float rotationSpeed = 10f;
        float rotation = 0;


        public Vector3 CameraPosition
		{
			get { return centreObject.transform.position; }
			set { centreObject.transform.position = value; }
		}
		
		public Vector3 CameraRotation
		{
			get { return rotationAngle; }
			set { rotationAngle = value; }
		}
		
		public float CameraDistance
		{
			get { return Vector3.Distance(offsetObject.transform.position, centreObject.transform.position);}
			set
			{
				Vector3 dir = (offsetObject.transform.position - centreObject.transform.position).normalized;
				
				offsetObject.transform.position = centreObject.transform.position + value * dir;
			}
		}
		
		void Awake()
		{
			//Create the centre object that forms the focal point of the camera.
			centreObject = new GameObject("Camera Centre Object");
			centreObject.transform.position = Vector3.zero;

			//Create the offset object that rotates around the focal point and parent it to the focal point.
			offsetObject = new GameObject("Camera Offset Object");
			offsetObject.transform.position = centreObject.transform.position + new Vector3(10, 0, 0);
			offsetObject.transform.parent = centreObject.transform;
		}

		void Update()
		{
            if (Input.GetMouseButton(0))
			{
				if (Input.GetKey(KeyCode.LeftAlt))
				{
					centreObject.transform.Translate(new Vector3(0, -Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X")), Space.Self);
				}
				else
				{
                    rotationAngle.y = Mathf.Repeat(rotationAngle.y + Input.GetAxis("Mouse X") * 5f, 360);
                    rotationAngle.z = Mathf.Repeat(rotationAngle.z - Input.GetAxis("Mouse Y") * 5f, 360);
                    
                }
			}
           
    
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                Vector3 dir = (offsetObject.transform.position - centreObject.transform.position).normalized;

                float dist = Vector3.Distance(offsetObject.transform.position, centreObject.transform.position);

                dist -= (scroll * 5f);

                offsetObject.transform.position = centreObject.transform.position + (dist * dir);
            }
}

		void LateUpdate()
		{
			centreObject.transform.rotation = Quaternion.Euler(rotationAngle);

			//Set our position to the offset object and look at the centre object.
			transform.position = offsetObject.transform.position;
			transform.LookAt(centreObject.transform);
		}
	}
}
