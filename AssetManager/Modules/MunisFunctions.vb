Module MunisFunctions
    Public Function Munis_GetReqNumberFromPO(PO As String) As String
        Return ReturnMSSQLValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber")
    End Function
    Public Function Munis_GetPOFromAsset(AssetTag As String) As String
        Return ReturnMSSQLValue("famaster", "fama_tag", AssetTag, "fama_purch_memo")
    End Function
    Public Function Munis_GetFYFromAsset(AssetTag As String) As String
        Return ReturnMSSQLValue("famaster", "fama_tag", AssetTag, "fama_fisc_yr")
    End Function
End Module
