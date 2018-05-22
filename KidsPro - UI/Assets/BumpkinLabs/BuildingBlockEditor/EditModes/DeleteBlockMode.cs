using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Edit Modes/Delete Block Mode")]
	[RequireComponent(typeof(BlockSelector))]
	public class DeleteBlockMode : EditMode 
	{
		//The block selector.
		private BlockSelector blockSelector;

		private static DeleteBlockMode instance;
		
		public static DeleteBlockMode Instance
		{
			get	
			{
				if (instance == null)
				{
					instance = FindObjectOfType<DeleteBlockMode>();
					
					if (instance == null)
					{
						instance = new GameObject("DeleteBlockMode", typeof(DeleteBlockMode)).GetComponent<DeleteBlockMode>();
					}
				}
				
				return instance;
			}
		}

		void Start()
		{
			//Set up components.
			blockSelector = GetComponent<BlockSelector>();

			//Set up the block selector.
			blockSelector.OnBuildingBlockHoverOver += HandleOnBuildingBlockHoverOver;
			blockSelector.OnBuildingBlockHoverOut += HandleOnBuildingBlockHoverOut;
		}

		void HandleOnBuildingBlockHoverOut (BuildingBlock block)
		{
			block.ExitHoverOverForDeleteMode();
		}

		void HandleOnBuildingBlockHoverOver (BuildingBlock block)
		{
			if (!block.locked)
				block.EnterHoverOverForDeleteMode();
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


		private void CheckForEsc()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				DisableMode();
			}
		}

		private void CheckForClick()
		{
			if (blockSelector.CurrentOverBlock != null)
			{
				if (IGEMouseController.Instance.MouseLeftDown())
				{
					blockSelector.DestroyCurrentBlock();
				}
			}
		}

		public override void OnUpdate ()
		{
			blockSelector.UpdateHoverBlock();
			CheckForClick();
			CheckForEsc();
			base.OnUpdate ();
		}
	}
}
