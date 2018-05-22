using UnityEngine;
using UnityEngine.Events;

using System.Collections;

namespace BumpkinLabs.IGE
{
	public class LevelLoader : MonoBehaviour 
	{
		public UnityEvent LevelLoaded;
		public bool enableMeshCombine = false;
		private bool loadLevel = true;
       

		public delegate void LevelLoadedEvent();
		public LevelLoadedEvent OnLevelLoaded;

		private static LevelLoader instance;

		private Vector3 min;
		private Vector3 max;
		private Vector3 centre;
		private float height;

		public static LevelLoader Instance
		{
			get
			{
				if (instance == null)
					instance = FindObjectOfType<LevelLoader>();

				return instance;
			}
		}

        public Vector3 Min
		{
			get { return min; }
		}

		public Vector3 Max
		{
			get { return max; }
		}

		public Vector3 Centre
		{
			get { return centre; }
		}

		public float Height
		{
			get { return height; }
		}

		

		void Update()
		{
			if (loadLevel)
			{
				loadLevel = false;
				LoadLevel();
			}
		}

		private void UpdateMinMax(Transform t)
		{
			min.x = Mathf.Min(min.x, t.position.x);
			min.y = Mathf.Min(min.y, t.position.y);
			min.z = Mathf.Min(min.z, t.position.z);

			max.x = Mathf.Max(max.x, t.position.x);
			max.y = Mathf.Max(max.y, t.position.y);
			max.z = Mathf.Max(max.z, t.position.z);
		}

		void LoadLevel()
		{
			bool firstPass = true;

			LevelData levelData = PersistentSceneData.Instance.LevelData;

			if (levelData != null)
			{
				foreach (BuildingBlockData bbd in levelData.BuildingBlocks)
				{
					bbd.CreateLevelBlock();
				}

				foreach (LevelBlock lb in FindObjectsOfType<LevelBlock>())
				{
					if (firstPass)
					{
						max = lb.transform.position;
						min = lb.transform.position;
						firstPass = false;
					}
					else
					{
						UpdateMinMax(lb.transform);
					}

					lb.gameObject.BroadcastMessage("LoadLevelCompleted", SendMessageOptions.DontRequireReceiver);
				}



				centre = min + ((max - min) / 2f);
				height = max.y - min.y;

				if (enableMeshCombine)
				{
					StaticModelCombiner smc = new StaticModelCombiner();
					smc.CombineModels();
				}
			}

			if (OnLevelLoaded != null)
				OnLevelLoaded();
				
			LevelLoaded.Invoke();

		}
	}
}
