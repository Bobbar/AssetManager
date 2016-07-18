Module MunisFunctions
    Public Function GetReqNumberFromPO(PO As String) As String
        Return ReturnMSSQLValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber")
    End Function
End Module
