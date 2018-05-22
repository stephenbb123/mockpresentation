using UnityEngine;
using System.Collections;

namespace BumpkinLabs.IGE
{
	public class EditorLoader : MonoBehaviour 
	{
        private bool loadLevel = true;

       void Update()
        {
            if (loadLevel)
            {
                loadLevel = false;

                if (PersistentSceneData.Instance.FileName != "")
                {
                    LoadLevel();
                }

                PersistentSceneData.Instance.TestMode = false;
            }
        }
        

		private void LoadLevel()
		{
			LevelData levelData = LevelSerializer.LoadFromFile(PersistentSceneData.Instance.FileName);

			int blockCount = 0;
			
			//Remove the initial blocks if we are loading.
			if (levelData.BuildingBlocks.Count > 0)
			{
				LevelEditorManager.Instance.DeleteInitialBlocks();
			}

			foreach (BuildingBlockData bbd in levelData.BuildingBlocks)
			{
				blockCount++;
				bbd.CreateEditorBlock();
			}

			foreach (EditableBlock eb in FindObjectsOfType<EditableBlock>())
			{
				eb.SceneLoaded();
			}
			
			Vector3 pos = new Vector3(
				levelData.CameraPositionX, levelData.CameraPositionY, levelData.CameraPositionZ);
				
			Vector3 rotation = new Vector3(
				levelData.CameraRotationX, levelData.CameraRotationY, levelData.CameraRotationZ);
            
			CameraController cc = LevelEditorManager.Instance.editorCamera.gameObject.GetComponent<CameraController>();
			cc.CameraPosition = pos;
			
			cc.CameraRotation = rotation;
			
			cc.CameraDistance = levelData.CameraDistance;
		}
	}
}
