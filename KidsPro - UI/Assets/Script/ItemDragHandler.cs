using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IEndDragHandler ,IDragHandler
{
    public string name;
    public GameObject goal;
    public BlockPanelItem Item { get; set; }


    public void OnDrag(PointerEventData eventData)
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 120;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        
        gameObject.transform.localPosition = new Vector3 (-1,1,0);
        goal.SetActive(true);
    }
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
