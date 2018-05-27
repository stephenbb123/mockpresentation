using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public Text bottomText;
	public Text clearMenuText;
	public Text menuButtonText;
	public GameObject completedMenu;
	public OrbitalCamera orbitalCamera;
	
	private bool listenForEscape = false;
	private int numberOfCollectables = 0;
	private static GameManager instance;
	private float totalTime;
	private bool timerActive = false;
	
	/// <summary>
	/// Gets the instance of the GameManager.
	/// </summary>
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<GameManager>();
				
			return instance;
		}
	}
	
	void Start () 
	{
		//Change items based on if the level is being played for testing or not.
		if (!BumpkinLabs.IGE.PersistentSceneData.Instance.TestMode)
		{
			bottomText.text = "use mouse to rotate, arrow keys to tilt\nescape to return to menu";
			listenForEscape = true;
		}
		else
		{
			menuButtonText.text = "Level Editor";
		}
	}
	
	void Update()
	{
		if (listenForEscape) //if we run from the level editor we're using the 'return on escape' property of LevelEditorManager.  
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
                UnityEngine.SceneManagement.SceneManager.LoadScene("BBExMenu");
			}
		}
		
		//Increment time.
		if (timerActive)
			totalTime += Time.deltaTime;
	}
	
	//User pressed the restart button
	public void RestartClicked()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
	
	/*//User pressed the menu button
	public void MenuClicked()
	{
		if (BumpkinLabs.IGE.PersistentSceneData.Instance.TestMode)
		{
			//If we're in test mode, reload the level editor.
			Application.LoadLevel("BBExLevelEditor");
		}
		else
		{
			//If we're in play mode load the menu
			Application.LoadLevel("BBExMenu");
		}
	}
	*/

	//Add a collectable to the collection.
	public void AddCollectable()
	{
		numberOfCollectables++;
	}
	
	//Remove a collectable
	public void RemoveCollectable()
	{
		numberOfCollectables--;
		
		//If we've collected everything finish the game.
		if (numberOfCollectables < 1)
		{
			timerActive = false;
			orbitalCamera.StopAffectingGravity();
			completedMenu.SetActive(true);
			
			clearMenuText.text = string.Format("Clear Time: {0} seconds", totalTime.ToString("0.00"));
		}
	}
	
	//This is called by an event on the LevelLoader object.
	public void LevelLoaded()
	{
		timerActive = true;
	}
	
	
}
