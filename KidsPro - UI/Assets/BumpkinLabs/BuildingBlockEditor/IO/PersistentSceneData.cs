using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public class PersistentSceneData : MonoBehaviour 
	{
		private string fileName = "";
		private TextAsset textAsset = null;
		private bool testMode = false;

		private static PersistentSceneData instance;

		/// <summary>
		/// Gets the instance
		/// </summary>
		public static PersistentSceneData Instance
		{
			get	
			{
				if (instance == null)
				{
					instance = FindObjectOfType<PersistentSceneData>();
					
					if (instance == null)
					{
						instance = new GameObject("PersistentSceneData", typeof(PersistentSceneData)).GetComponent<PersistentSceneData>();
					}
				}
				
				return instance;
			}
		}
		
		public string FileName
		{
			get { return fileName;}
			set
			{
				fileName = value;
				
				//Ensure that any current text asset is cleared.
				if (textAsset != null)
					textAsset = null;
			}
		}
		
		public TextAsset TextAsset
		{
			get { return textAsset; }
			set
			{
				textAsset = value;
				
				//Ensure that any current filename is cleared.
				fileName = "";
			}
		}

		/// <summary>
		/// Clear any level data
		/// </summary>
		public void Clear()
		{
			fileName = string.Empty;
			textAsset = null;
		}

		/// <summary>
		/// Returns true if level data is present.
		/// </summary>
		public bool HasData
		{
			get
			{
				if (fileName != "")
					return true;

				if (textAsset != null)
					return true;

				return false;
			}
		}

		/// <summary>
		/// The current level data
		/// </summary>
		public BumpkinLabs.IGE.LevelData LevelData
		{
			get
			{
				if (fileName != null)
				{
					if (System.IO.File.Exists(fileName))
						return BumpkinLabs.IGE.LevelSerializer.LoadFromFile(fileName);
				}

				if (textAsset != null)
				{
					return BumpkinLabs.IGE.LevelSerializer.CreateFromTextAsset(textAsset);
				}

				return null;
			}
		}
		
		public bool TestMode
		{
			get { return testMode; }
			set { testMode = value; }
		}

		void Start()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
