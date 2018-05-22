using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

namespace BumpkinLabs.IGE
{
	[XmlRoot("LevelData")] 
	public class BuildingBlockData 
	{
		private string buildingBlockName;
		private string buildingBlockSubName;
		private float positionX;
		private float positionY;
		private float positionZ;
		private float rotationX;
		private float rotationY;
		private float rotationZ;
		private float rotationW;
		private string metaData;
		private string buildingBlockId;
		private bool locked;

		[XmlAttribute("BuildingBlockName")]
		public string BuildingBlockName
		{
			get { return buildingBlockName; }
			set { buildingBlockName = value; }
		}

		[XmlAttribute("BuildingBlockSubName")]
		public string BuildingBlockSubName
		{
			get { return buildingBlockSubName; }
			set {buildingBlockSubName = value; }
		}

		[XmlAttribute("PositionX")]
		public float PositionX
		{
			get { return positionX; }
			set { positionX = value; }
		}

		[XmlAttribute("PositionY")]
		public float PositionY
		{
			get { return positionY; }
			set { positionY = value; }
		}

		[XmlAttribute("PositionZ")]
		public float PositionZ
		{
			get { return positionZ; }
			set { positionZ = value; }
		}

		[XmlAttribute("RotationX")]
		public float RotationX
		{
			get { return rotationX; }
			set { rotationX = value; }
		}

		[XmlAttribute("RotationY")]
		public float RotationY
		{
			get { return rotationY; }
			set { rotationY = value; }
		}

		[XmlAttribute("RotationZ")]
		public float RotationZ
		{
			get { return rotationZ; }
			set { rotationZ = value; }
		}

		[XmlAttribute("RotationW")]
		public float RotationW
		{
			get { return rotationW; }
			set { rotationW = value; }
		}

		[XmlAttribute("MetaData")]
		public string MetaData
		{
			get { return metaData; }
			set 
			{ 
				metaData = value; 
			}
		}

		[XmlAttribute("BuildingBlockId")]
		public string BuildingBlockID
		{
			get { return buildingBlockId; }
			set { buildingBlockId = value; }
		}
		
		[XmlAttribute("Locked")]
		public bool Locked
		{
			get { return locked; }
			set { locked = value; }
		}

		/// <summary>
		/// Sets values for this object with the given building block
		/// </summary>
		public void InitializeWithBuildingBlock(BuildingBlock bb)
		{
			this.BuildingBlockName = bb.buildingBlockName;
			this.BuildingBlockSubName = bb.buildingBlockSubName;
			this.BuildingBlockID = bb.BuildingBlockID;

			this.PositionX = bb.transform.position.x;
			this.PositionY = bb.transform.position.y;
			this.PositionZ = bb.transform.position.z;

			this.RotationX = bb.transform.rotation.x;
			this.RotationY = bb.transform.rotation.y;
			this.RotationZ = bb.transform.rotation.z;
			this.RotationW = bb.transform.rotation.w;

			this.metaData = "";
			
			this.locked = bb.locked;

			EditableBlock eb = bb.GetComponentInChildren<EditableBlock>();

			if (eb != null)
			{
				eb.PrepareForSave();
				this.MetaData = eb.MetaData;
			}
		}

		/// <summary>
		/// Creates a level block from the data in this object.
		/// </summary>
		public void CreateLevelBlock()
		{
			LevelBlock lb = LevelBlockLibrary.Instance.CreateBlock(buildingBlockName, buildingBlockSubName);
			lb.gameObject.transform.position = new Vector3(positionX, positionY, positionZ);
			lb.gameObject.transform.rotation = new Quaternion(rotationX, rotationY, rotationZ, rotationW);
			lb.BuildingBlockID = buildingBlockId;

			lb.Setup();
			lb.gameObject.BroadcastMessage("SetMetaData", MetaData, SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Creates an editor block with the data from this object.
		/// </summary>
		public void CreateEditorBlock ()
		{
			BuildingBlock bb = BuildingBlockLibrary.Instance.CreateBlock(buildingBlockName, buildingBlockSubName);

			bb.gameObject.transform.position = new Vector3(positionX, positionY, positionZ);
			bb.gameObject.transform.rotation = new Quaternion(rotationX, rotationY, rotationZ, rotationW);
			bb.BuildingBlockID = buildingBlockId;

			EditableBlock eb = bb.GetComponentInChildren<EditableBlock>();
			
			if (eb != null)
			{
				eb.MetaData = this.MetaData;
			}

			bb.AddToScene();
		}
	}
}
