using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public class PBXProject : PBXObject
	{
		protected string MAINGROUP_KEY = "mainGroup";
		protected string KNOWN_REGIONS_KEY = "knownRegions";

		protected bool _clearedLoc = false;

		public PBXProject() : base() {
		}
		
		public PBXProject( string guid, PBXDictionary dictionary ) : base( guid, dictionary ) {
		}
		
		public string mainGroupID {
			get {
				return (string)_data[ MAINGROUP_KEY ];
			}
		}

		public PBXList knownRegions {
			get {
				return (PBXList)_data[ KNOWN_REGIONS_KEY ];
			}
		}

		public void AddRegion(string region) {
			if (!_clearedLoc)
			{
				// Only include localizations we explicitly specify
				knownRegions.Clear();
				_clearedLoc = true;
			}

			knownRegions.Add(region);
		}

		public PBXDictionary Attributes {

			get { 
				return (PBXDictionary)_data ["attributes"];
			}
		}

		public PBXDictionary TargetAttributes {
			get{ 
				return (PBXDictionary)this.Attributes ["TargetAttributes"];
			}
		}

		public PBXDictionary FirstTargetAttribute {

			get { 
				return (PBXDictionary)this.TargetAttributes [this.target];
			}
		}

		public  PBXDictionary SystemCapabilities{
			get{
				PBXDictionary capabilities = (PBXDictionary)this.FirstTargetAttribute ["SystemCapabilities"];
				if (capabilities != null) {
						return capabilities;
				} else {
					return new PBXDictionary ();
				}
			}
		}

		public  string target{
			get{
				PBXList targets = (PBXList)_data ["targets"];
				return (string)targets[0];
			}
		}
	}
}
