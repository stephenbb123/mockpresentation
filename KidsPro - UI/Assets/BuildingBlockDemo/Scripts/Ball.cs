using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour 
{
	private AudioSource audioSource;
	
	void Start()
	{
		audioSource = GetComponent<AudioSource>();	
	}
	
	void OnTriggerEnter(Collider c)
	{
		//If the game objects tag == collectable then call the Collected method on the collectable.
		if (c.gameObject.tag == "Collectable")
		{
			Collectable col = c.gameObject.GetComponentInParent<Collectable>();
			col.Collected();
		}
	}
	
	void OnCollisionEnter(Collision c)
	{
		//If we have hit something hard make a noise.
		float s = c.relativeVelocity.magnitude;
		
		if (s > 5)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
	}
}
