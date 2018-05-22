using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Blocks/Level Block")]
	public class LevelBlock : MonoBehaviour 
	{
		//Name of the level block
		public string buildingBlockName = "";
		
		//Sub name of the block
		public string buildingBlockSubName = "";

		//The model object.
		public GameObject model;

		//force a rotation.
		public bool forceModelRotation;

		//rotation to use if forceModelRotation is true.
		public Vector3 forcedRotation;

		//randomize the rotation of the block
		public bool randomRotation = false;

		//Id for the building block.
		private string buildingBlockID = "";

		//The building blocks id.
		public string BuildingBlockID
		{
			get { return buildingBlockID; }
			set { buildingBlockID = value; }
		}

		public static LevelBlock FindByID(string id)
		{
			foreach (LevelBlock lb in FindObjectsOfType<LevelBlock>())
			{
				if (lb.BuildingBlockID == id)
					return lb;
			}

			return null;
		}

		private float RandomRotationAmount()
		{
			int r = Random.Range(0, 4);

			if (r == 0)
				return 0;

			if (r == 1)
				return 90;

			if (r == 2)
				return 180;

			if (r == 3)
				return 270;

			return -90;
		}

		//Called when the block is created.
		public void Setup()
		{
			if (forceModelRotation)
			{
				if (model != null)
					model.transform.rotation = Quaternion.Euler(forcedRotation);
			}

			if (randomRotation)
			{
				if (model != null)
					model.transform.Rotate(RandomRotationAmount(), RandomRotationAmount(), RandomRotationAmount());
			}
		}
	}
}
