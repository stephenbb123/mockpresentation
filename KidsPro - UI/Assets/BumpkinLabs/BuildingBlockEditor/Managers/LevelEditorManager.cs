using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BumpkinLabs.IGE
{
	[AddComponentMenu("IGE/Scene/Level Editor Manager")]
	public class LevelEditorManager : MonoBehaviour 
	{
        
		//If the scene starts with blocks in place then put them here.
		public BuildingBlock[] initialBlocks;

		//The material shown when a block is placed but not added.
		public Material potentialPlacementMaterial;

		//The material shown when a block is placed but cannot be added.
		public Material potenialPlacementBlockedMaterial;

		//The material shown when a block is hovered over for deletion.
		public Material deletionHighlightMaterial;
		
		//The material shown when a block is hovered over for deletion.
		public Material editHighlightMaterial;

		//Camera used for editing.
		public Camera editorCamera;

		//If true then you can switch back to the editor from the scene with the escape key.
		public bool returnFromTestOnEscape = true;

		//Current mode
		private EditMode currentMode;

		public delegate void ModeChanged(EditMode newMode);
		public event ModeChanged OnModeChanged;

		private static LevelEditorManager instance;
		
		public static LevelEditorManager Instance
		{
			get	
			{
				if (instance == null)
				{
					instance = FindObjectOfType<LevelEditorManager>();
					
					if (instance == null)
					{
						instance = new GameObject("LevelEditorManager", typeof(LevelEditorManager)).GetComponent<LevelEditorManager>();
					}
				}
				
				return instance;
			}
		}

		private void TriggerModeChangedEvent()
		{
			if (OnModeChanged != null)
				OnModeChanged(currentMode);
		}

		void Awake()
		{

            //Setup initial blocks.
            if (initialBlocks != null)
            {
                foreach (BuildingBlock bb in initialBlocks)
                {
                    bb.AddToScene();
                }
            }
		}

		/// <summary>
		/// Enter move block mode
		/// </summary>
		/// <param name="buildingBlock">The block to move</param>
		public void EnterAddBlockForMovingMode(BuildingBlock buildingBlock)
		{
			if (buildingBlock == null)
			{
				Debug.LogError("Enter Add Block Mode called with null");
			}

			if (currentMode != null)
				currentMode.DisableMode();

			if (buildingBlock.allowDecorativePlacement)
			{
				AddFreeMode.Instance.SetCurrentMode(AddFreeMode.AddFreeModes.MovingBlocks);
				currentMode = AddFreeMode.Instance;
			}
			else
			{
				AddBlockMode.Instance.SetMode(AddBlockMode.AddBlockModes.MovingBlock);
				currentMode = AddBlockMode.Instance;
			}

			currentMode.EnableMode(buildingBlock.gameObject);

			TriggerModeChangedEvent();
		}

		/// <summary>
		/// Enters the add block mode.
		/// </summary>
		/// <param name="buildingBlock">Building block you want to add</param>
		public void EnterAddBlockMode(Object buildingBlock)
		{
			if (buildingBlock == null)
			{
				Debug.LogError("Enter Add Block Mode called with null");
			}

			GameObject go = buildingBlock as GameObject;
            
			//Cast the arg as a building block and create a new instance of it.
			BuildingBlock bb = go.GetComponent<BuildingBlock>();
            

			//Disable the current mode.
			if (currentMode != null)
				currentMode.DisableMode();

			//Set the relivent mode based on connector placement.
			if (bb.allowDecorativePlacement)
			{
				AddFreeMode.Instance.SetCurrentMode(AddFreeMode.AddFreeModes.CreatingBlocks);
				currentMode = AddFreeMode.Instance;
			}
			else
			{
				AddBlockMode.Instance.SetMode(AddBlockMode.AddBlockModes.CreatingBlocks);
				currentMode = AddBlockMode.Instance;
			}

			//Enable the mode.
			currentMode.EnableMode(buildingBlock);

			TriggerModeChangedEvent();
		}

		/// <summary>
		/// Enters the delete block mode.
		/// </summary>
		public void EnterDeleteBlockMode()
		{
			if (currentMode != null)
				currentMode.DisableMode();

			currentMode = DeleteBlockMode.Instance;
			currentMode.EnableMode(null);
			TriggerModeChangedEvent();
		}

		/// <summary>
		/// Enters the select block mode.
		/// </summary>
		public void EnterSelectBlockMode()
		{
			if (currentMode != null)
				currentMode.DisableMode();

			SelectBlockMode.Instance.SetCurrentMode(SelectBlockMode.SelectBlockModes.SelectForEditing);
			currentMode = SelectBlockMode.Instance;
			currentMode.EnableMode(null);
			TriggerModeChangedEvent();
		}

		/// <summary>
		/// Enters the editing object mode.
		/// </summary>
		/// <param name="obj">Object being edited</param>
		public void EnterEditingObjectMode(Object obj)
		{
			if (currentMode != null)
				currentMode.DisableMode();

			currentMode = EditingObjectMode.Instance;
			currentMode.EnableMode(obj);
			TriggerModeChangedEvent();
		}

		/// <summary>
		/// Enters the select block for moving mode.
		/// </summary>
		public void EnterSelectBlockForMovingMode()
		{
			if (currentMode != null)
				currentMode.DisableMode();

			SelectBlockMode.Instance.SetCurrentMode(SelectBlockMode.SelectBlockModes.SelectForMoving);
			currentMode = SelectBlockMode.Instance;
			currentMode.EnableMode(null);
			TriggerModeChangedEvent();
		}

		/// <summary>
		/// Saves the level.
		/// </summary>
		/// <param name="fileName">File name to save the level as</param>
		public void SaveLevel(string fileName)
		{
			LevelSerializer.SaveLevelToFile(LevelSerializer.CreateFullFileName(fileName));
		}

		/// <summary>
		/// Loads the file.
		/// </summary>
		/// <param name="fileName">File to load</param>
		public void LoadFile(string fileName, string editorSceneName)
		{
			PersistentSceneData.Instance.FileName = LevelSerializer.CreateFullFileName(fileName);
			Application.LoadLevel(editorSceneName);
		}

		public void DeleteInitialBlocks()
		{
            if (initialBlocks != null)
            {
                foreach (BuildingBlock bb in initialBlocks)
                {
                    Destroy(bb.gameObject);
                }
            }
		}

		/// <summary>
		/// Load the test scene
		/// </summary>
		public void TestLevel(string levelSceneName)
		{
			//Create a return to editor stub if required
			if (returnFromTestOnEscape)
				ReturnToEditorStub.Create();

			string fileName = LevelSerializer.CreateFullFileName("tempsave.txt");

			LevelSerializer.SaveLevelToFile(fileName);
			PersistentSceneData.Instance.FileName = fileName;
			
			//Mark the TestMode flag - this gets unset my EditorLoader.
			PersistentSceneData.Instance.TestMode = true;
			
			Application.LoadLevel(levelSceneName);
		}

		public void EnableObject(GameObject objectToEnable)
		{
			objectToEnable.SetActive(true);
		}

		public void DisableCurrentMode()
		{
			if (currentMode != null)
				currentMode.DisableMode();

			currentMode = null;

			TriggerModeChangedEvent();
		}


	}
}
