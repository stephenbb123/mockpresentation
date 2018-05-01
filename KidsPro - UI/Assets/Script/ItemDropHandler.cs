using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour , IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform bloPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(bloPanel, Input.mousePosition))
        {
            Debug.Log("Drop");
        }
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
