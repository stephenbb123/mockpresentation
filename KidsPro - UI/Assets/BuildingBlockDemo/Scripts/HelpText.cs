using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class HelpText : MonoBehaviour 
{	
	public string baseStaticText = "";
	
	private string hoverText = "";
	private string staticText = "";
	private Text text;
	
	private static HelpText instance;
	
	/// <summary>
	/// Gets the instance of help text
	/// </summary>
	public static HelpText Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<HelpText>();
				
			return instance;
		}
	}
	
	void Start () 
	{
		//Get the text object and set the initil static text
		text = GetComponent<Text>();
		staticText = baseStaticText;
	}
	
	
	void Update () 
	{
		//Set the text as hover text.
		text.text = hoverText;
		
		//If hover text is "" then show static text instead.
		if (hoverText == "")
			text.text = staticText;
	}
	
	/// <summary>
	/// Set the hover text.
	/// </summary>
	public void SetHoverText(string s)
	{
		hoverText = s;
	}
	
	/// <summary>
	/// Set the text that will be shown if hover text is ""
	/// </summary>
	public void SetStaticText(string s)
	{
		staticText = s;
	}
	
	/// <summary>
	/// Resets the base static text.
	/// </summary>
	public void ResetBaseStaticText()
	{
		staticText = baseStaticText;
	}
}
