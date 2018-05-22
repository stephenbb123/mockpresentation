using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Edit Modes/Edit Mode")]
	public class EditMode : MonoBehaviour 
	{
		//Is this the current mode
		private bool isActive = false;

		private int frameTimer = 0;

		public virtual void OnUpdate() {}

		public virtual void EnableMode(Object arg)
		{
			frameTimer = 2;
			isActive = true;
		}

		public virtual void DisableMode()
		{
			isActive = false;
		}

		void Update () 
		{
			if (isActive)
			{
				if (frameTimer > 0)
					frameTimer--;

				if (frameTimer == 0)
					OnUpdate();
			}
		}
	}
}
