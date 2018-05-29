using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour {
    public Text randText;

    // Use this for initialization
    void Start () {
        randText.text = "Objective:Odd Number-> Go up. Even number -> Go left.\nNumber: " + Random.Range(1, 10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ReGenerate()
    {
        randText.text = "" + Random.Range(1, 10);
    }
    public string GetText()
    {
        return randText.text;
    }
    public void SetString(string txt)
    {
        randText.text = txt;
    }
}
