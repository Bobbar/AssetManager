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
End Module
