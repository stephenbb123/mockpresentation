using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BumpkinLabs.IGE
{
	[XmlRoot("LevelData")] 
	public class LevelData 
	{
		private List<BuildingBlockData> buildingBlocks;
		private float cameraPositionX;
		private float cameraPositionY;
		private float cameraPositionZ;
		
		private float cameraRotationX;
		private float cameraRotationY;
		private float cameraRotationZ;
		
		private float cameraDistance;
		
		public LevelData()
		{
			buildingBlocks = new List<BuildingBlockData>();
		}

		[XmlArray("BuildingBlocks")]
		[XmlArrayItem("BuildingBlock")]
		public List<BuildingBlockData> BuildingBlocks
		{
			get { return buildingBlocks; }
			set { buildingBlocks = value; }
		}

		public void AddBuildingBlock(BuildingBlock bb)
		{
			BuildingBlockData bbd = new BuildingBlockData();
			bbd.InitializeWithBuildingBlock(bb);

			BuildingBlocks.Add(bbd);
		}
		
		[XmlAttribute("CameraX")]
		public float CameraPositionX
		{
			get { return cameraPositionX; }
			set { cameraPositionX = value; }
		}
		
		[XmlAttribute("CameraY")]
		public float CameraPositionY
		{
			get { return cameraPositionY; }
			set { cameraPositionY = value; }
		}
		
		[XmlAttribute("CameraZ")]
		public float CameraPositionZ
		{
			get { return cameraPositionZ; }
			set { cameraPositionZ = value; }
		}
		
		[XmlAttribute("CameraRotationX")]
		public float CameraRotationX
		{
			get { return cameraRotationX; }
			set { cameraRotationX = value; }
		}
		
		[XmlAttribute("CameraRotationY")]
		public float CameraRotationY
		{
			get { return cameraRotationY; }
			set { cameraRotationY = value; }
		}
		
		[XmlAttribute("CameraRotationZ")]
		public float CameraRotationZ
		{
			get { return cameraRotationZ; }
			set { cameraRotationZ = value; }
		}
		
		[XmlAttribute("CameraDistance")]
		public float CameraDistance
		{
			get { return cameraDistance; }
			set { cameraDistance = value; }
		}
	}
}
