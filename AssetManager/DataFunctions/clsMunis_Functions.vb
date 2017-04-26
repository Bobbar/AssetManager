Imports MyDialogLib
Public Class clsMunis_Functions
    Private priv_Comms As New clsMunis_Comms
    Public Function Get_ReqNumber_From_PO(PO As String) As String
        If Not IsNothing(PO) Then
            If PO <> "" Then
                Return priv_Comms.Return_MSSQLValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber")
            Else
                Return Nothing
            End If
        End If
    End Function
    Public Function Get_PO_From_Asset(AssetTag As String) As String
        If Not IsNothing(AssetTag) Then
            If AssetTag <> "" Then
                Return Trim(priv_Comms.Return_MSSQLValue("famaster", "fama_tag", AssetTag, "fama_purch_memo"))
            Else
                Return Nothing
            End If
        End If
    End Function
    Public Function Get_PO_From_Serial(Serial As String) As String
        If Not IsNothing(Serial) Then
            If Serial <> "" Then
                Return Trim(priv_Comms.Return_MSSQLValue("famaster", "fama_serial", Serial, "fama_purch_memo"))
            Else
                Return Nothing
            End If
        End If
    End Function
    Public Function Get_FY_From_Asset(AssetTag As String) As String
        Return Trim(priv_Comms.Return_MSSQLValue("famaster", "fama_tag", AssetTag, "fama_fisc_yr"))
    End Function
    Public Function Get_PODT_From_PO(PO As String) As String
        Return YearFromDate(Trim(priv_Comms.Return_MSSQLValue("RequisitionItems", "PurchaseOrderNumber", PO, "PurchaseOrderDate")))
    End Function
    Public Function Get_VendorName_From_PO(PO As String) As String
        Dim strQRY As String = "SELECT TOP 1       dbo.ap_vendor.a_vendor_number, dbo.ap_vendor.a_vendor_name
FROM            dbo.ap_vendor INNER JOIN
                         dbo.rqdetail ON dbo.ap_vendor.a_vendor_number = dbo.rqdetail.rqdt_sug_vn
WHERE        (dbo.rqdetail.rqdt_req_no = " & Get_ReqNumber_From_PO(PO) & ") AND (dbo.rqdetail.rqdt_fsc_yr = " & Get_FY_From_PO(PO) & ")"
        Dim table As DataTable = priv_Comms.Return_MSSQLTable(strQRY)
        Return table(0).Item("a_vendor_name")
    End Function
    Public Function Get_VendorNumber_From_ReqNumber(ReqNum As String, FY As String) As String
        Dim strQRY As String = "SELECT TOP 1       dbo.ap_vendor.a_vendor_number, dbo.ap_vendor.a_vendor_name
FROM            dbo.ap_vendor INNER JOIN
                         dbo.rqdetail ON dbo.ap_vendor.a_vendor_number = dbo.rqdetail.rqdt_sug_vn
