using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour
{
    public Transform parent;
    public GameObject newBlock;
    public Sprite cap = null;
    public Sprite Image
    {
        get
        {
            return cap;
        }
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 120;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            
            newBlock = Instantiate(gameObject,transform.position,Quaternion.identity,parent);
            newBlock.SetActive(true);
            Debug.Log("Block Placed");
        }
    }

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }





}
