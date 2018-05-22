using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public TextAsset demoLevelTextAsset;
	
	//User pressed play demo level
	public void PlayDemoLevel()
	{
		//Set the persistent scene data instance's text asset to the level we want to play.
		BumpkinLabs.IGE.PersistentSceneData.Instance.TextAsset = demoLevelTextAsset;
		Application.LoadLevel("BBExLevelPlayer");
	}
	
	//User pressed level editor
	public void StartLevelEditor()
	{
		Application.LoadLevel("BBExLevelEditor");
	}
	
	public void Quit()
	{
		Application.Quit();
	}
}
