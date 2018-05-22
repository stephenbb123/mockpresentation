using UnityEngine;
using System.Collections;

public class ToolbarManager : MonoBehaviour 
{
	public GameObject mainToolbar;
	public GameObject createToolbar;
	
	/// <summary>
	/// Shows the main toolbar.
	/// </summary>
	public void ShowMainToolbar()
	{
		createToolbar.SetActive(false);
		mainToolbar.SetActive(true);
	}
	
	/// <summary>
	/// Shows the create toolbar.
	/// </summary>
	public void ShowCreateToolbar()
	{
		createToolbar.SetActive(true);
		mainToolbar.SetActive(false);
	}
	
	public void ReturnToMenu()
	{
		Application.LoadLevel("MainMenu");
	}
}
