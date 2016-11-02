Public Module DataColumns

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
        Public Const EQType As String = "dev_eq_type"
        Public Const Status As String = "dev_status"
        Public Const Trackable As String = "dev_trackable"
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
        Public Const ActionDateTime As String = "hist_action_datetime"
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
        Public Const TimeStamp As String = "attach_timestamp"
        Public Const FKey As String = "attach_fkey_UID"
        Public Const FileName As String = "attach_file_name"
        Public Const FileType As String = "attach_file_type"
        Public Const FileSize As String = "attach_file_size"
        Public Const FileUID As String = "attach_file_UID"
        Public Const FileHash As String = "attach_file_hash"
    End Class
    Public Class dev_attachments
        Inherits main_attachments
        Public Const TableName As String = "dev_attachments"
    End Class
    Public Class sibi_attachments
        Inherits main_attachments
        Public Const TableName As String = "sibi_attachments"
        Public Const Folder As String = "attach_folder"

    End Class
End Module

