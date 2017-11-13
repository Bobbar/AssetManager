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
	public class DevicesBaseCols
	{
		public const string AttribTable = "dev_codes";
		public const string DeviceUID = "dev_UID";
		public const string Description = "dev_description";
		public const string Location = "dev_location";
		public const string CurrentUser = "dev_cur_user";
		public const string Serial = "dev_serial";
		public const string AssetTag = "dev_asset_tag";
		public const string PurchaseDate = "dev_purchase_date";
		public const string ReplacementYear = "dev_replacement_year";
		public const string PO = "dev_po";
		public const string OSVersion = "dev_osversion";
		public const string PhoneNumber = "dev_phone_number";
		public const string EQType = "dev_eq_type";
		public const string Status = "dev_status";
		public const string Trackable = "dev_trackable";
		public const string HostName = "dev_hostname";
		public const string iCloudAccount = "dev_icloud_account";
	}
}
namespace AssetManager
{

	public class DevicesCols : DevicesBaseCols
	{
		public const string TableName = "devices";
		public const string LastModUser = "dev_lastmod_user";
		public const string LastModDate = "dev_lastmod_date";
		public const string InputDateTime = "dev_input_datetime";
		public const string CheckedOut = "dev_checkedout";
		public const string SibiLinkUID = "dev_sibi_link";
		public const string MunisEmpNum = "dev_cur_user_emp_num";
	}
}
namespace AssetManager
{

	public class HistoricalDevicesCols : DevicesBaseCols
	{
		public const string TableName = "dev_historical";
		public const string HistoryEntryUID = "hist_uid";
		public const string ChangeType = "hist_change_type";
		public const string Notes = "hist_notes";
		public const string ActionDateTime = "dev_lastmod_date";
		public const string ActionUser = "hist_action_user";
	}
}
namespace AssetManager
{

	public class TrackablesCols
	{
		public const string TableName = "dev_trackable";
		public const string UID = "track_uid";
		public const string CheckType = "track_check_type";
		public const string CheckoutTime = "track_checkout_time";
		public const string DueBackDate = "track_dueback_date";
		public const string CheckinTime = "track_checkin_time";
		public const string CheckoutUser = "track_checkout_user";
		public const string CheckinUser = "track_checkin_user";
		public const string AssetTag = "track_asset_tag";
		public const string FromLocation = "track_out_location";
		public const string UseLocation = "track_use_location";
		public const string Notes = "track_notes";
		public const string DeviceUID = "track_device_uid";
		public const string DateStamp = "track_datestamp";
	}
}
namespace AssetManager
{

	public abstract class AttachmentsBaseCols
	{

		public abstract string TableName { get; }
		public string Timestamp { get; }
		public string FKey { get; }
		public string FileName { get; }
		public string FileType { get; }
		public string FileSize { get; }
		public string FileUID { get; }
		public string FileHash { get; }
		public string Folder { get; }
	}
}
namespace AssetManager
{

	public class DeviceAttachmentsCols : AttachmentsBaseCols
	{
		public override string TableName { get; }
	}
}
namespace AssetManager
{

	public class SibiAttachmentsCols : AttachmentsBaseCols
	{
		public override string TableName { get; }
	}
}
namespace AssetManager
{

	public class SibiRequestCols
	{
		public const string AttribTable = "sibi_codes";
		public const string TableName = "sibi_requests";
		public const string UID = "sibi_uid";
		public const string RequestUser = "sibi_request_user";
		public const string Description = "sibi_description";
		public const string DateStamp = "sibi_datestamp";
		public const string NeedBy = "sibi_need_by";
		public const string Status = "sibi_status";
		public const string Type = "sibi_type";
		public const string PO = "sibi_PO";
		public const string RequisitionNumber = "sibi_requisition_number";
		public const string ReplaceAsset = "sibi_replace_asset";
		public const string ReplaceSerial = "sibi_replace_serial";
		public const string RequestNumber = "sibi_request_number";
		public const string RTNumber = "sibi_RT_number";
	}
}
namespace AssetManager
{

	public class SibiRequestItemsCols
	{
		public const string TableName = "sibi_request_items";
		public const string ItemUID = "sibi_items_uid";
		public const string RequestUID = "sibi_items_request_uid";
		public const string User = "sibi_items_user";
		public const string Description = "sibi_items_description";
		public const string Location = "sibi_items_location";
		public const string Status = "sibi_items_status";
		public const string ReplaceAsset = "sibi_items_replace_asset";
		public const string ReplaceSerial = "sibi_items_replace_serial";
		public const string NewAsset = "sibi_items_new_asset";
		public const string NewSerial = "sibi_items_new_serial";
		public const string OrgCode = "sibi_items_org_code";
		public const string ObjectCode = "sibi_items_object_code";
		public const string Qty = "sibi_items_qty";
		public const string Timestamp = "sibi_items_timestamp";
	}
}
namespace AssetManager
{

	public class SibiNotesCols
	{
		public const string TableName = "sibi_notes";
		public const string RequestUID = "sibi_request_uid";
		public const string NoteUID = "sibi_note_uid";
		public const string DateStamp = "sibi_datestamp";
		public const string Note = "sibi_note";
	}
}
namespace AssetManager
{

	public class ComboCodesBaseCols
	{
		public const string TypeName = "type_name";
		public const string DisplayValue = "human_value";
		public const string CodeValue = "db_value";
		public const string ID = "id";
	}
}
namespace AssetManager
{

	public class DeviceComboCodesCols : ComboCodesBaseCols
	{
		public const string TableName = "dev_codes";
		public const string MunisCode = "munis_code";
	}
}
namespace AssetManager
{

	public class SibiComboCodesCols : ComboCodesBaseCols
	{
		public const string TableName = "sibi_codes";
	}
}
namespace AssetManager
{

	public class SecurityCols
	{
		public const string TableName = "security";
		public const string SecModule = "sec_module";
		public const string AccessLevel = "sec_access_level";
		public const string Description = "sec_desc";
		public const string AvailOffline = "sec_availoffline";
	}
}
namespace AssetManager
{

	public class UsersCols
	{
		public const string TableName = "users";
		public const string UserName = "usr_username";
		public const string FullName = "usr_fullname";
		public const string AccessLevel = "usr_access_level";
		public const string UID = "usr_UID";
	}
}
namespace AssetManager
{

	public class EmployeesCols
	{
		public const string TableName = "employees";
		public const string Name = "emp_name";
		public const string Number = "emp_number";
		public const string UID = "emp_UID";
	}
}
namespace AssetManager
{

	public sealed class DeviceAttribType
	{
		public const string Location = "LOCATION";
		public const string ChangeType = "CHANGETYPE";
		public const string EquipType = "EQ_TYPE";
		public const string OSType = "OS_TYPE";

		public const string StatusType = "STATUS_TYPE";
	}
}
namespace AssetManager
{

	public sealed class SibiAttribType
	{

		public const string SibiStatusType = "STATUS";
		public const string SibiItemStatusType = "ITEM_STATUS";
		public const string SibiRequestType = "REQ_TYPE";
		public const string SibiAttachFolder = "ATTACH_FOLDER";
	}
}
namespace AssetManager
{

	public sealed class CheckType
	{
		public const string Checkin = "IN";
		public const string Checkout = "OUT";
	}
}
