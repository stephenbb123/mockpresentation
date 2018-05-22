using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Edit Modes/Select Block Mode")]
	[RequireComponent(typeof(BlockSelector))]
	public class SelectBlockMode : EditMode 
	{
		public enum SelectBlockModes
		{
			SelectForEditing,
			SelectForMoving
		}

		//The camera doing raycasting.
		public Camera raycastingCamera;
		
		//The block selector.
		private BlockSelector blockSelector;

		//current mode
		private SelectBlockModes currentMode = SelectBlockModes.SelectForEditing;
		
		private static SelectBlockMode instance;

		public static SelectBlockMode Instance
		{
			get	
			{
				if (instance == null)
				{
					instance = FindObjectOfType<SelectBlockMode>();
					
					if (instance == null)
					{
						instance = new GameObject("SelectBlockMode", typeof(SelectBlockMode)).GetComponent<SelectBlockMode>();
					}
				}
				
				return instance;
			}
		}

		public void SetCurrentMode(SelectBlockModes mode)
		{
			currentMode = mode;
		}

		void Start()
		{
			//Set up components.
			blockSelector = GetComponent<BlockSelector>();

			//Set up block selector.
			blockSelector.OnBuildingBlockHoverOver += HandleOnBuildingBlockHoverOver;
			blockSelector.OnBuildingBlockHoverOut += HandleOnBuildingBlockHoverOut;
		}
		
		public override void EnableMode (Object arg)
		{
			base.EnableMode (arg);
		}
		
		public override void DisableMode ()
		{
			blockSelector.SetNull(true);
			
			base.DisableMode ();
		}

		void HandleOnBuildingBlockHoverOut (BuildingBlock block)
		{
			block.ExitHoverOverForEditMode();
		}
		
		void HandleOnBuildingBlockHoverOver (BuildingBlock block)
		{
			block.EnterHoverOverForEditMode();
		}
		
		private void CheckForClick()
		{
			if (blockSelector.CurrentOverBlock != null)
			{
				if (IGEMouseController.Instance.MouseLeftDown())
				{
					if (currentMode == SelectBlockModes.SelectForEditing)
					{
						EditableBlock eb = blockSelector.CurrentOverBlock.GetComponentInChildren<EditableBlock>();

						if (eb != null)
						{
							LevelEditorManager.Instance.EnterEditingObjectMode(eb);
						}
					}

					if (currentMode == SelectBlockModes.SelectForMoving)
					{
						Debug.Log("Now I'm here");
						BuildingBlock currentBlock = blockSelector.CurrentOverBlock;
						blockSelector.SetNull(false);
						currentBlock.ExitHoverOverForEditMode();
						LevelEditorManager.Instance.EnterAddBlockForMovingMode(currentBlock);
					}
				}
			}
		}
		
		public override void OnUpdate ()
		{

			blockSelector.UpdateHoverBlock();
			CheckForClick();

			base.OnUpdate ();
		}
	}
}
