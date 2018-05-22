using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public class StaticModel : MonoBehaviour 
	{
		public string staticModelName = "";
		public float combineTollerance = 10;
		public bool applyMeshCollider = false;
		public bool recalculateNormals = true;
		public bool destroyLevelBlock = false;
	
		/// <summary>
		/// Returns a list of all models in tollerence.
		/// </summary>
		public List<StaticModel> GetModelsInTollerance()
		{
			List<StaticModel> rv = new List<StaticModel>();

			foreach (StaticModel sm in FindObjectsOfType<StaticModel>())
			{
				if (sm.staticModelName == staticModelName)
				{
					float distance = Vector3.Distance(sm.transform.position, transform.position);

					if (distance <= combineTollerance)
					{
						if (sm != this)
						{
							rv.Add(sm);
						}
					}
				}
			}

			return rv;
		}

		/// <summary>
		/// Called when this model is combined.
		/// </summary>
		public void Combined()
		{
			MeshRenderer mr = GetComponent<MeshRenderer>();

			if (mr != null)
			{
				Destroy(mr);
			}

			MeshFilter mf = GetComponent<MeshFilter>();

			if (mf != null)
			{
				Destroy(mf);
			}

			if (applyMeshCollider)
			{
				Collider c = GetComponent<Collider>();

				if (c != null)
					Destroy(c);
			}

			Destroy(this);
			
			if (destroyLevelBlock)
			{
				LevelBlock lb = GetComponentInParent<LevelBlock>();
				Destroy(lb.gameObject);
			}
		}
	}


}