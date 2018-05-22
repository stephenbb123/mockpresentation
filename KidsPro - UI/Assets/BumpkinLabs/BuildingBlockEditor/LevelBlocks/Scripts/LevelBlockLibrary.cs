using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	public class LevelBlockLibrary : MonoBehaviour 
	{
		public LevelBlock[] levelBlocks;

		private static LevelBlockLibrary instance;
		
		public static LevelBlockLibrary Instance
		{
			get	
			{
				if (instance == null)
				{
					instance = FindObjectOfType<LevelBlockLibrary>();
					
					if (instance == null)
					{
						instance = new GameObject("LevelBlockLibrary", typeof(LevelBlockLibrary)).GetComponent<LevelBlockLibrary>();
					}
				}
				
				return instance;
			}
		}

		public LevelBlock CreateBlock(string blockName, string blockSubName)
		{
			if (levelBlocks == null)
				levelBlocks = Resources.LoadAll<LevelBlock>("LevelBlocks");
			
			foreach (LevelBlock lb in levelBlocks)
			{
				if (lb == null)
					Debug.Log("It's null");

				if (lb.buildingBlockName == blockName)
				{
					if (lb.buildingBlockSubName == blockSubName)
					{
						LevelBlock newBlock = Instantiate(lb) as LevelBlock;
						return newBlock;
					}
				}
			}
			
			Debug.LogError(string.Format("The block {0}, {1} could not be found in the level block library"));
			
			return null;
		}
	}
}