using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Threading;
using System.IO;

namespace BumpkinLabs.IGE
{
	public static class LevelSerializer 
	{
		public static string CurrentLevelXML()
		{
			System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;

			Thread.CurrentThread.CurrentCulture =  new System.Globalization.CultureInfo("en-US");

			LevelData ld = new LevelData();
			
			//Store the camera location and rotation.
			CameraController cc = LevelEditorManager.Instance.editorCamera.gameObject.GetComponent<CameraController>();
			
			ld.CameraPositionX = cc.CameraPosition.x;
			ld.CameraPositionY = cc.CameraPosition.y;
			ld.CameraPositionZ = cc.CameraPosition.z;
			
			ld.CameraRotationX = cc.CameraRotation.x;
			ld.CameraRotationY = cc.CameraRotation.y;
			ld.CameraRotationZ = cc.CameraRotation.z;
			
			ld.CameraDistance = cc.CameraDistance;
			
			foreach (BuildingBlock bb in GameObject.FindObjectsOfType<BuildingBlock>())
			{
				if (bb.CurrentState == BuildingBlock.BuildingBlockState.InScene) //only add if the block is in the scene.
					ld.AddBuildingBlock(bb);
			}

			XmlSerializer s = new XmlSerializer(typeof(LevelData));
			System.IO.StringWriter sw = new System.IO.StringWriter();
			s.Serialize(sw, ld);
			
			sw.Close();

			string rv = sw.ToString();

			Thread.CurrentThread.CurrentCulture = ci;

			return rv;
		}

		/// <summary>
		/// Creates a LevelData object from a text asset containing saved data
		/// </summary>
		public static LevelData CreateFromTextAsset(TextAsset textAsset)
		{
			return CreateFromXML(textAsset.text);
		}

		/// <summary>
		/// Creates a LevelData object from a file containing saved data
		/// </summary>
		public static LevelData LoadFromFile(string fileName)
		{
			string xml = "";

			using (StreamReader sr = new StreamReader(fileName))
			{
				xml = sr.ReadToEnd();
			}

			return CreateFromXML(xml);
		}

		private static LevelData CreateFromXML (string xml)
		{
			XmlSerializer s = new XmlSerializer(typeof(LevelData));
			
			return s.Deserialize(new System.IO.StringReader(xml)) as LevelData; 
		}

		/// <summary>
		/// The temp file name
		/// </summary>
		public static string TempFileName
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "igetempsave.txt");
			}
		}

		/// <summary>
		/// Creates the full name of the file by appending the persistent file path to the begginning.  
		/// so temp.txt becomes c:\users\afilepath\temp.txt for example.
		/// </summary>
		public static string CreateFullFileName(string fileName)
		{
			return Path.Combine(Application.persistentDataPath, fileName);
		}

		/// <summary>
		/// Saves the current level to an XML file
		/// </summary>
		/// <param name="fileName">Full file name</param>
		public static void SaveLevelToFile(string fileName)
		{	
			using (StreamWriter sw = new StreamWriter(fileName, false))
			{
				sw.Write(CurrentLevelXML());
			}
		}

		/// <summary>
		/// Saves the levels to the temp file
		/// </summary>
		public static void SaveLevelToTempFile()
		{
			SaveLevelToFile(TempFileName);
		}
	}
}
