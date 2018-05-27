using UnityEngine;
using System.Collections;

public class Crashed : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CallCrash()
    {
        gameObject.SetActive(true);
    }
}
