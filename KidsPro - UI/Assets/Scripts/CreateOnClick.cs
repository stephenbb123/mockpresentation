using UnityEngine;
using System.Collections;

public class CreateOnClick : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public GameObject prefab;
    public Transform newParent;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetKey(KeyCode.Mouse0))
            {
                GameObject obj = Instantiate(prefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;
                obj.transform.SetParent(newParent);
                obj.transform.localScale = new Vector3(25, 25, 25);
                prefab.transform.localPosition = Vector3.zero;
               
            }

        }
    }

    public void OnMouseDrag(GameObject gameObject)
    {
        gameObject.transform.position = Input.mousePosition;
    }


}
