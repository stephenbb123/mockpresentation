using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public static class LevelEditorGlobals 
	{
		public static int BuildingBlockConnector = 8;

		public static int BuildingBlockConnectorMask
		{
			get { return 1 << BuildingBlockConnector; }
		}
	}
}