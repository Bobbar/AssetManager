using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
namespace AssetManager
{
	static class GlobalInstances
	{

		public class SibiAttributes
		{
			public AttributeDataStruct[] StatusType;
			public AttributeDataStruct[] ItemStatusType;
			public AttributeDataStruct[] RequestType;
			public AttributeDataStruct[] AttachFolder;
		}

		public class DeviceAttributes
		{
			public AttributeDataStruct[] Locations;
			public AttributeDataStruct[] ChangeType;
			public AttributeDataStruct[] EquipType;
			public AttributeDataStruct[] OSType;
			public AttributeDataStruct[] StatusType;
		}

		public static DeviceAttributes DeviceAttribute = new DeviceAttributes();
		public static SibiAttributes SibiAttribute = new SibiAttributes();
		public static MunisFunctions MunisFunc = new MunisFunctions();
		public static AssetManagerFunctions AssetFunc = new AssetManagerFunctions();

		public static FtpFunctions FTPFunc = new FtpFunctions();
	}
}
