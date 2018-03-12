using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SwitchScenes : MonoBehaviour {


    public void newScene(string name)
    {
        
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }
}