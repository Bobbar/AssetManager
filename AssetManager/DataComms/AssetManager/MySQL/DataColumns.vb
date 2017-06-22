Public Class devices_main
    Public Const DeviceUID As String = "dev_UID"
    Public Const Description As String = "dev_description"
    Public Const Location As String = "dev_location"
    Public Const CurrentUser As String = "dev_cur_user"
    Public Const Serial As String = "dev_serial"
    Public Const AssetTag As String = "dev_asset_tag"
    Public Const PurchaseDate As String = "dev_purchase_date"
    Public Const ReplacementYear As String = "dev_replacement_year"
    Public Const PO As String = "dev_po"
    Public Const OSVersion As String = "dev_osversion"
    Public Const PhoneNumber As String = "dev_phone_number"
    Public Const EQType As String = "dev_eq_type"
    Public Const Status As String = "dev_status"
    Public Const Trackable As String = "dev_trackable"
    Public Const CheckSum As String = "dev_checksum"
End Class
Public Class devices
    Inherits devices_main
    Public Const TableName As String = "devices"
    Public Const LastMod_User As String = "dev_lastmod_user"
    Public Const LastMod_Date As String = "dev_lastmod_date"
    Public Const Input_DateTime As String = "dev_input_datetime"
    Public Const CheckedOut As String = "dev_checkedout"
    Public Const Sibi_Link_UID As String = "dev_sibi_link"
    Public Const Munis_Emp_Num As String = "dev_cur_user_emp_num"
End Class
Public Class historical_dev
    Inherits devices_main
    Public Const TableName As String = "dev_historical"
    Public Const History_Entry_UID As String = "hist_uid"
    Public Const ChangeType As String = "hist_change_type"
    Public Const Notes As String = "hist_notes"
    Public Const ActionDateTime As String = "dev_lastmod_date" '"hist_action_datetime"
    Public Const ActionUser As String = "hist_action_user"
End Class
Public Class trackable
    Public Const TableName As String = "dev_trackable"
    Public Const UID As String = "track_uid"
    Public Const CheckType As String = "track_check_type"
    Public Const CheckOut_Time As String = "track_checkout_time"
    Public Const DueBackDate As String = "track_dueback_date"
    Public Const CheckIn_Time As String = "track_checkin_time"
    Public Const CheckOut_User As String = "track_checkout_user"
    Public Const CheckIn_User As String = "track_checkin_user"
    Public Const AssetTag As String = "track_asset_tag"
    Public Const FromLocation As String = "track_out_location"
    Public Const UseLocation As String = "track_use_location"
    Public Const Notes As String = "track_notes"
    Public Const DeviceUID As String = "track_device_uid"
    Public Const DateStamp As String = "track_datestamp"
End Class
Public Class main_attachments

    Public Overridable ReadOnly Property TableName As String = "attachments"
    Public ReadOnly Property TimeStamp As String = "attach_timestamp"
    Public ReadOnly Property FKey As String = "attach_fkey_UID"
    Public ReadOnly Property FileName As String = "attach_file_name"
    Public ReadOnly Property FileType As String = "attach_file_type"
    Public ReadOnly Property FileSize As String = "attach_file_size"
    Public ReadOnly Property FileUID As String = "attach_file_UID"
    Public ReadOnly Property FileHash As String = "attach_file_hash"
    Public Overridable ReadOnly Property Folder As String = "attach_folder"
End Class
Public Class dev_attachments
    Inherits main_attachments
    Public Overrides ReadOnly Property TableName As String = "dev_attachments"
End Class
Public Class sibi_attachments
    Inherits main_attachments
    Public Overrides ReadOnly Property TableName As String = "sibi_attachments"
    Public Overrides ReadOnly Property Folder As String = "attach_folder"
End Class
Public Class sibi_requests
    Public Const TableName As String = "sibi_requests"
    Public Const UID As String = "sibi_uid"
    Public Const RequestUser As String = "sibi_request_user"
    Public Const Description As String = "sibi_description"
    Public Const DateStamp As String = "sibi_datestamp"
    Public Const NeedBy As String = "sibi_need_by"
    Public Const Status As String = "sibi_status"
    Public Const Type As String = "sibi_type"
    Public Const PO As String = "sibi_PO"
    Public Const RequisitionNumber As String = "sibi_requisition_number"
    Public Const Replace_Asset As String = "sibi_replace_asset"
    Public Const Replace_Serial As String = "sibi_replace_serial"
    Public Const RequestNumber As String = "sibi_request_number"
    Public Const RT_Number As String = "sibi_RT_number"
End Class
Public Class sibi_request_items
    Public Const TableName As String = "sibi_request_items"
    Public Const Item_UID As String = "sibi_items_uid"
    Public Const Request_UID As String = "sibi_items_request_uid"
    Public Const User As String = "sibi_items_user"
    Public Const Description As String = "sibi_items_description"
    Public Const Location As String = "sibi_items_location"
    Public Const Status As String = "sibi_items_status"
    Public Const Replace_Asset As String = "sibi_items_replace_asset"
    Public Const Replace_Serial As String = "sibi_items_replace_serial"
    Public Const New_Asset As String = "sibi_items_new_asset"
    Public Const New_Serial As String = "sibi_items_new_serial"
    Public Const Org_Code As String = "sibi_items_org_code"
    Public Const Object_Code As String = "sibi_items_object_code"
    Public Const Qty As String = "sibi_items_qty"
    Public Const Sequence As String = "sibi_items_seq"
    Public Const TimeStamp As String = "sibi_items_timestamp"
End Class
Public Class sibi_notes
    Public Const TableName As String = "sibi_notes"
    Public Const Request_UID As String = "sibi_request_uid"
    Public Const Note_UID As String = "sibi_note_uid"
    Public Const DateStamp As String = "sibi_datestamp"
    Public Const Note As String = "sibi_note"
End Class
Public Class main_combocodes
    Public Const TypeName As String = "type_name"
    Public Const HumanValue As String = "human_value"
    Public Const DB_Value As String = "db_value"
    Public Const ID As String = "id"
End Class
Public Class dev_codes
    Inherits main_combocodes
    Public Const TableName As String = "dev_codes"
    Public Const MunisCode As String = "munis_code"
End Class
Public Class sibi_codes
    Inherits main_combocodes
    Public Const TableName As String = "sibi_codes"
End Class
Public Class security
    Public Const TableName As String = "security"
    Public Const SecModule As String = "sec_module"
    Public Const AccessLevel As String = "sec_access_level"
    Public Const Description As String = "sec_desc"
    Public Const AvailOffline As String = "sec_availoffline"
End Class
Public Class users
    Public Const TableName As String = "users"
    Public Const UserName As String = "usr_username"
    Public Const FullName As String = "usr_fullname"
    Public Const AccessLevel As String = "usr_access_level"
    Public Const UID As String = "usr_UID"
End Class
Public Class employees
    Public Const TableName As String = "employees"
    Public Const Name As String = "emp_name"
    Public Const Number As String = "emp_number"
    Public Const UID As String = "emp_UID"
End Class

