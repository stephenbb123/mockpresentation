using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour 
{
	private Vector3 endPoint;
	private Vector3 startPoint;
	
	private const float delay = 0.5f;
	
	//SetMetaData is called on every script attached to a LevelBlock as the level is being loaded.
	private void SetMetaData(string metaData)
	{
		endPoint = BumpkinLabs.IGE.MetaDataHelper.ToVector3(metaData);
	}
	
	//LoadLevelCompleted is called on every script attached to a LevelBlock when the level has completed loading.
	private void LoadLevelCompleted()
	{
		//Remember where we started
		startPoint = transform.position;
		
		//Start the coroutine that does the movement
		StartCoroutine(MovePlatformLoop());
	}
	
	IEnumerator MovePlatformLoop()
	{
		float distanceToEnd;
		
		while(true)
		{
			//Start to end point.
			
			//Delay.
			yield return new WaitForSeconds(delay);
			
			distanceToEnd = 100;
			
			while(distanceToEnd > 0.1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, endPoint, Time.deltaTime);
				distanceToEnd = Vector3.Distance(transform.position, endPoint);	
				yield return new WaitForEndOfFrame();
			}
			
			//End to start point.
			
			//Delay.
			yield return new WaitForSeconds(delay);
			
			distanceToEnd = 100;
			
			while(distanceToEnd > 0.1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, startPoint, Time.deltaTime);
				distanceToEnd = Vector3.Distance(transform.position, startPoint);	
				yield return new WaitForEndOfFrame();
			}
			
		}
	}
	
}
