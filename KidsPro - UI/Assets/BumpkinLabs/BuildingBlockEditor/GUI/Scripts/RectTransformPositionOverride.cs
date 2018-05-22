using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RectTransformPositionOverride : MonoBehaviour 
{
	public Vector2 anchorPosition = Vector2.zero;

	private RectTransform myRectTransform;

	// Use this for initialization
	void Start () 
	{
		myRectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		myRectTransform.anchoredPosition = anchorPosition;
	}
}
