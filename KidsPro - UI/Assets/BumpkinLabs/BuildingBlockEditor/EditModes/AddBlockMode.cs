using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Edit Modes/Add Block Mode")]
	[RequireComponent(typeof(BlockSelector))]
	public class AddBlockMode : EditMode 
	{
		public enum AddBlockModes
		{
			CreatingBlocks,
			MovingBlock
		}

		//The camera doing raycasting.
		public Camera raycastingCamera;

		//The current block being added.
		private BuildingBlock currentBlock;

		//Game objects that aid in placing and orientation of new objects.
		private GameObject placementHelperCurrent, placementHelperScene;

		//Current connector
		private BuildingBlockConnector currentConnector;

		//The block template we're currently using.
		private BuildingBlock blockTemplate;

		//The block selector
		private BlockSelector blockSelector;

		//Current mode
		private AddBlockModes currentMode;

		//Current rotation
		private float rotation = 0.0f;

		private static AddBlockMode instance;
	
		
		public static AddBlockMode Instance
		{
			get	
			{
				if (instance == null)
				{
					instance = FindObjectOfType<AddBlockMode>();
					
					if (instance == null)
					{
						instance = new GameObject("AddBlockMode", typeof(AddBlockMode)).GetComponent<AddBlockMode>();
					}
				}
				
				return instance;
			}
		}

		public void SetMode(AddBlockModes mode)
		{
			currentMode = mode;
		}

		void Start()
		{
			//Create objects that help in the placement of blocks.
			placementHelperCurrent = new GameObject("Placement Helper Current");
			placementHelperScene = new GameObject("Placement Helper Scene");

			//Get handles to other components.
			blockSelector = GetComponent<BlockSelector>();

			//Setup the block selector events.
			blockSelector.OnBuildingBlockHoverOut += HandleOnBuildingBlockHoverOut;
			blockSelector.OnBuildingBlockHoverOver += HandleOnBuildingBlockHoverOver;
		}

		void HandleOnBuildingBlockHoverOver (BuildingBlock block)
		{
			block.EnableConnectors();
		}

		void HandleOnBuildingBlockHoverOut (BuildingBlock block)
		{
			block.DisableConnectors();
		}

		/// <summary>
		/// Enables this edit mode.  OnUpdate will now be called until disable mode is called
		/// </summary>
		/// <param name="arg">Building block that will become current</param>
		public override void EnableMode (Object arg)
		{
			//Reset the rotation
			rotation = 0.0f;

			GameObject go = arg as GameObject;

			//Cast the arg as a building block and create a new instance of it.
			blockTemplate = go.GetComponent<BuildingBlock>();

			CreateBlock(-1);

			//If we are in 'move' mode we have just created a copy
			if (currentMode == AddBlockModes.MovingBlock)
			{
				//Clone the id.
				currentBlock.BuildingBlockID = blockTemplate.BuildingBlockID;

				//Destory the original.
				Destroy(go);
			}

			base.EnableMode (arg);
		}

		private void CreateBlock(int connectorIndex)
		{
			BuildingBlock newObject = null;

			newObject = Instantiate(blockTemplate, Vector3.zero, Quaternion.identity) as BuildingBlock;

		
			//Put the current block in placement mode.
			currentBlock = newObject.GetComponent<BuildingBlock>();

			//If connectorIndex has been set (is not < 1) then set it.
			if (connectorIndex > -1)
				currentBlock.ConnectorIndex = connectorIndex;
	
			currentBlock.EnterPlacementMode();
		}

		/// <summary>
		/// Disables this edit mode
		/// </summary>
		public override void DisableMode ()
		{
			//Reset the current connector.
			currentConnector = null;

			//if there is a current block destroy it.
			if (currentBlock != null)
			{
				Destroy(currentBlock.gameObject);
			}

			blockSelector.SetNull(true);

			//Set rotate to false.

			base.DisableMode ();
		}

	
		/// <summary>
		/// Places the current block on current connector.
		/// </summary>
		private void PlaceCurrentBlockOnCurrentConnector()
		{
			//Place the current placement helper.
			placementHelperCurrent.transform.position = currentBlock.CurrentConnector.transform.position;
			placementHelperCurrent.transform.rotation = currentBlock.CurrentConnector.transform.rotation;

			//Place the scene placement helper.
			placementHelperScene.transform.position = currentConnector.transform.position;
			placementHelperScene.transform.rotation = currentConnector.transform.rotation;

			//Make the current block a child of placement helper placed at its connector.
			currentBlock.transform.parent = placementHelperCurrent.transform;

			//Move the placement helpers to the same location.
			placementHelperCurrent.transform.position = placementHelperScene.transform.position;

			//Rotate the placement helper.
			placementHelperCurrent.transform.forward = -placementHelperScene.transform.forward;

			//Rotate by rotation
			placementHelperCurrent.transform.Rotate(0,0,rotation, Space.Self);

			//Unparent the current block.
			currentBlock.transform.parent = null;
		}

		/// <summary>
		/// Changes the current connector we're hovering over.  Also accepts nulls.
		/// </summary>
		private void ExchangeCurrentConnector(BuildingBlockConnector newConnector)
		{
			if (newConnector == currentConnector)
				return;

			currentConnector = newConnector;

			if (currentConnector != null)
			{
				PlaceCurrentBlockOnCurrentConnector();
				currentBlock.EnterOnConnectorMode(currentConnector);
			}
			else
			{
				currentBlock.EnterPlacementMode();
			}
		}

		/// <summary>
		/// Performs a raycast on connectors 
		/// Passes them to Exchange current connector.
		/// </summary>
		private void UpdateSelectedConnector()
		{
			BuildingBlockConnector newConnector = null;
			RaycastHit hit;
			Ray r = raycastingCamera.ScreenPointToRay(IGEMouseController.Instance.MousePosition);

			if (blockSelector.CurrentOverBlock != null)
			{
				if (Physics.Raycast(r, out hit, 30000f, LevelEditorGlobals.BuildingBlockConnectorMask))
				{
					newConnector = hit.collider.gameObject.GetComponentInParent<BuildingBlockConnector>();
				}
			}

			ExchangeCurrentConnector(newConnector);
		}

		private void TestForNextConnector()
		{
			if (currentBlock.CurrentState == BuildingBlock.BuildingBlockState.OnConnector)
			{
				if (IGEMouseController.Instance.MouseRightDown())
				{
					if (!Input.GetKey(KeyCode.LeftShift))
					{
						currentBlock.SelectNextConnector();
						PlaceCurrentBlockOnCurrentConnector();
						currentBlock.EnterOnConnectorMode(currentConnector);
					}
				}
			}
		}

		private void TestForRotation()
		{
			if (currentBlock.CurrentState == BuildingBlock.BuildingBlockState.OnConnector)
			{
				if (IGEMouseController.Instance.MouseRightDown())
				{
					if (Input.GetKey(KeyCode.LeftShift))
					{
						rotation -= 90f;

						if (rotation == -360f)
							rotation = 0;

						PlaceCurrentBlockOnCurrentConnector();
						currentBlock.EnterOnConnectorMode(currentConnector);
					}
				}
			}
		}

		/// <summary>
		/// Checks if we want to add the block to the scene.
		/// </summary>
		private void TestForAdd()
		{
			if (currentBlock != null)
			{
				if (currentBlock.CanBeAdded)
				{
					if (IGEMouseController.Instance.MouseLeftDown())
					{
						AddCurrentBlockToScene();	
					}
				}
			}
		}

		private void AddCurrentBlockToScene()
		{
			int currentConnectorIndex = currentBlock.ConnectorIndex;

			currentBlock.AddToScene();
			
			//Reset connectors
			blockSelector.SetNull(true);
			
			currentConnector = null;
			
			if (currentMode == AddBlockModes.CreatingBlocks)
			{
				//Make a new block.
				CreateBlock(currentConnectorIndex);
			}
			
			if (currentMode == AddBlockModes.MovingBlock)
			{
				currentBlock = null;
				LevelEditorManager.Instance.EnterSelectBlockForMovingMode();
			}
		}



		/// <summary>
		/// Called each frame when the mode is active.
		/// </summary>
		public override void OnUpdate ()
		{
			blockSelector.UpdateHoverBlock();

			UpdateSelectedConnector();
            
			TestForNextConnector();
			TestForRotation();
		
			TestForAdd();

			base.OnUpdate ();
		}
	}
}