WHERE        (dbo.rqdetail.rqdt_req_no = " & ReqNum & ") AND (dbo.rqdetail.rqdt_fsc_yr = " & FY & ")"
        Dim table As DataTable = priv_Comms.Return_MSSQLTable(strQRY)
        If table.Rows.Count > 0 Then
            Return table(0).Item("a_vendor_number")
        End If
        Return Nothing
    End Function
    Public Function Get_FY_From_PO(PO As String) As String
        Dim strFYyy As String = Left(PO, 2)
        Return "20" + strFYyy
    End Function
    Public Async Function Get_PO_Status(PO As String) As Task(Of String)
        Dim StatusString As String
        Dim StatusCode As String = Await priv_Comms.Return_MSSQLValueAsync("poheader", "pohd_pur_no", PO, "pohd_sta_cd")
        If StatusCode <> "" Then
            StatusString = StatusCode.ToString & " - " & POStatusCodeToLong(StatusCode)
            Return StatusString
        End If
        Return Nothing
    End Function
    Private Function POStatusCodeToLong(Code As Integer) As String
        Select Case Code
            Case 0
                Return "Closed"
            Case 1
                Return "Rejected"
            Case 2
                Return "Created"
            Case 4
                Return "Allocated"
            Case 5
                Return "Released"
            Case 6
                Return "Posted"
            Case 8
                Return "Printed"
            Case 9
                Return "Carry Forward"
            Case 10
                Return "Canceled"
            Case 11
                Return "Closed"
        End Select
        Return Nothing
    End Function
    Public Sub AssetSearch(Parent As Form)
        Try
            Dim Device As New Device_Info
            Device.dtPurchaseDate = Nothing
            Dim NewDialog As New MyDialog(Parent)
            With NewDialog
                .Text = "Asset Search"
                .AddTextBox("txtAsset", "Asset:")
                .AddTextBox("txtSerial", "Serial:")
                .ShowDialog()
                If .DialogResult = DialogResult.OK Then
                    Device.strAssetTag = Trim(NewDialog.GetControlValue("txtAsset"))
                    Device.strSerial = Trim(NewDialog.GetControlValue("txtSerial"))
                    NewMunisView_Device(Device, Parent)
                End If
            End With
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Sub NameSearch(Parent As Form)
        Dim NewDialog As New MyDialog(Parent)
        Dim strName As String
        With NewDialog
            .Text = "Org/Object Code Search"
            .AddTextBox("txtName", "First or Last Name:")
            .ShowDialog()
            If .DialogResult = DialogResult.OK Then strName = NewDialog.GetControlValue("txtName")
        End With
        If Trim(strName) IsNot "" Then
            NewMunisView_NameSearch(strName, Parent)
        End If
    End Sub
    Public Sub POSearch(Parent As Form)
        Try
            Dim PO, FY As String
            Dim NewDialog As New MyDialog(Parent)
            With NewDialog
                .Text = "PO Search"
                .AddTextBox("txtPO", "PO #:")
                .AddTextBox("txtFY", "FY:")
                .ShowDialog()
                If .DialogResult = DialogResult.OK Then
                    PO = NewDialog.GetControlValue("txtPO")
                    FY = NewDialog.GetControlValue("txtFY")
                    NewMunisView_POSearch(PO, Parent)
                End If
            End With
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Sub ReqSearch(Parent As Form)
        Try
            'Dim blah As String = InputBox("Enter Requisition # followed by FY separated by a comma.  (Format #########,YYYY", "Req Search", "")
            'If Trim(blah) IsNot "" Then
            '    Dim splitValues() As String = Split(blah, ",")
            '    Dim ReqNumber As String = splitValues(0)
            '    Dim FY As String = splitValues(1)
            '    NewMunisView_ReqSearch(ReqNumber, FY)
            'End If
            Dim ReqNumber, FY As String
            Dim NewDialog As New MyDialog(Parent)
            With NewDialog
                .Text = "Req Search"
                .AddTextBox("txtReqNum", "Requisition #:")
                .AddTextBox("txtFY", "FY:")
                .ShowDialog()
                If .DialogResult = DialogResult.OK Then
                    ReqNumber = NewDialog.GetControlValue("txtReqNum")
                    FY = NewDialog.GetControlValue("txtFY")
                    NewMunisView_ReqSearch(ReqNumber, FY, Parent)
                End If
            End With
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Function ListOfEmpBySup(SupEmpNum As String) As DataTable
        Dim strQRY As String = "SELECT TOP 100 a_employee_number FROM pr_employee_master WHERE e_supervisor='" & SupEmpNum & "'"
        Return priv_Comms.Return_MSSQLTable(strQRY)
    End Function
    Public Sub NewMunisView_NameSearch(Name As String, Optional Parent As Form = Nothing)
        Waiting()
        Dim NewMunis As New View_Munis(Parent, True)
        NewMunis.LoadMunisEmployeeByLastName(Name)
        DoneWaiting()
    End Sub
    Public Sub NewMunisView_POSearch(PO As String, Optional Parent As Form = Nothing)
        Waiting()
        Dim NewMunis As New View_Munis(Parent, False)
        NewMunis.LoadMunisPOGridByPONo(PO)
        DoneWaiting()
    End Sub
    Public Sub NewMunisView_ReqSearch(ReqNumber As String, FY As String, Optional Parent As Form = Nothing)
        Waiting()
        Dim NewMunis As New View_Munis(Parent, True)
        NewMunis.LoadMunisRequisitionGridByReqNo(ReqNumber, FY)
        DoneWaiting()
    End Sub
    Public Sub NewMunisView_Device(Device As Device_Info, Optional Parent As Form = Nothing)
        Waiting()
        Dim NewMunis As New View_Munis(Parent)
        NewMunis.LoadMunisInfoByDevice(Device)
        DoneWaiting()
    End Sub
    Private Sub Waiting()
        SetCursor(Cursors.WaitCursor)
    End Sub
    Private Sub DoneWaiting()
        SetCursor(Cursors.Default)
    End Sub
End Class
