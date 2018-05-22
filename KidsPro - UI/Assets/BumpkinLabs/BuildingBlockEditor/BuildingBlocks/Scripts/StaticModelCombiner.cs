using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public class StaticModelCombiner 
	{
		private List<StaticModel> staticModels;

		/// <summary>
		/// Combine static models.
		/// </summary>
		public void CombineModels()
		{
			//Create a list to contain the models.
			if (staticModels == null)
				staticModels = new List<StaticModel>();

			staticModels.Clear();

			//Add all static models to the list.
			foreach (StaticModel sm in GameObject.FindObjectsOfType<StaticModel>())
			{
				staticModels.Add(sm);
			}

			//While there are still models in the list, call combine.
			while(staticModels.Count > 0)
			{
				CombineModel(staticModels[0]);
			}
		}

		/// <summary>
		/// Using the provided staticModel as a starting point - create a combined model.
		/// </summary>
		private void CombineModel(StaticModel staticModel)
		{
			//New list for models that can be combined.
			List<StaticModel> modelsToCombine = new List<StaticModel>();

			//Find the models that can be combined with staticModel
			AddModelToList(staticModel, modelsToCombine);

			//Combine these models.
			CreateCombinedMesh(modelsToCombine);
		}

		/// <summary>
		/// Adds static models within a tollerance to staticModel to list.
		/// </summary>
		private void AddModelToList(StaticModel staticModel, List<StaticModel> list)
		{
			if (staticModels.Contains(staticModel))
			{
				staticModels.Remove(staticModel);

				if (!list.Contains(staticModel))
					list.Add(staticModel);
			}

			foreach (StaticModel sm in staticModel.GetModelsInTollerance())
			{
				if (staticModels.Contains(sm))
					AddModelToList(sm, list);
			}
		}

		//Combine all staticModels in the staticModels list into one mesh
		private void CreateCombinedMesh(List<StaticModel> staticModels)
		{
			//Create the object
			GameObject newGameObject = CreateGameObject(staticModels);

			//Create the combined mesh
			CombinedMesh combinedMesh = new CombinedMesh(newGameObject.transform.position);

			//Add each mesh to the combinedMesh.
			foreach (StaticModel sm in staticModels)
			{
				MeshFilter mf = sm.gameObject.GetComponent<MeshFilter>();

				combinedMesh.AddMesh(mf.sharedMesh, sm.transform);
			}
	
			//Add components.
			newGameObject.AddComponent<MeshRenderer>();
			newGameObject.AddComponent<MeshFilter>();

			MeshFilter mff = newGameObject.GetComponent<MeshFilter>();

			//Cteaye the mesh.
			combinedMesh.CreateMesh(mff.mesh, staticModels[0].recalculateNormals);

			MeshRenderer mr = newGameObject.GetComponent<MeshRenderer>();
			MeshRenderer current = staticModels[0].gameObject.GetComponent<MeshRenderer>();

			mr.sharedMaterial = current.sharedMaterial;

			if (staticModels[0].applyMeshCollider)
			{
				newGameObject.AddComponent<MeshCollider>();
				MeshCollider mc = newGameObject.GetComponent<MeshCollider>();
				mc.sharedMesh = mff.sharedMesh;
			}

			foreach (StaticModel staticModel in staticModels)
			{
				staticModel.Combined();
			}
		}

		/// <summary>
		/// Creates a game object centered around the positions in the list of static models.
		/// </summary>
		private GameObject CreateGameObject(List<StaticModel> models)
		{
			float minX = 0f, minY = 0f, minZ = 0f, maxX = 0f, maxY = 0f, maxZ = 0f;

			bool firstPass = true;

			foreach (StaticModel sm in models)
			{
				if (firstPass)
				{
					minX = sm.transform.position.x;
					minY = sm.transform.position.y;
					minZ = sm.transform.position.z;

					maxX = sm.transform.position.x;
					maxY = sm.transform.position.y;
					maxZ = sm.transform.position.z;
				}
				else
				{
					minX = Mathf.Min(minX, sm.transform.position.x);
					minY = Mathf.Min(minY, sm.transform.position.y);
					minZ = Mathf.Min(minZ, sm.transform.position.z);

					maxX = Mathf.Max(maxX, sm.transform.position.x);
					maxY = Mathf.Max(maxY, sm.transform.position.y);
					maxZ = Mathf.Max(maxZ, sm.transform.position.z);
				}
			}

			GameObject combinedMesh = new GameObject("CombinedMesh");
			combinedMesh.transform.position = new Vector3(
				minX + ((maxX - minX) / 2f),
				minY + ((maxY - minY) / 2f),
				minY + ((maxZ - minZ) / 2f)
				 );

			return combinedMesh;
		}

	}
}
