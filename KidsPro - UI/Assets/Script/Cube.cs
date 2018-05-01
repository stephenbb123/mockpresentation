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
        
        

    }

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        
    }
    private void OnMouseDrag()
    {
        if (!GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<Rigidbody>();
        }
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 130;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
    private void OnMouseUp()
    {
        //newBlock = Instantiate(gameObject, transform.position, Quaternion.identity, parent);
        //newBlock.SetActive(true);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        Debug.Log("Block Placed");
    }
    private void OnCollisionStay(Collision collision)
    {
       Destroy(GetComponent<Rigidbody>());
    }




}
