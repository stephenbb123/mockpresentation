using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public class ModalDialog : MonoBehaviour 
	{
		private List<GameObject> objectsToReEnable;

		void OnEnable()
		{
			//Create a list of games objects if it doesn't exist and clear it out.
			if (objectsToReEnable == null)
				objectsToReEnable = new List<GameObject>();

			objectsToReEnable.Clear();

			//Find all the toolbards and disable them.
			foreach (Toolbar tb in FindObjectsOfType<Toolbar>())
			{
				objectsToReEnable.Add(tb.gameObject);

				tb.gameObject.SetActive(false);
			}
		}

		void OnDisable()
		{
			//Re-enable all the objects that were disabled when this appeared.
			if (objectsToReEnable != null)
			{
				foreach (GameObject go in objectsToReEnable)
				{
					go.SetActive(true);
				}
			}
		}

		/// <summary>
		/// Disables this game object
		/// </summary>
		public void Close()
		{
			gameObject.SetActive(false);
		}
	}
}
