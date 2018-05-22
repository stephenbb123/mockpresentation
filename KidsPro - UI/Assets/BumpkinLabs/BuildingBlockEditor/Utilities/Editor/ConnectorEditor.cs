using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using BumpkinLabs.IGE;

[CustomEditor(typeof(BuildingBlockConnector))]
public class ConnectorEditor : Editor 
{
	private bool pickModeActive = false;
	private GameObject colliderObject;
	private bool reselect = false;
	
	void OnEnable()
	{
		pickModeActive = false;
	}
	
	void OnDisable()
	{	
		if (pickModeActive)
		{
			SceneView.onSceneGUIDelegate -= HandleSceneView;
		}
		
		if (reselect)
		{
			reselect = false;
			GameObject go = (target as BuildingBlockConnector).gameObject;
			Selection.activeGameObject = go;
		}
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		bool disabledState = GUI.enabled;
		
		if (pickModeActive)
			GUI.enabled = false;
								
		if (GUILayout.Button("Pick Point", GUILayout.Height(40)))
		{
			BuildingBlockConnector thisObject = target as BuildingBlockConnector;
			BuildingBlock bb = thisObject.gameObject.GetComponentInParent<BuildingBlock>();
			
			if (bb != null)
			{
				DetailedMeshCollider dmc = bb.gameObject.GetComponentInChildren<DetailedMeshCollider>();
				
				if (dmc != null)
				{
					colliderObject = dmc.gameObject;
					pickModeActive = true;
					SceneView.onSceneGUIDelegate += HandleSceneView;	
				}
			}
		}
		
		if (GUILayout.Button("Snap Position", GUILayout.Height(40)))
		{
			SnapPosition();
		}
		
		GUI.enabled = disabledState;
		
		if (pickModeActive)
			GUILayout.Label("Right click on the model to place connector.  Hold shift to snap to 0.5");
	}
	
	private void SnapPosition()
	{
		Vector3 pos = (target as BuildingBlockConnector).transform.localPosition;
		
		pos.x = Mathf.Round(pos.x * 2f) / 2f;
		pos.y = Mathf.Round(pos.y * 2f) / 2f;
		pos.z = Mathf.Round(pos.z * 2f) / 2f;
		
		(target as BuildingBlockConnector).transform.localPosition = pos;
	}
	
	void HandleSceneView(SceneView s)
	{
		Event e = Event.current;
	
		if (e.type == EventType.MouseDown)
		{
			if (e.button == 0)
			{
				Vector3 mousePos = Event.current.mousePosition;
				mousePos.y = Screen.height - mousePos.y;
				Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
				
				foreach (RaycastHit hit in Physics.RaycastAll(r))
				{
					if (hit.collider.gameObject == colliderObject)
					{
						pickModeActive = false;
						SceneView.onSceneGUIDelegate -= HandleSceneView;
						
						BuildingBlockConnector connector = target as BuildingBlockConnector;
						connector.transform.position = hit.point;
						connector.transform.forward = hit.normal;
						reselect = true;
						e.Use();
						
						if (e.modifiers == EventModifiers.Shift)
						{
							SnapPosition();
						}
						
					}
				}
			}
		}	
	
	}
}
