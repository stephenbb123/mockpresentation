using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using BumpkinLabs.IGE;

public class IGEEditorTools : EditorWindow
{
	[MenuItem("Tools/Bumpkin Labs/IGE/Convert To BuildingBlock")]
	public static void ConvertToBuildingBlock()
	{
		foreach (GameObject go in Selection.gameObjects)
		{
			ConvertGameObjectToBuildingBlock(go);
		}
	}

	[MenuItem("Tools/Bumpkin Labs/IGE/Convert BuildingBlock to LevelBlock")]
	public static void ConvertToLevelBlock()
	{
		foreach (GameObject go in Selection.gameObjects)
		{
			BuildingBlock bb = go.GetComponent<BuildingBlock>();

			if (bb != null)
				ConvertBuildingBlockToLevelBlock(bb);
		}
	}

	[MenuItem("Tools/Bumpkin Labs/IGE/Get Block ID")]
	public static void GetBlockID()
	{
		foreach (GameObject go in Selection.gameObjects)
		{
			bool found = false;

			BuildingBlock bb = go.GetComponent<BuildingBlock>();

			if (bb != null)
			{
				found = true;
				Debug.Log(string.Format("Object {0} ID = {1}", go.name, bb.BuildingBlockID));
			}

			LevelBlock lb = go.GetComponent<LevelBlock>();

			if (lb != null)
			{
				found = true;
				Debug.Log(string.Format("Object {0} ID = {1}", go.name, lb.BuildingBlockID));
			}

			if (!found)
			{
				Debug.Log(string.Format("The object {0} is not a building block", go.name));
			}
		}
	}
	
	[MenuItem("Tools/Bumpkin Labs/IGE/Convert Scene To Level Editor")]
	public static void ConvertSceneToLevelEditor()
	{
		ActionConvertSceneToLevelEditor();
	}
	
	[MenuItem("Tools/Bumpkin Labs/IGE/Add Level Loader")]
	public static void AddLevelLoader()
	{
		new GameObject("Level Loader", typeof(LevelLoader));
	}
	
	[MenuItem("Tools/Bumpkin Labs/IGE/Update Connectors")]
	public static void UpdateConnectors()
	{
		if (EditorUtility.DisplayDialog("Update Connectors", "This function is destructive and will alter prefabs in your project.\n\nPlease back up your work before running", "OK", "Cancel"))
		{
			//Get the connector in the current selection.
			bool wasValid = false;
			
			for (int i = 0; i < Selection.objects.Length; ++i)
			{
				Object pObj = PrefabUtility.GetPrefabObject(Selection.objects[i]);
				
				if (pObj != null)
				{
					string prefabPath = AssetDatabase.GetAssetPath(pObj);
					
					GameObject go = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
					
					GameObject instance = Instantiate(go) as GameObject;
					
					BuildingBlockConnector connector = instance.GetComponent<BuildingBlockConnector>();
					
					if (connector != null)
					{
						wasValid = true;
						UpdateAllConnectors(go);
					}
					
					DestroyImmediate(instance);
				}
			}
			
			if (!wasValid)
			{
				Debug.LogError("Could not update connectors.  Ensure the connector you want to use is selected");
			}
		}
	}
	
