using UnityEngine;
using System.Collections;

/// <summary>
///  Mono behaviour that listens for an escape press in the main game allowing you to easily return to the editor.
/// </summary>
public class ReturnToEditorStub : MonoBehaviour 
{
	public string editorSceneName = "";

	private bool listening = false;

	public static void Create(bool editSceneLoading = true)
	{
		ReturnToEditorStub newObject = new GameObject("Return To Editor Stub", typeof(ReturnToEditorStub)).GetComponent<ReturnToEditorStub>();
		newObject.editorSceneName = Application.loadedLevelName;
		newObject.Init(editSceneLoading);
	}
    

    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
      

        if (Application.loadedLevelName == editorSceneName)
        {
            //We've just re-entered the editor.
            listening = false;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            Destroy(gameObject);
        }
        else
        {
            //We've entered the scene - start listening to escape.
            listening = true;
        }
    }

    public void Init(bool editScreenLoading = true)
	{
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        DontDestroyOnLoad(gameObject);

        //Scene loading messages can take a frame to start listening so this
        //bool enables us to start listening instantly,
        if (editScreenLoading)
        {
            listening = true;
        }
    }
    

	void Update()
	{
		if (listening)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
                UnityEngine.SceneManagement.SceneManager.LoadScene(editorSceneName);
			}
		}
	}
}
