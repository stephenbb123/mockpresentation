using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BumpkinLabs.IGE
{
	[RequireComponent(typeof(RectTransform))]
	public class IGEMouseController : MonoBehaviour 
	{
		private RectTransform rectTransform;
		private bool overUIElement = false;

		private static IGEMouseController instance;

		/// <summary>
		/// Return the current instance
		/// </summary>
		/// <value>The instance.</value>
		public static IGEMouseController Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<IGEMouseController>();

					if (instance == null)
					{
						Debug.LogError("There is no IGEMouseController setup");
					}
				}

				return instance;
			}
		}

		public Vector3 MousePosition
		{
			get
			{
				return rectTransform.position;
			}
		}

		public bool MouseLeftDown()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (!overUIElement)
					return true;
			}

			return false;
		}

		public bool MouseRightDown()
		{
			if (Input.GetMouseButtonDown(1))
			{
				if (!overUIElement)
					return true;
			}

			return false;
		}

		public bool OverGUIElement
		{
			get
			{
				return overUIElement;
			}
		}

		void Start()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		void Update()
		{
			if (EventSystem.current != null)
			{
				overUIElement = EventSystem.current.IsPointerOverGameObject();
				rectTransform.position = Input.mousePosition;
			}
		}
	}
}