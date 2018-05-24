using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour
{
    public GameObject car;
    public static GameObject item;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Clicked()
    {
        
        Debug.Log(gameObject.name);
        car.transform.position = new Vector3(677.3f, 383.4f, -4.3f);
        gameObject.GetComponent<Coding>().Clear();
        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild != null)
                {
                    
                    Debug.Log("GrandChild: " + grandChild.name);
                    Destroy(grandChild.gameObject);

                }
            }
        }
    }
}
