using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour 
{
	public GameObject ballPrefab;
	
	//LoadLevelCompleted will be called on all scripts attached to a LevelBlock.
	private void LoadLevelCompleted()
	{
		//Create the new object.
		GameObject newGameObject = Instantiate<GameObject>(ballPrefab);
		newGameObject.transform.position = transform.position;
		
		//Control it with the orbital camera.
		OrbitalCamera.Instance.SetFollowObject(newGameObject.transform);
	}
}
