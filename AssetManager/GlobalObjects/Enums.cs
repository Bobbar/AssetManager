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
	public enum EntryType
	{
		Sibi,
		Device
	}
}
namespace AssetManager
{

	public enum PdfFormType
	{
		InputForm,
		TransferForm,
		DisposeForm
	}
}
namespace AssetManager
{

	public enum LiveBoxType
	{
		DynamicSearch,
		InstaLoad,
		SelectValue,
		UserSelect
	}
}
namespace AssetManager
{

	public enum FindDevType
	{
		AssetTag,
		Serial
	}
}
namespace AssetManager
{

	public enum CommandArgs
	{
		TESTDB,
		VINTONDD
	}
}
namespace AssetManager
{

	public enum Databases
	{
		test_db,
		asset_manager,
		vintondd
	}
}
