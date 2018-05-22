using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	public class BuildingBlockConnector : MonoBehaviour 
	{
		//Will this connector be used in placement
		public bool useInPlacementMode = true;

		//Will this connector be used in scene.
		public bool useInSceneMode = true;

		//Game object that shows the connector in its highlighted state.
		public GameObject highlightObject;

		//Collider that allows raycasting on the connector.
		public Collider raycastCollider;

		//Set to true if you can rotate the object on this connector.
		public bool allowRoation = true;

		//Set to true if you want to disable updating of this connector from the automated 'Update connectors' utility.
		public bool disableAutomatedUpdate = false;
		
		void Awake()
		{
			//parentBuildingBlock = GetComponentInParent<BuildingBlock>();

			if (raycastCollider == null)
			{
				raycastCollider = GetComponentInChildren<MeshCollider>();
			}

			raycastCollider.gameObject.layer = LevelEditorGlobals.BuildingBlockConnector;
			raycastCollider.enabled = false;
			highlightObject.gameObject.SetActive(false);
		}

		/// <summary>
		/// Call to enable to connector colliders and show the connector model
		/// </summary>
		public void EnableCollider()
		{
			if (useInSceneMode) //Don't allow colliders to be enabled if not allowed in scene mode.
			{
				raycastCollider.enabled = true;
				highlightObject.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Call the disable the connector colliders and hide the connector model
		/// </summary>
		public void DisableCollider()
		{
			raycastCollider.enabled = false;
			highlightObject.gameObject.SetActive(false);
		}

	}
}
