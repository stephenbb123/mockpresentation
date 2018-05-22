using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	public class MaterialSwapper : MonoBehaviour 
	{
		private bool isSetup = false;
		private Material[] originalMaterials;
		private MeshRenderer meshRenderer;

		/// <summary>
		/// Sets up the game object (and all mesh renderers in children) for mesh swapping
		/// </summary>
		public static void SetupGameObjectForMaterialSwapping(GameObject go)
		{
			MeshRenderer mr = go.GetComponent<MeshRenderer>();

			if (mr != null)
			{
				AddMaterialSwapperToGameObject(go);
			}

			foreach (MeshRenderer testRenderer in go.GetComponentsInChildren<MeshRenderer>())
			{
				AddMaterialSwapperToGameObject(testRenderer.gameObject);
			}
		}

		private static void AddMaterialSwapperToGameObject(GameObject go)
		{
			MaterialSwapper ms = go.GetComponent<MaterialSwapper>();

			if (ms == null)
			{
				go.AddComponent<MaterialSwapper>();
			}
		}

		/// <summary>
		/// Set all the mesh renderers in this object to the given material
		/// SetupGameObjectForMaterialSwapping should be called on this object BEFORE this is called.
		/// </summary>
		public static void SetMaterials(GameObject go, Material m)
		{
			foreach (MaterialSwapper ms in go.GetComponentsInChildren<MaterialSwapper>())
			{
				ms.SetTempMaterial(m);
			}
		}

		/// <summary>
		/// Sets all the mesh renderers in this object to use their original material
		/// SetupGameObjectForMaterialSwapping should be called on this object BEFORE this is called.
		/// </summary>
		public static void ResetMaterials(GameObject go)
		{
			foreach (MaterialSwapper ms in go.GetComponentsInChildren<MaterialSwapper>())
			{
				ms.ResetMaterial();
			}
		}

		/// <summary>
		/// Ensures the material swapper is setup and properties setup
		/// </summary>
		private void Setup()
		{
			if (!isSetup)
			{
				isSetup = true;
				meshRenderer = GetComponent<MeshRenderer>();

				if (meshRenderer != null)
				{
					originalMaterials = new Material[meshRenderer.sharedMaterials.Length];
					
					for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
					{
						originalMaterials[i] = meshRenderer.sharedMaterials[i];
					}
					//originalMaterial = meshRenderer.sharedMaterial;
				}
			}
		}

		/// <summary>
		/// Change the default material for this one.
		/// </summary>
		public void SetTempMaterial(Material m)
		{
			Setup();

			if (meshRenderer != null)
			{
				Material[] mats = meshRenderer.sharedMaterials;
				
				for (int i = 0; i < mats.Length; i++)
				{
					mats[i] = m;
				}
				
				meshRenderer.sharedMaterials = mats;
			}

		}

		/// <summary>
		/// Resets the material to the initial setup.
		/// </summary>
		public void ResetMaterial()
		{
			Setup();

			if (meshRenderer != null)
			{
				meshRenderer.sharedMaterials = originalMaterials;
			}
		}
	}
}
