Module MunisFunctions

    Public Function GetMunisRequsitionTable(PO As String) As DataTable








    End Function


    Public Function GetReqNumberFromPO(PO As String) As String

        Return ReturnMSSQLValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber")



    End Function



End Module
