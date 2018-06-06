using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetText : MonoBehaviour {
    public Text randText;
    public string message;
    public int number;
    // Use this for initialization
    void Start () {
        Scene currentScene = SceneManager.GetActiveScene();

        switch(currentScene.name){
            case "Level1":
                randText.text = "Go to the position of the white flag .";
                break;
            case "Level2":
                randText.text = "Objective: Collect 3 Flags and go to the end point";
                break;
            case "Level3":
                number = Random.Range(1, 10);
                randText.text = "Odd Number-> Go up. Even number -> Go right.\nNumber: ";
                break;
        }
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Generate(){
        randText.text = "Objective:Odd Number-> Go up. Even number -> Go right.\nNumber: "+number;
    }
    public void ReGenerate()
    {
        number= Random.Range(1,10);
        Generate();
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
    public void Clear()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        switch (currentScene.name)
        {
            case "Level1":
                randText.text = "Go to the position of the white flag .";
                break;
            case "Level2":
                randText.text = "Objective: Collect 3 Flags and go to the end point";
                break;
            case "Level3":
                number = Random.Range(1, 10);
                randText.text = "Odd Number-> Go up. Even number -> Go right.\nNumber: ";
                break;
        }
    }
}
