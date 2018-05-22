using UnityEngine;
using System.Collections;
using System.Globalization;

namespace BumpkinLabs.IGE
{
	public static class MetaDataHelper  
	{
		/// <summary>
		/// Casts a vector3 to a string with can be read with ToVector3
		/// </summary>
		public static string FromVector3(Vector3 v)
		{
			return string.Format("{0}~{1}~{2}", 
				FromFloat(v.x),
				FromFloat(v.y),
				FromFloat(v.z));
		}
		
		/// <summary>
		/// Returns a vector3 from a string created with FromVector3
		/// </summary>
		public static Vector3 ToVector3(string s)
		{
			Vector3 rv = Vector3.zero;
			
			try
			{
				string[] parts = s.Split('~');
				
				if (parts.Length == 3)
				{
					rv = new Vector3(ToFloat(parts[0]), ToFloat(parts[1]), ToFloat(parts[2]));
				}
			}
			catch(System.Exception ex)
			{
				Debug.LogWarning(string.Format("Exception during ToVector {0}", ex.Message));
			}
			
			return rv;
		}
		
		/// <summary>
		/// Returns a string that can be changed back into a float with ToFloat
		/// </summary>
		public static string FromFloat(float f)
		{
			return f.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
		}
		
		/// <summary>
		/// Returns a float from a string created with FromFloat
		/// </summary>
		public static float ToFloat(string s)
		{
			float r;
			if (float.TryParse(s, NumberStyles.Float, CultureInfo.CreateSpecificCulture("en-GB"), out r))
			{
				return r;
			}
			else
			{
				Debug.LogWarning(string.Format("ToFloat could not parse {0}", s));
			}
			
			return 0;
		}
	}
}
