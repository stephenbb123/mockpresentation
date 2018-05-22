using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Edit Modes/Block Selector")]
	public class BlockSelector : MonoBehaviour 
	{
		public delegate void BuildingBlockChangedDelegate(BuildingBlock block);

		public event BuildingBlockChangedDelegate OnBuildingBlockHoverOut;
		public event BuildingBlockChangedDelegate OnBuildingBlockHoverOver;

		public delegate void BuildingBlockRaycastedDelegate(BuildingBlock block, RaycastHit raycastHit);
		public event BuildingBlockRaycastedDelegate OnBuildingBlockRaycasted;

		public delegate void NonBuildingBlockRaycastedDelegate(RaycastHit raycastHit);
		public event NonBuildingBlockRaycastedDelegate OnNonBuildingBlockRaycasted;

		public delegate void GameObjectChangedDelegate(GameObject go);
		public event GameObjectChangedDelegate OnGameObjectHoverOut;
		public event GameObjectChangedDelegate OnGameObjectHoverOver;

		private BuildingBlock currentOverBlock = null;
		private GameObject currentGameObject = null;

		public BuildingBlock CurrentOverBlock
		{
			get
			{
				return currentOverBlock; 
			}

			set
			{
				currentOverBlock = value;
			}
		}

		/// <summary>
		/// Sets the current game object being hovered over
		/// </summary>
		private void ExchangeCurrentGameObject(GameObject newSelection)
		{
			//If there is no change return.
			if (newSelection == currentGameObject)
				return;

			//There is a different selection.
			if (currentGameObject != null)
			{
				//We have moved of an object.
				if (OnGameObjectHoverOut != null)
					OnGameObjectHoverOut(currentGameObject);
			}

			//Setup the new selection.
			currentGameObject = newSelection;

			//If the new selection is not null send an over event.
			if (currentGameObject != null)
			{
				if (OnGameObjectHoverOver != null)
					OnGameObjectHoverOver(currentGameObject);
			}
		}

		/// <summary>
		/// Sets the current building block being hovered over.  Also handles null.
		/// </summary>
		private void ExchangeCurrentHoverBlock(BuildingBlock newSelection)
		{
			//If there has been no change return.
			if (newSelection == currentOverBlock)
				return;
			
			//There is a different block - swtich the current one off.
			if (currentOverBlock != null)
			{
				//We have moved off a block.
				if (OnBuildingBlockHoverOut != null)
					OnBuildingBlockHoverOut(currentOverBlock);
			}
			
			//Set the new selection up.
			currentOverBlock = newSelection;
			
			if (currentOverBlock != null)
			{
				if (OnBuildingBlockHoverOver != null)
					OnBuildingBlockHoverOver(currentOverBlock);
			}
		}
		
		/// <summary>
		/// Performs a raycast to see if we're over a building block.  
		/// Passes that block to ExchangeCurrentHoverBlock
		/// </summary>
		public void UpdateHoverBlock()
		{
			RaycastHit hit;
			BuildingBlock newBlock = null;
			GameObject newObject = null;

			if (!IGEMouseController.Instance.OverGUIElement)
			{
				Ray r = LevelEditorManager.Instance.editorCamera.ScreenPointToRay(IGEMouseController.Instance.MousePosition);
				
				if (Physics.Raycast(r, out hit, 30000f, 1))
				{
					newBlock = hit.collider.GetComponentInParent<BuildingBlock>();

					//If we have scanned a building block trigger the raycast event.
					if (newBlock != null)
					{
						if (OnBuildingBlockRaycasted != null)
							OnBuildingBlockRaycasted(newBlock, hit);
					}
					else
					{
						newObject = hit.collider.gameObject;

						if (OnNonBuildingBlockRaycasted != null)
							OnNonBuildingBlockRaycasted(hit);
					}
				}
			}
			
			ExchangeCurrentHoverBlock(newBlock);
			ExchangeCurrentGameObject(newObject);
		}

		/// <summary>
		/// Sets the current block as null.
		/// This should be called if the block selector is no longer in use by an editor.
		/// </summary>
		/// <param name="fireOutEvent">If true then the on hover out event is fired.</param>
		public void SetNull(bool fireOutEvent)
		{
			if (currentOverBlock != null)
			{
				if (fireOutEvent)
				{
					if (OnBuildingBlockHoverOut != null)
						OnBuildingBlockHoverOut(currentOverBlock);
				}

				currentOverBlock = null;
			}
		}

		/// <summary>
		/// Destroies the current block.
		/// </summary>
		public void DestroyCurrentBlock()
		{
			if (currentOverBlock != null)
			{
				Destroy(currentOverBlock.gameObject);
				currentOverBlock = null;
			}
		}
	
	}
}	