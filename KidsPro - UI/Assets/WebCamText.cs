using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamText : MonoBehaviour {

	public GameObject webCameraPlane;
	// Use this for initialization
	void Start () {

		WebCamTexture webCameraTexture = new WebCamTexture ();
		webCameraPlane.GetComponent<MeshRenderer> ().material.mainTexture = webCameraTexture;
		webCameraTexture.Play ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
