using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPlane : MonoBehaviour {

	public GameObject webcameraPlane;

	// Use this for initialization
	void Start () {
		WebCamTexture webCameraTexture = new WebCamTexture ();
		webcameraPlane.GetComponent<MeshRenderer> ().material.mainTexture = webCameraTexture;
		webCameraTexture.Play ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
