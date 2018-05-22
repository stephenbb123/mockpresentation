//DetailedMeshCollider.cs - Bumpkin Labs
//The Detailed Mesh Collider is a component that works with a mesh collider to peform an accurate mesh to mesh overlap test.

//Setup - this system uses a collision layer to perform raycasts on only one mesh.  This needs to be setup in the Unity project and match
//the scanLayer const in this class.

//Use - Attach to a game object and assign a mesh.  When testing for a collision call UpdatePosition on BOTH DetailedMeshColliders then 
//call CollisionTest on one collider - passing in the other.  
//UpdatePosition is an expensive call which is why it needs to be specified.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Blocks/Detailed Mesh Collider")]
	[RequireComponent(typeof(MeshCollider))]
	public class DetailedMeshCollider : MonoBehaviour 
	{
		private List<MeshTriangle> tris;
		private Bounds bounds;
		private Vector3 initialScale;
		private bool shrunk = false;

		private const int scanLayer = 8;
		private MeshCollider meshCollider;
		private bool initialized = false;

		public void EnableMeshCollider()
		{
			if (meshCollider == null)
				meshCollider = GetComponent<MeshCollider>();

			meshCollider.enabled = true;
		}

		public void DisableMeshCollider()
		{
			if (meshCollider == null)
				meshCollider = GetComponent<MeshCollider>();

			meshCollider.enabled = false;
		}

		public void Shrink()
		{
			if (shrunk == false)
			{
				initialScale = transform.localScale;
				transform.localScale += new Vector3(-0.1f, -0.1f, -0.1f);
				
				UpdatePosition();

				shrunk = true;
			}
		}

		public void ResetScale()
		{
			if (shrunk)
			{
				transform.localScale = initialScale;
				UpdatePosition();

				shrunk = false;
			}
		}

		void Initialize()
		{
			if (!initialized)
			{
				tris = new List<MeshTriangle>();
				meshCollider = GetComponent<MeshCollider>();
				Mesh mesh = meshCollider.sharedMesh;
				
				int index = 0;
				
				while (index < mesh.triangles.Length)
				{
					MeshTriangle t = new MeshTriangle(
						mesh.vertices[mesh.triangles[index]],
						mesh.vertices[mesh.triangles[index+1]],
						mesh.vertices[mesh.triangles[index+2]], 
						transform);
					
					tris.Add(t);
					
					index += 3;
				}

				initialized = true;
			}
		}

		void Awake()
		{
			Initialize();

			UpdatePosition();
		}

		public void UpdatePosition()
		{
			Initialize();

			bool firstPass = true;
			float minX = 0, minY = 0, minZ = 0, maxX = 0, maxY = 0, maxZ = 0;

			foreach (MeshTriangle mt in tris)
			{
				mt.UpdateVertexPositions();

				if (firstPass)
				{
					firstPass = false;

					minX = mt.MinX;
					minY = mt.MinY;
					minZ = mt.MinZ;

					maxX = mt.MaxX;
					maxY = mt.MaxY;
					maxZ = mt.MaxZ;
				}
				else
				{
					minX = Mathf.Min(minX, mt.MinX);
					minY = Mathf.Min(minY, mt.MinY);
					minZ = Mathf.Min(minZ, mt.MinZ);

					maxX = Mathf.Max(maxX, mt.MaxX);
					maxY = Mathf.Max(maxY, mt.MaxY);
					maxZ = Mathf.Max(maxZ, mt.MaxZ);
				}
			}

			Vector3 size = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
			Vector3 ctr = new Vector3(minX + (size.x/2f), minY + (size.y/2f), minZ + (size.z/2f));

			bounds = new Bounds(ctr, size);
		}

		public Bounds Bounds
		{
			get
			{
				return bounds;
			}
		}

		/// <summary>
		/// Performs a collision test between this and another collider.  UpdatePositions should have been called PRIOR to this.
		/// </summary>
		/// <returns><c>true</c>, if there is a collision, <c>false</c> otherwise.</returns>
		public bool CollisionTest(DetailedMeshCollider other)
		{
			if (Bounds.Intersects(other.Bounds))
			{
				if (Bounds.Contains(other.Bounds.max))
				{
					return MeshCollisionTest(other);
				}

				if (other.Bounds.Contains(Bounds.max))
				{
					return MeshCollisionTest(other);
				}
					
				return MeshCollisionTest(other);
			}

			return false;
		}

		private bool MeshCollisionTest(DetailedMeshCollider other)
		{

			int scanMask = 1 << scanLayer;
			int currentLayer = other.gameObject.layer;

			other.gameObject.layer = scanLayer;

			foreach (MeshTriangle mt in tris)
			{
				if (mt.Raycast(scanMask))
				{
					other.gameObject.layer = currentLayer;
					return true;
				}
			}

			other.gameObject.layer = currentLayer;
			return false;
		}

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;

			if (tris != null)
			{
				foreach (MeshTriangle t in tris)
				{
					t.DrawDebug();
				}
			}

			Gizmos.color = Color.cyan;

			//if (bounds != null)
				Gizmos.DrawWireCube(bounds.center, bounds.size);
		}
	}
}
