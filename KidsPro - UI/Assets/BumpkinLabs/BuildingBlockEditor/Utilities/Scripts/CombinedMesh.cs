using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public class CombinedMesh 
	{
		private List<Vector3> verts;
		private List<Vector3> normals;
		private List<Vector2> uvs;
		private List<int> tris;
		private int triOffset = 0;

		private Vector3 position;

		public CombinedMesh(Vector3 position)
		{
			this.position = position;

			verts = new List<Vector3>();
			normals = new List<Vector3>();
			uvs = new List<Vector2>();
			tris = new List<int>();
			triOffset = 0;
		}

		public void CreateMesh(Mesh m, bool recalculateNormals)
		{
			m.Clear();

			m.vertices = verts.ToArray();
			m.uv = uvs.ToArray();
			m.triangles = tris.ToArray();
			m.normals = normals.ToArray();

			if (recalculateNormals)
				m.RecalculateNormals();

			m.RecalculateBounds();
			;
		}

		public void AddMesh(Mesh m, Transform t)
		{
			//Add the verts
			foreach (Vector3 vert in m.vertices)
			{
				Vector3 cVert = vert;

				cVert.x *= t.lossyScale.x;
				cVert.y *= t.localScale.y;
				cVert.z *= t.localScale.z;

				cVert = Quaternion.Euler(t.rotation.eulerAngles) * cVert;

				cVert = (t.position - this.position) + cVert;

				verts.Add(cVert);
			}

			foreach (Vector3 norm in m.normals)
			{
				normals.Add(norm);
			}

			//Add the uvs.
			foreach (Vector2 uv in m.uv)
			{
				uvs.Add(uv);
			}

			//Add the tris.
			foreach (int tr in m.triangles)
			{
				tris.Add(tr + triOffset);
			}

			triOffset += m.vertexCount;
		
		}
	}
}