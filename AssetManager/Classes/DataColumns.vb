Public Module DataColumns

    Public Class devices_main

        Public Const UID As String = "dev_UID"
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

End Module

