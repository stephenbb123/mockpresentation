using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTexture : MonoBehaviour {

    public Texture oldTexture;
    public Texture newTexture;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
<<<<<<< HEAD
            //gameObject.guiTexture = newTexture;
=======
           // gameObject.GetComponent<GUITexture>() = newTexture;
>>>>>>> c33bf63fcdcc60c7c55d85388cfb804773139cf1
        }
	}
}
