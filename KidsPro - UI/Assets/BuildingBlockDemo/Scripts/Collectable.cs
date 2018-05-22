using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Collectable : MonoBehaviour 
{
	public ParticleSystem pSystem;
	public GameObject collectableObject;
	
	private AudioSource audioSource;
	
	void Start()
	{
		//Add this collectable to the colection, so we know how many remain to be picked up.
		GameManager.Instance.AddCollectable();
		
		//Get a handle of the audio source for later.
		audioSource = GetComponent<AudioSource>();
	}
	
	//Called when the ball collides with the trigger.
	public void Collected()
	{
		//Play the audio.
		audioSource.Play();
		
		//Destroy the model / trigger.
		Destroy(collectableObject);
		
		//Play the particle effect. 
		pSystem.Play();
		
		//Remove a collectable from the collection.
		GameManager.Instance.RemoveCollectable();
	}
}
