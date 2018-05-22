using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
public class TestingLoader : MonoBehaviour 
	{
		public bool isEnabled = true;
		public string fileName = "";
		public TextAsset textAsset;

		// Use this for initialization
		void Start () 
		{
			if (isEnabled)
			{
				if (!PersistentSceneData.Instance.HasData)
				{
					if (textAsset == null)
					{
						if (System.IO.File.Exists(fileName))
						{
							PersistentSceneData.Instance.FileName = fileName;
							Application.LoadLevel(Application.loadedLevel);
						}
					}
					else
					{
						PersistentSceneData.Instance.TextAsset = textAsset;
						Application.LoadLevel(Application.loadedLevel);
					}
				}
			}
		}
	}
}