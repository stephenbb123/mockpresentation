using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	public class EditableBlock : MonoBehaviour 
	{
		private string metaData;

		public string MetaData
		{
			get
			{
				return metaData;
			}
			set
			{
				metaData = value;
			}
		}

		/// <summary>
		/// Called when the block is selected for editing.
		/// </summary>
		public virtual void EnterEditMode() { }

		/// <summary>
		/// Called when the edit mode changes.
		/// </summary>
		public virtual void ExitEditMode() { }

		/// <summary>
		/// Called when the block is added to the scene.
		/// </summary>
		public virtual void AddedToScene() { }

		/// <summary>
		/// Called before saving
		/// </summary>
		public virtual void PrepareForSave() { }

		/// <summary>
		/// Called when the edtor scene is loaded.
		/// </summary>
		public virtual void SceneLoaded() { }
	}
}
