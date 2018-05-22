using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Edit Modes/Add Free Mode")]
	[RequireComponent(typeof(BlockSelector))]
	public class AddFreeMode : EditMode 
	{
		public enum AddFreeModes
		{
			CreatingBlocks,
			MovingBlocks
		}

		//The camera doing raycasting.
		public Camera raycastingCamera;
		
		//The current block being added.
		private BuildingBlock currentBlock;
		
		//Game object that aids in placing and orientation of new objects.
		private GameObject placementHelperCurrent;

		//The block template we're currently using.
		private BuildingBlock blockTemplate;

		//The block selector.
		private BlockSelector blockSelector;

		//Current mode
		private AddFreeModes currentMode;

		//current rotation
		private float rotation =0f;
		
		private static AddFreeMode instance;

		public static AddFreeMode Instance
		{
			get	
			{
				if (instance == null)
				{
					instance = FindObjectOfType<AddFreeMode>();
					
					if (instance == null)
					{
						instance = new GameObject("AddBlockMode", typeof(AddFreeMode)).GetComponent<AddFreeMode>();
					}
				}
				
				return instance;
			}
		}

		public void SetCurrentMode(AddFreeModes mode)
		{
			currentMode = mode;
		}
		
		void Start()
		{
			//Create objects that help in the placement of blocks.
			placementHelperCurrent = new GameObject("Placement Helper Current");

			//Setup handles to components.
			blockSelector = GetComponent<BlockSelector>();

			//Setup the block selector.
			blockSelector.OnBuildingBlockRaycasted += HandleOnBuildingBlockRaycasted;
			blockSelector.OnBuildingBlockHoverOut += HandleOnBuildingBlockHoverOut;
		}

		
		/// <summary>
		/// Enables this edit mode.  OnUpdate will now be called until disable mode is called
		/// </summary>
		/// <param name="arg">Building block that will become current</param>
		public override void EnableMode (Object arg)
		{
			GameObject go = arg as GameObject;
			
			//Cast the arg as a building block and create a new instance of it.
			blockTemplate = go.GetComponent<BuildingBlock>();
			
			if (blockTemplate == null)
			{
				Debug.LogError(string.Format("{0} does not contain a Building Block", arg.name));
			}

			CreateBlock();

			//If we are in 'move' mode we have just created a copy
			if (currentMode == AddFreeModes.MovingBlocks)
			{
				//Clone the id.
				currentBlock.BuildingBlockID = blockTemplate.BuildingBlockID;

				//Destroy the original.
				Destroy(go);
			}
			
			base.EnableMode (arg);
		}
		
		private void CreateBlock()
		{
			BuildingBlock newObject = null;

			newObject = Instantiate(blockTemplate, Vector3.zero, Quaternion.identity) as BuildingBlock;

			//Put the current block in placement mode.
			currentBlock = newObject.GetComponent<BuildingBlock>();
			
			currentBlock.EnterPlacementMode();
		}
		
		/// <summary>
		/// Disables this edit mode
		/// </summary>
		public override void DisableMode ()
		{
			Debug.Log("Disabling mode");
			//if there is a current block destroy it.
			if (currentBlock != null)
			{
				Destroy(currentBlock.gameObject);
			}
			
			blockSelector.SetNull(true);

			base.DisableMode ();
		}

		/// <summary>
		/// Called when block selected moves off a block
		/// </summary>
		void HandleOnBuildingBlockHoverOut (BuildingBlock block)
		{
			currentBlock.EnterPlacementMode();
		}

		/// <summary>
		/// Called when the block selected is over a block
		/// </summary>
		void HandleOnBuildingBlockRaycasted (BuildingBlock block, RaycastHit raycastHit)
		{
			bool validObject = false;

			if (block.allowDecorateObjects)
				validObject = true;

			if (validObject)
			{
				placementHelperCurrent.transform.position = currentBlock.CurrentConnector.transform.position;
				placementHelperCurrent.transform.rotation = currentBlock.CurrentConnector.transform.rotation;
				
				currentBlock.transform.parent = placementHelperCurrent.transform;
				
				placementHelperCurrent.transform.position = raycastHit.point;
				placementHelperCurrent.transform.forward = -raycastHit.normal;

				//Rotate by rotation
				placementHelperCurrent.transform.Rotate(0,0,rotation, Space.Self);
				
				currentBlock.transform.parent = null;
				
				currentBlock.EnterOnConnectorMode(null);
			}
			else
			{
				currentBlock.EnterPlacementMode();
			}
		}

		private void TestForRotation()
		{
			if (currentBlock != null)
			{
				if (IGEMouseController.Instance.MouseRightDown())
				{
					if (Input.GetKey(KeyCode.LeftShift))
					{
						rotation -= 90f;
						
						if (rotation == -360f)
							rotation = 0;
						

						currentBlock.EnterOnConnectorMode(null);
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
						currentBlock.AddToScene();
						
						//Reset connectors
						blockSelector.SetNull(false);

						if (currentMode == AddFreeModes.CreatingBlocks)
						{
							//Make a new block.
							CreateBlock();
						}

						if (currentMode == AddFreeModes.MovingBlocks)
						{
							Debug.Log("Adding block");
							currentBlock = null;
							LevelEditorManager.Instance.EnterSelectBlockForMovingMode();
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Called each frame when the mode is active.
		/// </summary>
		public override void OnUpdate ()
		{
			blockSelector.UpdateHoverBlock();
		
			TestForAdd();
			TestForRotation();
			
			base.OnUpdate ();
		}
	}
}
