using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Edit Modes/Editing Object Mode")]
	public class EditingObjectMode : EditMode 
	{
		private EditableBlock currentBlock = null;

		private static EditingObjectMode instance;
		
		public static EditingObjectMode Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<EditingObjectMode>();
					
					if (instance == null)
					{
						instance = new GameObject("EditingObjectMode", typeof(EditingObjectMode)).GetComponent<EditingObjectMode>();
					}
				}
				
				return instance;
			}
		}

		public override void EnableMode (Object arg)
		{
			EditableBlock eb = arg as EditableBlock;

			if (eb == null)
				Debug.Log(string.Format("The object {0} is not a editable block", arg.name));

			if (currentBlock != null)
			{
				currentBlock.ExitEditMode();
			}

			currentBlock = eb;
			currentBlock.EnterEditMode();

			base.EnableMode (arg);
		}

		public override void DisableMode ()
		{
			if (currentBlock != null)
			{
				currentBlock.ExitEditMode();
			}

			currentBlock = null;

			base.DisableMode ();
		}
	}
}