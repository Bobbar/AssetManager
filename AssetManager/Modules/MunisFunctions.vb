Module MunisFunctions
    Public Function Munis_GetReqNumberFromPO(PO As String) As String
        Return Return_MSSQLValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber")
    End Function
    Public Function Munis_GetPOFromAsset(AssetTag As String) As String
        Return Trim(Return_MSSQLValue("famaster", "fama_tag", AssetTag, "fama_purch_memo"))
    End Function
    Public Function Munis_GetPOFromSerial(AssetTag As String) As String
        Return Trim(Return_MSSQLValue("famaster", "fama_serial", AssetTag, "fama_purch_memo"))
    End Function
    Public Function Munis_GetFYFromAsset(AssetTag As String) As String
        Return Trim(Return_MSSQLValue("famaster", "fama_tag", AssetTag, "fama_fisc_yr"))
    End Function
    Public Function Munis_GetFYFromPO(PO As String) As String
        Return YearFromDate(Trim(Return_MSSQLValue("RequisitionItems", "PurchaseOrderNumber", PO, "PurchaseOrderDate")))
    End Function
    Public Function Munis_Get_VendorName_From_PO(PO As String) As String
        Dim strQRY As String = "SELECT TOP 1       dbo.ap_vendor.a_vendor_number, dbo.ap_vendor.a_vendor_name
FROM            dbo.ap_vendor INNER JOIN
                         dbo.rqdetail ON dbo.ap_vendor.a_vendor_number = dbo.rqdetail.rqdt_sug_vn
WHERE        (dbo.rqdetail.rqdt_req_no = " & Munis_GetReqNumberFromPO(PO) & ") AND (dbo.rqdetail.rqdt_fsc_yr = " & Munis_Get_FY_From_PO(PO) & ")"
        Dim table As DataTable = Return_MSSQLTable(strQRY)
        Return table(0).Item("a_vendor_name")
    End Function
    Private Function Munis_Get_FY_From_PO(PO As String) As String
        Dim strFYyy As String = Left(PO, 2)
        Return "20" + strFYyy
    End Function
    Public Sub Munis_NameSearch()
        Dim blah As String = InputBox("Enter a full or patial first or last name of the Employee.", "Org/Object Code Search", "")
        If Trim(blah) IsNot "" Then
            NewMunisView_NameSearch(blah)
        End If
    End Sub
    Public Sub Munis_POSearch()
        Try
            Dim blah As String = InputBox("Enter PO # followed by FY separated by a comma.  (Format ########,YYYY", "PO Search", "")
            If Trim(blah) IsNot "" Then
                Dim splitValues() As String = Split(blah, ",")
                Dim PO As String = splitValues(0)
                Dim FY As String = splitValues(1)
                NewMunisView_POSearch(PO, FY)
            End If
        Catch
        End Try
    End Sub
    Public Sub Munis_ReqSearch()
        Try
            Dim blah As String = InputBox("Enter Requisition # followed by FY separated by a comma.  (Format #########,YYYY", "Req Search", "")
            If Trim(blah) IsNot "" Then
                Dim splitValues() As String = Split(blah, ",")
                Dim ReqNumber As String = splitValues(0)
                Dim FY As String = splitValues(1)
                NewMunisView_ReqSearch(ReqNumber, FY)
            End If
        Catch
        End Try
    End Sub
    Private Sub NewMunisView_NameSearch(Name As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewMunis As New View_Munis
        NewMunis.HideFixedAssetGrid()
        NewMunis.Show()
        NewMunis.LoadMunisEmployeeByLastName(Name)
    End Sub
    Private Sub NewMunisView_POSearch(PO As String, FY As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewMunis As New View_Munis
        NewMunis.HideFixedAssetGrid()
        NewMunis.Show()
        NewMunis.LoadMunisRequisitionGridByReqNo(Munis_GetReqNumberFromPO(PO), Munis_GetFYFromPO(PO)) 'LoadMunisRequisitionGridByPO(PO, FY)
    End Sub
    Private Sub NewMunisView_ReqSearch(ReqNumber As String, FY As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewMunis As New View_Munis
        NewMunis.HideFixedAssetGrid()
        NewMunis.Show()
        NewMunis.LoadMunisRequisitionGridByReqNo(ReqNumber, FY)
    End Sub
End Module
