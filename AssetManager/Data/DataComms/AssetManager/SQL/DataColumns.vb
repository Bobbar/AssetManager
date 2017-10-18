Public Class DevicesBaseCols
    Public Const AttribTable As String = "dev_codes"
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
    Public Const HostName As String = "dev_hostname"
    Public Const iCloudAccount As String = "dev_icloud_account"
End Class

Public Class DevicesCols
    Inherits DevicesBaseCols
    Public Const TableName As String = "devices"
    Public Const LastModUser As String = "dev_lastmod_user"
    Public Const LastModDate As String = "dev_lastmod_date"
    Public Const InputDateTime As String = "dev_input_datetime"
    Public Const CheckedOut As String = "dev_checkedout"
    Public Const SibiLinkUID As String = "dev_sibi_link"
    Public Const MunisEmpNum As String = "dev_cur_user_emp_num"
End Class

Public Class HistoricalDevicesCols
    Inherits DevicesBaseCols
    Public Const TableName As String = "dev_historical"
    Public Const HistoryEntryUID As String = "hist_uid"
    Public Const ChangeType As String = "hist_change_type"
    Public Const Notes As String = "hist_notes"
    Public Const ActionDateTime As String = "dev_lastmod_date"
    Public Const ActionUser As String = "hist_action_user"
End Class

Public Class TrackablesCols
    Public Const TableName As String = "dev_trackable"
    Public Const UID As String = "track_uid"
    Public Const CheckType As String = "track_check_type"
    Public Const CheckoutTime As String = "track_checkout_time"
    Public Const DueBackDate As String = "track_dueback_date"
    Public Const CheckinTime As String = "track_checkin_time"
    Public Const CheckoutUser As String = "track_checkout_user"
    Public Const CheckinUser As String = "track_checkin_user"
    Public Const AssetTag As String = "track_asset_tag"
    Public Const FromLocation As String = "track_out_location"
    Public Const UseLocation As String = "track_use_location"
    Public Const Notes As String = "track_notes"
    Public Const DeviceUID As String = "track_device_uid"
    Public Const DateStamp As String = "track_datestamp"
End Class

Public MustInherit Class AttachmentsBaseCols

    Public MustOverride ReadOnly Property TableName As String
    Public ReadOnly Property Timestamp As String = "attach_timestamp"
    Public ReadOnly Property FKey As String = "attach_fkey_UID"
    Public ReadOnly Property FileName As String = "attach_file_name"
    Public ReadOnly Property FileType As String = "attach_file_type"
    Public ReadOnly Property FileSize As String = "attach_file_size"
    Public ReadOnly Property FileUID As String = "attach_file_UID"
    Public ReadOnly Property FileHash As String = "attach_file_hash"
    Public ReadOnly Property Folder As String = "attach_folder"
End Class

Public Class DeviceAttachmentsCols
    Inherits AttachmentsBaseCols
    Public Overrides ReadOnly Property TableName As String = "dev_attachments"
End Class

Public Class SibiAttachmentsCols
    Inherits AttachmentsBaseCols
    Public Overrides ReadOnly Property TableName As String = "sibi_attachments"
End Class

Public Class SibiRequestCols
    Public Const AttribTable As String = "sibi_codes"
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
    Public Const ReplaceAsset As String = "sibi_replace_asset"
    Public Const ReplaceSerial As String = "sibi_replace_serial"
    Public Const RequestNumber As String = "sibi_request_number"
    Public Const RTNumber As String = "sibi_RT_number"
End Class

Public Class SibiRequestItemsCols
    Public Const TableName As String = "sibi_request_items"
    Public Const ItemUID As String = "sibi_items_uid"
    Public Const RequestUID As String = "sibi_items_request_uid"
    Public Const User As String = "sibi_items_user"
    Public Const Description As String = "sibi_items_description"
    Public Const Location As String = "sibi_items_location"
    Public Const Status As String = "sibi_items_status"
    Public Const ReplaceAsset As String = "sibi_items_replace_asset"
    Public Const ReplaceSerial As String = "sibi_items_replace_serial"
    Public Const NewAsset As String = "sibi_items_new_asset"
    Public Const NewSerial As String = "sibi_items_new_serial"
    Public Const OrgCode As String = "sibi_items_org_code"
    Public Const ObjectCode As String = "sibi_items_object_code"
    Public Const Qty As String = "sibi_items_qty"
    Public Const Timestamp As String = "sibi_items_timestamp"
End Class

Public Class SibiNotesCols
    Public Const TableName As String = "sibi_notes"
    Public Const RequestUID As String = "sibi_request_uid"
    Public Const NoteUID As String = "sibi_note_uid"
    Public Const DateStamp As String = "sibi_datestamp"
    Public Const Note As String = "sibi_note"
End Class

Public Class ComboCodesBaseCols
    Public Const TypeName As String = "type_name"
    Public Const DisplayValue As String = "human_value"
    Public Const CodeValue As String = "db_value"
    Public Const ID As String = "id"
End Class

Public Class DeviceComboCodesCols
    Inherits ComboCodesBaseCols
    Public Const TableName As String = "dev_codes"
    Public Const MunisCode As String = "munis_code"
End Class

Public Class SibiComboCodesCols
    Inherits ComboCodesBaseCols
    Public Const TableName As String = "sibi_codes"
End Class

Public Class SecurityCols
    Public Const TableName As String = "security"
    Public Const SecModule As String = "sec_module"
    Public Const AccessLevel As String = "sec_access_level"
    Public Const Description As String = "sec_desc"
    Public Const AvailOffline As String = "sec_availoffline"
End Class

Public Class UsersCols
    Public Const TableName As String = "users"
    Public Const UserName As String = "usr_username"
    Public Const FullName As String = "usr_fullname"
    Public Const AccessLevel As String = "usr_access_level"
    Public Const UID As String = "usr_UID"
End Class

Public Class EmployeesCols
    Public Const TableName As String = "employees"
    Public Const Name As String = "emp_name"
    Public Const Number As String = "emp_number"
    Public Const UID As String = "emp_UID"
End Class

Public NotInheritable Class DeviceAttribType
    Public Const Location As String = "LOCATION"
    Public Const ChangeType As String = "CHANGETYPE"
    Public Const EquipType As String = "EQ_TYPE"
    Public Const OSType As String = "OS_TYPE"
    Public Const StatusType As String = "STATUS_TYPE"

End Class

Public NotInheritable Class SibiAttribType

    Public Const SibiStatusType As String = "STATUS"
    Public Const SibiItemStatusType As String = "ITEM_STATUS"
    Public Const SibiRequestType As String = "REQ_TYPE"
    Public Const SibiAttachFolder As String = "ATTACH_FOLDER"
End Class

Public NotInheritable Class CheckType
    Public Const Checkin As String = "IN"
    Public Const Checkout As String = "OUT"
End Class