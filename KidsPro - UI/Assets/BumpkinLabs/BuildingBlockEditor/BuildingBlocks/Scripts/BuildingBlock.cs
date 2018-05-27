using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Blocks/Building Block")]
	public class BuildingBlock : MonoBehaviour 
	{
		//Name of the building block
		public string buildingBlockName = "";

		//Sub name of the block
		public string buildingBlockSubName = "";

		//A mesh that defines the area (and selection) of the mesh.
		public DetailedMeshCollider blockMesh;

		//A game object that reprisents the look of the block.
		public GameObject blockModel;

		//Allow non-connector placement or this object
		public bool allowDecorativePlacement = false;

		//Allow for decrative objects to be placed on this block.
		public bool allowDecorateObjects = true;

		//Game object to be enabled when the block is placed in the scene.
		public GameObject[] inSceneObjects;

		//Force the orientation of the model.
		public bool forceModelRotation = false;

		//Rotation to be foreced if forceModelRotation = true;
		public Vector3 forcedRotation = Vector3.zero;
		
		//If true then the block cannot be deleted one placed in edit mode.
		public bool locked = false;

		public bool dontCheckOverlaps = false;

		//The id of the block.
		private string buildingBlockId = "";

		//The current connector that is being used to place the block.
		private BuildingBlockConnector currentConnector;

		//The index of the current connector.
		private int connectorIndex;

		//The current state of the building block.
		private BuildingBlockState currentState;

		//List of connectors.
		private List<BuildingBlockConnector> connectors;
	

		public static BuildingBlock FindFromID(string id)
		{
			foreach (BuildingBlock bb in FindObjectsOfType<BuildingBlock>())
			{
				if (bb.BuildingBlockID == id)
					return bb;
			}

			Debug.Log(string.Format("Could not find block {0}", id));

			return null;
		}

		public enum BuildingBlockState
		{
			BeingAdded, //In the scene but invisible
			OnConnector, //On a connector
			InScene //Is in the scene.
		}

		public int ConnectorIndex
		{
			get
			{
				return connectorIndex;
			}
			set
			{
				while(connectorIndex != value)
				{
					SelectNextConnector();
				}
			}
		}

		public string BuildingBlockID
		{
			get
			{
				//if there is no building block ID currently create one
				if (buildingBlockId == "")
					buildingBlockId = System.Guid.NewGuid().ToString();

				if (string.IsNullOrEmpty(buildingBlockId))
					buildingBlockId = System.Guid.NewGuid().ToString();

				return buildingBlockId;
			}
			set
			{
				buildingBlockId = value;
			}
		}

	
		//Set by CheckOverlap - true if the block overlaps another.
		private bool overlap = false;

        //Set when we enter 'OnConnectorMode'
        private bool connectorTagsMatch = true;

		/// <summary>
		/// Gets the current state of the block
		/// </summary>
		public BuildingBlockState CurrentState
		{
			get { return currentState; }
		}
		
		private void Awake()
		{
			//setup the material swapper
			MaterialSwapper.SetupGameObjectForMaterialSwapping(blockModel);
		
			//Ensure that in game only objects are disabled
			ActivateInGameObjects(false);
		}

		/// <summary>
		/// True if block is on a connector and there is no overlap.
		/// </summary>
		public bool CanBeAdded
		{
			get
			{
				if (currentState == BuildingBlockState.OnConnector)
				{

					if (overlap)
					{
						return false;
					}

                    if (!connectorTagsMatch)
                    {
                        return false;
                    }

                    return true;
				}

				return false;
			}
		}

		/// <summary>
		/// The current connector used on the incomming block
		/// </summary>
		public BuildingBlockConnector CurrentConnector
		{
			get
			{
				if (currentConnector == null)
				{
					InitializeConnectorList();

					currentConnector = connectors[connectorIndex];
				}


				return currentConnector;
			}
		}

		/// <summary>
		/// Ensures a connector list exists.
		/// </summary>
		private void InitializeConnectorList()
		{
			if (connectors == null)
			{
				connectors = new List<BuildingBlockConnector>();
			
				foreach (BuildingBlockConnector bbc in GetComponentsInChildren<BuildingBlockConnector>())
				{
					connectors.Add(bbc);
				}
			}
		}

		/// <summary>
		/// Selects the next connector.
		/// </summary>
		public void SelectNextConnector()
		{
			bool validNextConnectorFound = false;
			int loopCount = 0;

			InitializeConnectorList();

			while(!validNextConnectorFound)
			{
				connectorIndex++;

				if (connectorIndex == connectors.Count)
					connectorIndex = 0;

				currentConnector = connectors[connectorIndex];

				if (currentConnector.useInPlacementMode)
					validNextConnectorFound = true;

				if (loopCount > 1000)
				{
					Debug.LogError("A connector cannot be found on this object that allows placement");
					validNextConnectorFound = true;
				}

				loopCount++;
			}
		}

		/// <summary>
		/// Ready the block for placement.
		/// </summary>
		public void EnterPlacementMode()
		{
			currentState = BuildingBlockState.BeingAdded;

			//Disable all colliders as we don't want them to affect raycasts.
			blockMesh.DisableMeshCollider();

			//Don't show the model.
			blockModel.gameObject.SetActive(false);

			//Shrink the model slightly for overlaps.
			blockMesh.Shrink();

			//Rotate the model if required.
			SetForcedModelRotation();
		}

		/// <summary>
		/// Checks for overlaps with other modesl.
		/// </summary>
		private void CheckOverlap()
		{
			//If we don't want to check for overlaps - for example, this is a decorative object - get out.
			if (dontCheckOverlaps)
			{
				overlap = false;
				return;
			}

			//Ensure this blocks mesh is updated.
			blockMesh.UpdatePosition();
	
			//Set the overlap to false initially.
			overlap = false;

			//Check against all the other blocks in the scene.
			foreach (BuildingBlock bb in FindObjectsOfType<BuildingBlock>())
			{
				if (bb != this) //don't check against ourself.
				{
					if (TestForOverlapWithOther(bb))
					{
						//If there is an overlap set the flag and stop.
						overlap = true;
						return;
					}
				}
			}
		}

		/// <summary>
		/// Enters the on connector mode - called when a potential connector is selected. 
		/// </summary>
		public void EnterOnConnectorMode(BuildingBlockConnector connector)
		{
			currentState = BuildingBlockState.OnConnector;

			//Show the model.
			blockModel.gameObject.SetActive(true);

			CheckOverlap();

            if (connector != null)
            {
                connectorTagsMatch = (connector.tag == CurrentConnector.tag) ? true : false;
            }
            else
            {
                connectorTagsMatch = true;
            }

			//Set the material based on the overlap flag.
			if (overlap || !connectorTagsMatch)
			{
				MaterialSwapper.SetMaterials(blockModel, LevelEditorManager.Instance.potenialPlacementBlockedMaterial);
			}
			else
			{
				MaterialSwapper.ResetMaterials(blockModel);
			}

			SetForcedModelRotation();
		}

		public void EnterHoverOverForDeleteMode()
		{
			MaterialSwapper.SetMaterials(blockModel, LevelEditorManager.Instance.deletionHighlightMaterial);
		}

		public void ExitHoverOverForDeleteMode()
		{
			MaterialSwapper.ResetMaterials(blockModel);
		}

		public void EnterHoverOverForEditMode()
		{
			MaterialSwapper.SetMaterials(blockModel, LevelEditorManager.Instance.editHighlightMaterial);
		}
		
		public void ExitHoverOverForEditMode()
		{
			MaterialSwapper.ResetMaterials(blockModel);
		}

		/// <summary>
		/// If forceModelRotation is set this method rotates the object
		/// </summary>
		private void SetForcedModelRotation()
		{
			//Set the rotation of the object, if required.
			if (forceModelRotation)
			{
				blockModel.transform.rotation = Quaternion.Euler(forcedRotation);
			}
		}

		/// <summary>
		/// Add this building block to the scene
		/// </summary>
		public void AddToScene()
		{
			currentState = BuildingBlockState.InScene;

			//Reset the size of the collider
			blockMesh.ResetScale();

			//Ensure the collider position is updated.
			blockMesh.UpdatePosition();
		
			//Enable the collider.
			blockMesh.EnableMeshCollider();

			//Ensure the model is visible and showing the correct material.
			blockModel.gameObject.SetActive(true);
			//MaterialSwapper.ResetMaterials(blockModel);

			//Make any extra object active.
			ActivateInGameObjects(true);

			//If there is an editable block object attached call the added to scene method.
			EditableBlock eb = GetComponentInChildren<EditableBlock>();



			if (eb != null)
				eb.AddedToScene();
		}

		/// <summary>
		/// Activates objects that are only seen once added to the scene.
		/// </summary>
		private void ActivateInGameObjects(bool activate)
		{
			if (inSceneObjects != null)
			{
				foreach (GameObject go in inSceneObjects)
				{
					go.SetActive(activate);
				}
			}
		}

		/// <summary>
		/// Enables the colliders.
		/// </summary>
		public void EnableConnectors()
		{
			foreach (BuildingBlockConnector connector in GetComponentsInChildren<BuildingBlockConnector>())
			{
				connector.EnableCollider();
			}
		}

		/// <summary>
		/// Disables the colliders.
		/// </summary>
		public void DisableConnectors()
		{
			foreach (BuildingBlockConnector connector in GetComponentsInChildren<BuildingBlockConnector>())
			{
				connector.DisableCollider();
			}
		}

		/// <summary>
		/// Returns true if this block overlaps with the supplied block
		/// </summary>
		private bool TestForOverlapWithOther(BuildingBlock bb)
		{
			return blockMesh.CollisionTest(bb.blockMesh);
		}


	}
}