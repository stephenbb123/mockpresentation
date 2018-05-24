using UnityEngine;
using System.Collections;

public class GetText : MonoBehaviour
{
    public string userText;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void getText()
    {
        userText = gameObject.GetComponent<string>();
    }
}
