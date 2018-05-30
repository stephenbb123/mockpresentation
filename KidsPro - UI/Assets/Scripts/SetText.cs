using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour {
    public Text randText;
    public string message;
    public int number;
    // Use this for initialization
    void Start () {
        number = Random.Range(1, 10);
        randText.text = "Objective:Odd Number-> Go up. Even number -> Go right.\nNumber: " + number;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ReGenerate()
    {
        randText.text = "Objective:Odd Number-> Go up. Even number -> Go right.\nNumber: " + Random.Range(1,10);
    }
    public string GetString()
    {
        message = randText.text;
        return message;
    }
    public void SetString(string txt)
    {
        randText.text = txt;
    }
    public int GetNum(){
        return number;
    }
}
