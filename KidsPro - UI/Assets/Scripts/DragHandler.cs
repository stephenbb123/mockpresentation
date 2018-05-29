using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public Transform parent;
    public static GameObject item;
	Vector3 startPosition;
	Transform startParent;
    GameObject newIcon;
    Boolean read =false;



   
	public void OnBeginDrag (PointerEventData eventData){
		item = gameObject;
		startPosition = transform.position;
        startParent = transform.parent;
        Debug.Log(startParent.name);
        if (startParent.name.Equals("Slot"))
        {
            newIcon = Instantiate(item, startPosition, Quaternion.identity, startParent);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        gameObject.transform.SetParent(parent);
        
	}

	public void OnDrag (PointerEventData eventData){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 445;
		transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        
	}

	public void OnEndDrag (PointerEventData eventData){
        if (startParent.name.Equals("Coding Slot"))
        {
            Destroy(item);
        }
        item = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent == startParent){
			transform.position = startPosition;
		}
       
	}
    public void SetRead(Boolean scanned)
    {
        read = scanned;
    }
    public Boolean GetRead()
    {
        return read;
    }
}
