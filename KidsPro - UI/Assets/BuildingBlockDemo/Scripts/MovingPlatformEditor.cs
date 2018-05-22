using UnityEngine;
using System.Collections;
using BumpkinLabs.IGE;

public class MovingPlatformEditor : EditableBlock
{
	public GameObject canvas;
	public GameObject endPoint;
	
	//Will be called when the block is clicked on when we're in edit mode.
	public override void EnterEditMode ()
	{
		//Enable the canvas containing the move controls.
		canvas.SetActive(true);
		base.EnterEditMode ();
	}
	
	//Will be called when the edit mode changes.
	public override void ExitEditMode ()
	{
		//Disable the canvas containing the move controls.
		canvas.SetActive(false);
		base.ExitEditMode ();
	}
	
	//Called on all blocks before the level is saved.
	public override void PrepareForSave ()
	{
		//Set the MetaData property as a serialized version of the position.
		MetaData = MetaDataHelper.FromVector3(endPoint.transform.position);
		base.PrepareForSave ();
	}
	
	//Called when the scene is loaded.
	public override void SceneLoaded ()
	{
		//Set the end point in the correct position.
		Vector3 v = MetaDataHelper.ToVector3(MetaData);
		endPoint.transform.position = v;
		base.SceneLoaded ();
	}
	
	/// <summary>
	/// Alters the position x.
	/// </summary>
	public void AlterPositionX(float amount)
	{
		endPoint.transform.position = endPoint.transform.position + endPoint.transform.right * amount;
	}
	
	/// <summary>
	/// Alters the position z.
	/// </summary>
	public void AlterPositionZ(float amount)
	{
		endPoint.transform.position = endPoint.transform.position + endPoint.transform.up * amount;
	}
}
