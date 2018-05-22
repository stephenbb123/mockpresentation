using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	public class MeshTriangle 
	{
		private Vector3 vertex1;
		private Vector3 vertex2;
		private Vector3 vertex3;

		private Vector3 v1;
		private Vector3 v2;
		private Vector3 v3;

		private Transform transform;

		/// <summary>
		/// Creates a new mesh triangle object
		/// </summary>
		public MeshTriangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Transform t)
		{
			this.vertex1 = vertex1;
			this.vertex2 = vertex2;
			this.vertex3 = vertex3;

			v1 = this.vertex1;
			v2 = this.vertex2;
			v3 = this.vertex3;

			this.transform = t;

			UpdateVertexPositions();
		}

		/// <summary>
		/// Updates the vertex positions to reflect the true position of the mesh
		/// </summary>
		public void UpdateVertexPositions()
		{
			v1 = UpdateVertexPosition(vertex1);
			v2 = UpdateVertexPosition(vertex2);
			v3 = UpdateVertexPosition(vertex3);
		}

		private Vector3 UpdateVertexPosition(Vector3 iv)
		{
			Vector3 rv = iv;
			rv.x *= transform.lossyScale.x;
			rv.y *= transform.lossyScale.y;
			rv.z *= transform.lossyScale.z;

			rv = Quaternion.Euler(transform.rotation.eulerAngles) * rv;
			rv += transform.position;

			return rv;
		}
		
		public Vector3 V1
		{
			get
			{
				return v1;
			}
		}

		public Vector3 V2
		{
			get
			{
				return v2;
			}
		}

		public Vector3 V3
		{
			get
			{
				return v3;
			}
		}

		public float MinX
		{
			get
			{
				return Mathf.Min(V1.x, V2.x, V3.x);
			}
		}

		public float MinY
		{
			get
			{
				return Mathf.Min(V1.y, V2.y, V3.y);
			}
		}

		public float MinZ
		{
			get
			{
				return Mathf.Min(V1.z, V2.z, V3.z);
			}
		}

		public float MaxX
		{
			get
			{
				return Mathf.Max(V1.x, V2.x, V3.x);
			}
		}

		public float MaxY
		{
			get
			{
				return Mathf.Max(V1.y, V2.y, V3.y);
			}
		}

		public float MaxZ
		{
			get
			{
				return Mathf.Max(V1.z, V2.z, V3.z);
			}
		}

		public bool Raycast(int layerMask)
		{
			if (Physics.Raycast(new Ray(V1, V2 - V1), Vector3.Distance(V1, V2), layerMask))
				return true;

			if (Physics.Raycast(new Ray(V2, V3 - V2), Vector3.Distance(V2, V3), layerMask))
				return true;

			if (Physics.Raycast(new Ray(V3, V1 - V3), Vector3.Distance(V3, V1), layerMask))
				return true;

			return false;
		}

		public void DrawDebug()
		{
			Gizmos.DrawLine(V1, V2);
			Gizmos.DrawLine(V2, V3);
			Gizmos.DrawLine(V3, V1);
		}
	}
}
