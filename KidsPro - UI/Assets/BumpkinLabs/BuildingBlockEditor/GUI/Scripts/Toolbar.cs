using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public class Toolbar : MonoBehaviour 
	{
		public string toolbarName = "";

		/// <summary>
		/// Finds the toolbar with the given name
		/// </summary>
		public static Toolbar FindToolbarByName(string name)
		{
			foreach (Toolbar tb in FindObjectsOfType<Toolbar>())
			{
				if (tb.toolbarName == "name")
				{
					return tb;
				}
			}

			return null;
		}
	}
}
