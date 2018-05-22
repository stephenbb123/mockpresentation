using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public class BuildingBlockLibrary : MonoBehaviour 
	{
		public BuildingBlock[] buildingBlocks;

		private static BuildingBlockLibrary instance;
		
		public static BuildingBlockLibrary Instance
		{
			get	
			{
				if (instance == null)
				{
					instance = FindObjectOfType<BuildingBlockLibrary>();
					
					if (instance == null)
					{
						instance = new GameObject("BuildingBlockLibrary", typeof(BuildingBlockLibrary)).GetComponent<BuildingBlockLibrary>();
					}
				}
				
				return instance;
			}
		}

		/// <summary>
		/// Create a building block.		/// </summary>
		/// <returns>A building block</returns>
		/// <param name="blockName">Name of the block to create</param>
		/// <param name="blockSubName">Sub name of the block to create</param>
		public BuildingBlock CreateBlock(string blockName, string blockSubName)
		{
			if (buildingBlocks == null)
				buildingBlocks = Resources.LoadAll<BuildingBlock>("EditorBlocks");
				
			foreach (BuildingBlock bb in buildingBlocks)
			{
				if (bb.buildingBlockName == blockName)
				{
					if (bb.buildingBlockSubName == blockSubName)
					{
						BuildingBlock newBlock = Instantiate(bb) as BuildingBlock;
						return newBlock;
					}
				}
			}

			Debug.LogError(string.Format("The block {0}, {1} could not be found in the building block library", blockName, blockSubName));

			return null;
		}

	}
}