	private static void UpdateAllConnectors(GameObject newConnectorPrefab)
	{
		if (newConnectorPrefab != null)
		{
			foreach (string s in AssetDatabase.FindAssets("t:gameobject"))
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(s);
				
				if (assetPath.EndsWith(".prefab"))
				{
					GameObject asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
					
					if (asset != newConnectorPrefab)
					{
						if (asset != null)
						{
							bool adjusted = false;
							GameObject assetInstance = Instantiate(asset) as GameObject;
							
							assetInstance.transform.position = Vector3.zero;
							assetInstance.transform.rotation = Quaternion.identity;
							
							BuildingBlock buildingBlock = assetInstance.GetComponent<BuildingBlock>();
							
							if (buildingBlock != null)
							{
								foreach (BuildingBlockConnector currentConnector in assetInstance.GetComponentsInChildren<BuildingBlockConnector>())
								{
									if (!currentConnector.disableAutomatedUpdate)
									{
										GameObject newConnector = Instantiate(newConnectorPrefab);
										newConnector.transform.position = currentConnector.transform.position;
										newConnector.transform.rotation = currentConnector.transform.rotation;
										
										newConnector.transform.parent = currentConnector.transform.parent;
										
										BuildingBlockConnector nConnector = newConnector.GetComponent<BuildingBlockConnector>();
										nConnector.useInPlacementMode = currentConnector.useInPlacementMode;
										nConnector.useInSceneMode = currentConnector.useInSceneMode;
										nConnector.allowRoation = currentConnector.allowRoation;
										
										newConnector.name = currentConnector.gameObject.name;
										
										DestroyImmediate(currentConnector.gameObject);
										
										adjusted = true;
									}
								}
								
								if (adjusted)
								{
									Object prefabObject = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
									PrefabUtility.ReplacePrefab(assetInstance, prefabObject, ReplacePrefabOptions.ReplaceNameBased);
								}
							}
							
							DestroyImmediate(assetInstance);
						}
					}
				}
			}
		}
	}
	

	private static void ConvertBuildingBlockToLevelBlock(BuildingBlock buildingBlock)
	{
		//Add the level block component.
		LevelBlock levelBlock = buildingBlock.gameObject.AddComponent<LevelBlock>();
		levelBlock.buildingBlockName = buildingBlock.buildingBlockName;
		levelBlock.buildingBlockSubName = buildingBlock.buildingBlockSubName;

		DetailedMeshCollider detailedMeshCollider = buildingBlock.blockMesh;

		if (detailedMeshCollider != null)
			DestroyImmediate(detailedMeshCollider.gameObject);

		BuildingBlockConnector connector = buildingBlock.gameObject.GetComponentInChildren<BuildingBlockConnector>();

		if (connector != null)
		{
			Transform parent = connector.transform.parent;
			if (parent != null)
				DestroyImmediate(parent.gameObject);
		}

		DestroyImmediate(buildingBlock);
	}

	private static bool CanGameObjectBeConvertedToBuildingBlock(GameObject go)
	{
		bool rv = true;

		MeshFilter mf = go.GetComponent<MeshFilter>();

		if (mf == null)
		{
			Debug.Log(string.Format("The object {0} does not contain a mesh filter and cannot be converted using this tool", go.name));
			rv = false;
		}

		MeshRenderer mr = go.GetComponent<MeshRenderer>();

		if (mr == null)
		{
			if (mf == null)
			{
				Debug.Log(string.Format("The object {0} does not contain a mesh renderer and cannot be converted using this tool", go.name));
				rv = false;
			}
		}

		return rv;
	}

	private static void ConvertGameObjectToBuildingBlock(GameObject go)
	{
		BuildingBlock newBuildingBlock;
		DetailedMeshCollider detailedMeshCollider;

		if (CanGameObjectBeConvertedToBuildingBlock(go))
		{
			newBuildingBlock = new GameObject(go.name, typeof(BuildingBlock)).GetComponent<BuildingBlock>();
			newBuildingBlock.transform.position = go.transform.position;
			go.transform.parent = newBuildingBlock.transform;

			newBuildingBlock.blockModel = go;

			detailedMeshCollider = new GameObject("Detailed Mesh Collider", typeof(DetailedMeshCollider)).GetComponent<DetailedMeshCollider>();
			detailedMeshCollider.transform.position = go.transform.position;
			detailedMeshCollider.transform.parent = newBuildingBlock.transform;
			detailedMeshCollider.transform.localScale = go.transform.localScale;
			detailedMeshCollider.transform.rotation = go.transform.rotation;

			MeshFilter mf = go.GetComponent<MeshFilter>();
			MeshCollider mc = detailedMeshCollider.GetComponent<MeshCollider>();

			mc.sharedMesh = mf.sharedMesh;

			newBuildingBlock.blockMesh = detailedMeshCollider;

			newBuildingBlock.buildingBlockName = go.name;
			newBuildingBlock.buildingBlockSubName = string.Format("{0} subname", go.name);

			GameObject connectorsObject = new GameObject("Connectors");
			connectorsObject.transform.position = go.transform.position;
			connectorsObject.transform.parent = newBuildingBlock.transform;

			BoxCollider bc = go.GetComponent<BoxCollider>();

			if (bc != null)
				DestroyImmediate(bc);

			go.name = "Model";
			detailedMeshCollider.gameObject.name = "Collider";
			
			//Create connectors
			GameObject connectorPrefab = AssetDatabase.LoadAssetAtPath("Assets/BumpkinLabs/BuildingBlockEditor/BuildingBlocks/Prefabs/Connector.prefab", typeof(GameObject)) as GameObject;
			GameObject connector = Instantiate<GameObject>(connectorPrefab);
			connector.name = "Connector";
			connector.transform.parent = connectorsObject.transform;
			connector.transform.localPosition = Vector3.zero;
		}
	}
	
	private static void ActionConvertSceneToLevelEditor()
	{
		//Create a parent object to keep the scene tidy.
		GameObject parent = new GameObject("Level Editor Components");
		
		//Add the level editor manager.
		LevelEditorManager levelEditorManager = new GameObject("Level Editor Manager", typeof(LevelEditorManager)).GetComponent<LevelEditorManager>();
		
		//Add an orbit camera to the camera.
		Camera cam = FindObjectOfType<Camera>();
		
		if (cam == null)
		{
			//there is no camera in the scene - let's add one.
			cam = new GameObject("Main Camera", typeof(Camera)).GetComponent<Camera>();
		}
		
		//If there is no camera controller on the camera, add one.
		CameraController cameraController = cam.GetComponent<CameraController>();
		
		if (cameraController == null)
			cam.gameObject.AddComponent<BumpkinLabs.IGE.CameraController>();
		
		//Set the camera on the level editor
		levelEditorManager.editorCamera = cam;
		
		//Add the Add Block mode
		AddBlockMode addBlockMode = new GameObject("Add Block Mode", typeof(AddBlockMode)).GetComponent<AddBlockMode>();
		addBlockMode.raycastingCamera = cam;
		addBlockMode.transform.parent = parent.transform;
		
		//Add add free mode.
		AddFreeMode addFreeMode = new GameObject("Add Free Mode", typeof(AddFreeMode)).GetComponent<AddFreeMode>();
		addFreeMode.raycastingCamera = cam;
		addFreeMode.transform.parent = parent.transform;
		
		//Add delete block mode
		DeleteBlockMode deleteBlockMode = new GameObject("Delete Block Mode", typeof(DeleteBlockMode)).GetComponent<DeleteBlockMode>();
		deleteBlockMode.transform.parent = parent.transform;
		
		//Add select block mode.
		SelectBlockMode selectBlockMode = new GameObject("Select Block Mode", typeof(SelectBlockMode)).GetComponent<SelectBlockMode>();
		selectBlockMode.raycastingCamera = cam;
		selectBlockMode.transform.parent = parent.transform;
		
		//Add editing object mode
		EditingObjectMode editingObjectMode = new GameObject("Editing Object Mode", typeof(EditingObjectMode)).GetComponent<EditingObjectMode>();
		editingObjectMode.transform.parent = parent.transform;
		
		//Add the editor loader.
		EditorLoader editorLoader = new GameObject("Editor Loader", typeof(EditorLoader)).GetComponent<EditorLoader>();
		editorLoader.transform.parent = parent.transform;
		
		//Add the mouse editor
		new GameObject("Mouse Controller", typeof(IGEMouseController)).transform.parent = parent.transform;
		
		//Load base materials
		Material potentialPlacementMaterial = AssetDatabase.LoadAssetAtPath("Assets/BumpkinLabs/BuildingBlockEditor/Materials/Potential Placement.mat", typeof(Material)) as Material;
		Material potentialPlacementBlockedMaterial = AssetDatabase.LoadAssetAtPath("Assets/BumpkinLabs/BuildingBlockEditor/Materials/Potential Placement Blocked.mat", typeof(Material)) as Material;
		Material highlightToDeleteMaterial = AssetDatabase.LoadAssetAtPath("Assets/BumpkinLabs/BuildingBlockEditor/Materials/Hightlight To Delete.mat", typeof(Material)) as Material;
		Material highlightToEditMaterial = AssetDatabase.LoadAssetAtPath("Assets/BumpkinLabs/BuildingBlockEditor/Materials/Highlight To Edit.mat", typeof(Material)) as Material;
		
		//Assign base materials to editor.
		if (potentialPlacementMaterial != null)
			levelEditorManager.potentialPlacementMaterial = potentialPlacementMaterial;
			
		if (potentialPlacementBlockedMaterial != null)
			levelEditorManager.potenialPlacementBlockedMaterial = potentialPlacementBlockedMaterial;
			
		if (highlightToDeleteMaterial != null)
			levelEditorManager.deletionHighlightMaterial = highlightToDeleteMaterial;
			
		if (highlightToEditMaterial != null)
			levelEditorManager.editHighlightMaterial = highlightToEditMaterial;
		
		
		//Create a default block.
		GameObject defaultCubePrefab = AssetDatabase.LoadAssetAtPath("Assets/BumpkinLabs/BuildingBlockEditor/Resources/EditorBlocks/Default Cube EDT.prefab", typeof(GameObject)) as GameObject;
		
		BuildingBlock builidngBlock = Instantiate<GameObject>(defaultCubePrefab).GetComponent<BuildingBlock>();
		builidngBlock.transform.position = Vector3.zero;
		
		List<BuildingBlock> initialBlocks = new List<BuildingBlock>();
		initialBlocks.Add(builidngBlock);
		
		levelEditorManager.initialBlocks = initialBlocks.ToArray();
	}
	
	
	
}
