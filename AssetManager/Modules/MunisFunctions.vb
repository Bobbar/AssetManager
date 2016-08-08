Module MunisFunctions
    Public Function Munis_GetReqNumberFromPO(PO As String) As String
        Return ReturnMSSQLValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber")
    End Function
    Public Function Munis_GetPOFromAsset(AssetTag As String) As String
        Return Trim(ReturnMSSQLValue("famaster", "fama_tag", AssetTag, "fama_purch_memo"))
    End Function
    Public Function Munis_GetPOFromSerial(AssetTag As String) As String
        Return Trim(ReturnMSSQLValue("famaster", "fama_serial", AssetTag, "fama_purch_memo"))
    End Function
    Public Function Munis_GetFYFromAsset(AssetTag As String) As String
        Return Trim(ReturnMSSQLValue("famaster", "fama_tag", AssetTag, "fama_fisc_yr"))
    End Function
    Public Function Munis_GetFYFromPO(PO As String) As String
        Return YearFromDate(Trim(ReturnMSSQLValue("RequisitionItems", "PurchaseOrderNumber", PO, "PurchaseOrderDate")))
    End Function
    Public Sub Munis_NameSearch()
        Dim blah As String = InputBox("Enter a full or patial first or last name of the Employee.", "Org/Object Code Search", "")
        If Trim(blah) IsNot "" Then
            NewMunisView_NameSearch(blah)
        End If
    End Sub
    Public Sub Munis_POSearch()
        Dim blah As String = InputBox("Enter PO # followed by FY separated by a comma.  (Format ########,YYYY", "PO Search", "")
        If Trim(blah) IsNot "" Then
            Dim splitValues() As String = Split(blah, ",")
            Dim PO As String = splitValues(0)
            Dim FY As String = splitValues(1)
            NewMunisView_POSearch(PO, FY)
        End If
    End Sub
    Public Sub Munis_ReqSearch()
        Dim blah As String = InputBox("Enter Requisition # followed by FY separated by a comma.  (Format #########,YYYY", "Req Search", "")
        If Trim(blah) IsNot "" Then
            Dim splitValues() As String = Split(blah, ",")
            Dim ReqNumber As String = splitValues(0)
            Dim FY As String = splitValues(1)
            NewMunisView_ReqSearch(ReqNumber, FY)
        End If
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
        NewMunis.LoadMunisRequisitionGridByPO(PO, FY)
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
